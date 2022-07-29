using CarRental.Domain.Entities;
using CarRental.Domain.Models;
using CarRental.DTOs;
using CarRental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Abstract
{
    public interface IUserRepository
    {
        Task<string> GetAdminUsers();
        Task<bool> AddUser(User user);
        Task<string> ResetPassword(User user);
        Task<bool> ChangePassword(User user, string oldPassword, string newPassword);
        Task<User> GetUser(string Username);
        Task<IEnumerable<User>> GetUsers();
        Task<bool> CheckUser(string username, string password);
        Task<LoanDtoModel> GetRentedCars(User user, bool past, Paging paging);
        Task<bool> SaveAllChanges();
    }
}
