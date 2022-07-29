using CarRental.App_Start;
using CarRental.Domain.Abstract;
using CarRental.Domain.Entities;
using CarRental.Domain.Models;
using CarRental.DTOs;
using CarRental.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace CarRental.Domain.Concrete
{
    public class UserRepository : IUserRepository
    {
        private DataContext context = new DataContext();

        public async Task<string> GetAdminUsers()
        {
            var admins = await context.Users.Where(u => u.AccountType == Role.Administrator).ToListAsync();
            string toReturn = "";
            foreach(var admin in admins)
            {
                toReturn += admin.UserName + ",";
            }
            return toReturn;
        }
        public async Task<bool> AddUser(User user)
        {
            if (user.Id == 0)
                if (! await context.Users.AnyAsync(x => x.UserName == user.UserName))
                {
                    user.Name = TitleCase(user.Name);
                    user.Surname = TitleCase(user.Surname);
                    context.Users.Add(user);
                }
                else return false;
            else
            {
                user.Name = TitleCase(user.Name);
                user.Surname = TitleCase(user.Surname);
                context.Entry(user).State = EntityState.Modified;
            }
            return await SaveAllChanges();
        }

        public async Task<string> ResetPassword(User user)
        {
            var hmac = new HMACSHA512();
            string password = Membership.GeneratePassword(8, 3);
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            user.PasswordSalt = hmac.Key;
            context.Entry(user).State = EntityState.Modified;
            await SaveAllChanges();
            return "Hasło zostało zmienione. Nowe hasło to "+password;
        }
        public async Task<bool> ChangePassword(User user, string oldPassword, string newPassword)
        {
            var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(oldPassword));
            for(int i = 0; i < user.PasswordHash.Length; i++)
            {
                if(user.PasswordHash[i] != computedHash[i])
                {
                    return false;
                }
            }
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(newPassword));
            context.Entry(user).State = EntityState.Modified;
            return await SaveAllChanges();
        }


        public async Task<User> GetUser(string username)
        {
            return await context.Users.SingleOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await context.Users.ToListAsync();
        }

        public async Task<bool> CheckUser(string username, string password)
        {
            var user = await context.Users.SingleOrDefaultAsync(u => u.UserName == username);
            if (user == null)
                return false;
            var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            for(int i = 0; i < user.PasswordHash.Length; i++)
            {
                if(user.PasswordHash[i] != computedHash[i])
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<LoanDtoModel> GetRentedCars(User user, bool past, Paging paging)
        {
            var rentedCars = await context.Loans
                .Where(u => u.UserId == user.Id)
                .Where(c => past ? c.Returned == true : c.Returned == false)
                .Where(l => l.Rented).ToListAsync();
            rentedCars.AddRange(await context.Loans.OrderBy(l => l.LoanDate)
                .Where(u => u.UserId == user.Id)
                .Where(c => past ? c.Returned == true : c.Returned == false)
                .Where(l => !l.Rented)
                .ToListAsync());
            var rentedCarstoReturn = rentedCars
                .Select(l => new LoanDto { 
                LoanId = l.Id,
                LoanUser = context.Users.Where(u => u.Id == l.UserId).FirstOrDefault(),
                LicensePlate = l.LicensePlate,
                Car = context.Cars.Where(c => c.Id == l.CarId).FirstOrDefault(),
                Price = l.Price,
                Caution = l.Caution,
                LoanDate = l.LoanDate,
                ReturnDate = l.ReturnDate,
                Returned = l.Returned,
                Rented = l.Rented
            }).ToList();
            paging.TotalItems = rentedCars.Count();

            return new LoanDtoModel
            {
                Loans = rentedCarstoReturn.Skip((paging.CurrentPage - 1) * paging.ItemsPerPage).Take(paging.ItemsPerPage),
                Paging = paging
            };
        }

        public async Task<bool> SaveAllChanges()
        {
            return await context.SaveChangesAsync() > 0;
        }
        private string TitleCase(string text)
        {
            string textToReturn = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (i == 0)
                    textToReturn += char.ToUpper(text[i]);
                else
                    textToReturn += char.ToLower(text[i]);
            }
            return textToReturn;
        }
    }
}
