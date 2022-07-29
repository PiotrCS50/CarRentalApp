using CarRental.App_Start;
using CarRental.Domain.Abstract;
using CarRental.Domain.DTOs;
using CarRental.Domain.Entities;
using CarRental.Domain.Models;
using CarRental.DTOs;
using CarRental.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CarRental.Domain.Concrete
{
    public class CarRentalRepository : IRentalRepository
    {
        private DataContext context = new DataContext();

        public async Task<RentalRecord> AddCar(RentalRecord car, HttpPostedFileBase image)
        {
            
            if (car.Id == 0)
            {
                if (await context.Cars.AnyAsync(c => c.Brand == car.Car.Brand && c.Model == car.Car.Model))
                {
                    if (await context.Rental.AnyAsync(c => c.Car.Brand == car.Car.Brand && c.Car.Model == car.Car.Model))
                        return null;
                    else
                        car.Car = await context.Cars.Where(c => c.Brand == car.Car.Brand && c.Model == car.Car.Model).SingleOrDefaultAsync();
                }
                if (image != null)
                {
                    car.Car.ImageMimeType = image.ContentType;
                    car.Car.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(car.Car.ImageData, 0, image.ContentLength);
                }
                context.Rental.Add(car);
                await SaveAllChaneges();
                return car;
            }
            return null;
        }

        public async Task<RentalRecord> EditCar(RentalRecord car, HttpPostedFileBase image )
        {
            if(image != null)
            {
                car.Car.ImageMimeType = image.ContentType;
                car.Car.ImageData = new byte[image.ContentLength];
                image.InputStream.Read(car.Car.ImageData, 0, image.ContentLength);
            }
            context.Entry(car).State = EntityState.Modified;
            context.Entry(car.Car).State = EntityState.Modified;
            await SaveAllChaneges();
            return car;        
        }

        public async Task<string> RemoveCar(int carId)
        { 
            var rentalRecord = await context.Rental.Include(c => c.Car).Where(c => c.Id == carId).SingleOrDefaultAsync();
            string stringToReturn = rentalRecord.Car.Brand + " " + rentalRecord.Car.Model;
            var car = rentalRecord.Car;

            var licensePlates = await context.LicensePlates.Where(c => c.CarId == rentalRecord.Car.Id).ToListAsync();
            foreach(var item in licensePlates)
            {
                context.Entry(item).State = EntityState.Deleted;
            }
            context.Entry(rentalRecord).State = EntityState.Deleted;
            await SaveAllChaneges();
            if (!await context.Loans.AnyAsync(c => c.CarId == car.Id))
            {
                context.Entry(car).State = EntityState.Deleted;
                await SaveAllChaneges();
            }
            return stringToReturn;
        }

        public async Task<bool> AddLicense(int carId, string licenseNumber)
        {
            if (await context.LicensePlates.AnyAsync(l => l.LicenseNumber == licenseNumber))
                return false;
            var car = await context.Cars.Where(c => c.Id == carId).Include(c => c.LicensePlates).SingleOrDefaultAsync();
            if (car.LicensePlates == null)
                car.LicensePlates = new List<LicesnePlate>();
            car.LicensePlates.Add(new LicesnePlate { CarId = carId, LicenseNumber = licenseNumber });
            context.Entry(car).State = EntityState.Modified;
            return await SaveAllChaneges();
        }

        public async Task<LicesnePlate> RemoveLicense(int licenseId)
        {
            var license = await context.LicensePlates.Where(l => l.Id == licenseId).SingleOrDefaultAsync();
            context.Entry(license).State = EntityState.Deleted;
            await SaveAllChaneges();
            return license;

        }
        public async Task<RentalRecord> GetRentalByName(string carBrand, string carModel)
        {
            return await context.Rental.Include(r => r.Car).Include(c => c.Car.LicensePlates).Where(c => c.Car.Brand == carBrand && c.Car.Model == carModel).SingleOrDefaultAsync();
        }

        public async Task<RentalRecord> GetRentalById(int carId)
        {
            return await context.Rental.Where(c => c.Id == carId).Include(c => c.Car.LicensePlates).SingleOrDefaultAsync();
        }
        public async Task<Car> GetCarById(int carId)
        {
            return await context.Cars.Where(c => c.Id == carId).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<RentalRecord>> GetCars()
        {
            return await context.Rental.Include(r => r.Car).Include(c => c.Car.LicensePlates).ToListAsync();
        }

        public async Task<CarListModel> GetFreeCars(DateTime start, DateTime end, Paging paging, FilterModel filter)
        {
        
            bool carAvaliable = false;
            List<RentalRecordDto> freeCars = new List<RentalRecordDto>();

            var carss = await context.Rental.Include(c => c.Car).Include(c => c.Car.LicensePlates).OrderBy(c => c.Car.Brand).ToListAsync();
            var cars = carss.
                Select(x => new RentalRecordDto
                {
                    Id = x.Id,
                    Car = x.Car,
                    Caution = x.Caution,
                    Description = x.Description,
                    Price = x.Price,
                    NumberOfLoans = context.Loans.Where(l => l.CarId == x.Car.Id).Count()
                }).Where(x => filter.CarBrands.Any(y => y.Name == x.Car.Brand && y.CheckBoxValue == true))
                .Where(x => filter.CarTypes.Any(y => y.Name == x.Car.Type && y.CheckBoxValue == true))
                .Where(x => x.Price >= filter.MinPrice && x.Price <= filter.MaxPrice).ToList();

            paging.TotalItems = cars.Count();

            //var cars = await context.Rental.Include(c=>c.Car).Include(c => c.Car.LicensePlates).OrderBy(c => c.Car.Brand).ToListAsync();
            
            foreach (var car in cars)
            {
                //var licensePlates = context.LicensePlates.Where(l => l.CarId == car.Car.Id).ToList();
                foreach (var plate in car.Car.LicensePlates)
                {
                    var carLoans = await context.Loans.Where(l => l.CarId == car.Car.Id && l.LicensePlate == plate.LicenseNumber).ToListAsync();
                    if (checkAvailability(carLoans, start, end))
                        carAvaliable = true;
                }
                if (carAvaliable)
                {
                    freeCars.Add(car);
                    carAvaliable = false;
                }
            }

            if (filter.SortOprion == SortOprions.popularity)
                freeCars = freeCars.OrderByDescending(x => x.NumberOfLoans).ToList();
            else if (filter.SortOprion == SortOprions.priceDown)
                freeCars = freeCars.OrderByDescending(x => x.Price).ToList();
            else
                freeCars = freeCars.OrderBy(x => x.Price).ToList();

            return new CarListModel { Cars = freeCars.Skip((paging.CurrentPage - 1) * paging.ItemsPerPage).Take(paging.ItemsPerPage),
                Paging = paging, Filter = filter };
        }

        public async Task<IEnumerable<Loan>> GetUserLoans(User user)
        {
            return await context.Loans.Where(u => u.UserId == user.Id).ToListAsync();
        }
        public async Task<LoanDtoModel> GetAllLoans(bool past, Paging paging, UserDataModel userDataModel)
        {
            List<LoanDto> loans = new List<LoanDto>();
            var allLoans = await context.Loans
            .Where(x => past ? x.Returned : !x.Returned)
            .Where(l => l.Rented)
            .OrderBy(x => x.LoanDate).ToListAsync();

            allLoans.AddRange(await context.Loans
                .Where(x => past ? x.Returned : !x.Returned)
                .Where(l => !l.Rented)
                .OrderBy(x => x.LoanDate).ToListAsync());
            loans = allLoans
            .Select(x =>
            new LoanDto
            {
                Car = context.Cars.Where(c => c.Id == x.CarId).FirstOrDefault(),
                Caution = x.Caution,
                LicensePlate = x.LicensePlate,
                LoanDate = x.LoanDate,
                ReturnDate = x.ReturnDate,
                LoanId = x.Id,
                LoanUser = context.Users.Where(u => u.Id == x.UserId).FirstOrDefault(),
                Price = x.Price,
                Rented = x.Rented,
                Returned = x.Returned,
                CarReturnedDate = x.CarReturnedDate,
            }).ToList();
            if (userDataModel != null && !string.IsNullOrEmpty(userDataModel.Name) && !string.IsNullOrWhiteSpace(userDataModel.Name)
                && !string.IsNullOrEmpty(userDataModel.Surname) && !string.IsNullOrWhiteSpace(userDataModel.Surname))
                loans = loans.Where(l => l.LoanUser.Name == userDataModel.Name && l.LoanUser.Surname == userDataModel.Surname).ToList();
            paging.TotalItems = loans.Count();
            return new LoanDtoModel { Loans = loans.Skip((paging.CurrentPage - 1) * paging.ItemsPerPage).Take(paging.ItemsPerPage), Paging = paging };
        }

        public async Task<LoanDto> Rent(LoanDto loanDto)
        {
            var licensePlates = await context.LicensePlates.Where(l => l.CarId == loanDto.Car.Id).ToListAsync();
            foreach (var licensePlate in licensePlates)
            {
                var loans = await context.Loans.Where(c => c.CarId == loanDto.Car.Id && c.LicensePlate == licensePlate.LicenseNumber).ToListAsync();
                if (checkAvailability(loans, loanDto.LoanDate, loanDto.ReturnDate))
                {
                    var loan = new Loan()
                    {
                        UserId = loanDto.LoanUser.Id,
                        CarId = loanDto.Car.Id,
                        LicensePlate = licensePlate.LicenseNumber,
                        Price = loanDto.Price,
                        Caution = loanDto.Caution,
                        LoanDate = loanDto.LoanDate,
                        ReturnDate = loanDto.ReturnDate,
                        Returned = false,
                        Rented = false,
                        CarReturnedDate = null
                    };
                    context.Loans.Add(loan);
                    await SaveAllChaneges();

                    loanDto.Rented = false;
                    loanDto.LicensePlate = licensePlate.LicenseNumber;
                    Loan last = await context.Loans.Take(1).OrderByDescending(l => l.Id).SingleOrDefaultAsync();
                    loanDto.LoanId = last.Id;
                    return loanDto;
                }
            }
            return null;
        }

        public async Task<bool> AcceptRent(int loanId)
        {
            var loan  = await context.Loans.Where(l => l.Id == loanId).FirstOrDefaultAsync();
            loan.Rented = true;
            context.Entry(loan).State = EntityState.Modified;
            return await SaveAllChaneges();            
        }

        public async Task<bool> ReturnCar(int loanId)
        {
            var loan = await context.Loans.Where(l => l.Id == loanId).SingleOrDefaultAsync();
            loan.CarReturnedDate = DateTime.Today;
            loan.Returned = true;
            loan.Rented = false;
            context.Entry(loan).State = EntityState.Modified;
            return await SaveAllChaneges();
        }
        public async Task<Loan> Cancel(int loanId)
        {
            var loan = await context.Loans.Where(l => l.Id == loanId).SingleOrDefaultAsync();
            if (loan != null)
            {
                context.Loans.Remove(loan);
                context.Entry(loan).State = EntityState.Deleted;
                await SaveAllChaneges();
            }
            return loan;
        }

        public async Task<bool> SaveAllChaneges()
        {
            return await context.SaveChangesAsync() > 0;
        }
        public async Task<bool> LoanExist(int loanId)
        {
            var loan = await context.Loans.Where(l => l.Id == loanId).SingleOrDefaultAsync();
            if (loan == null)
                return false;
            return true;
        }
        public async Task<IEnumerable<string>> GetCarBrands()
        {
            return await context.Cars.Select(c => c.Brand).Distinct().OrderBy(b => b).ToListAsync();
        }
        public async Task<IEnumerable<string>> GetCarTypes()
        {
            return await context.Cars.Select(c => c.Type).Distinct().OrderBy(b => b).ToListAsync();
        }
        private bool checkAvailability(IEnumerable<Loan> loans, DateTime start, DateTime end)
        {
            foreach (var loan in loans)
            {
                if (loan.CarReturnedDate == null)
                {
                    if (!(((start < loan.LoanDate && end < loan.LoanDate) || (start > loan.ReturnDate && end > loan.ReturnDate))))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!(((start < loan.LoanDate && end < loan.LoanDate) || (start >= loan.CarReturnedDate && end >= loan.CarReturnedDate)) && loan.Returned))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
