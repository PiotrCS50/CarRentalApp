using CarRental.Domain.Entities;
using CarRental.Domain.Models;
using CarRental.DTOs;
using CarRental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CarRental.Domain.Abstract
{
    public interface IRentalRepository
    {
        Task<IEnumerable<RentalRecord>> GetCars();
        Task<RentalRecord> GetRentalByName(string carBrand, string carModel);
        Task<RentalRecord> GetRentalById(int caId);
        Task<Car> GetCarById(int carId);
        Task<CarListModel> GetFreeCars(DateTime start, DateTime end, Paging paging, FilterModel filter);
        Task<IEnumerable<Loan>> GetUserLoans(User user);
        Task<LoanDtoModel> GetAllLoans(bool past, Paging paging, UserDataModel userDataModel);
        Task<RentalRecord> EditCar(RentalRecord car, HttpPostedFileBase image);
        Task<string> RemoveCar(int carId);
        Task<RentalRecord> AddCar(RentalRecord car, HttpPostedFileBase image);
        Task<bool> AddLicense(int carId, string licenseNumber);
        Task<LicesnePlate> RemoveLicense(int licenseId);
        Task<LoanDto> Rent(LoanDto loanDto);
        Task<bool> ReturnCar(int loanId);
        Task<Loan> Cancel(int loanId);
        Task<bool> LoanExist(int loanId);
        Task<IEnumerable<string>> GetCarBrands();
        Task<IEnumerable<string>> GetCarTypes();
        Task<bool> SaveAllChaneges();
        Task<bool> AcceptRent(int loanId);
    }
}
