using CarRental.Domain.Abstract;
using CarRental.Domain.Entities;
using CarRental.Domain.Extentions;
using CarRental.Domain.Models;
using CarRental.Infrastructure;
using CarRental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CarRental.Controllers
{
    [AdminAuth]
    [Authorize]
    public class AdminController : Controller
    {
        private IRentalRepository repository;
        private IUserRepository _userRepository;
        private int pageSize = 5;
        public AdminController(IRentalRepository rentalRepository, IUserRepository userRepository)
        {
            repository = rentalRepository;
            _userRepository = userRepository;
        }

        // GET: Admin
        public async Task<ActionResult> Index()
        {
            return View();
        }
        public async Task<ActionResult> Cars()
        {

                var cars = await repository.GetCars();
                return View(cars);

        }
        public ActionResult AddLicense(int id)
        {
             return View(new AddLicenseViweModel {CarId = id });
        }
        [HttpPost]
        public async Task<ActionResult> AddLicense(AddLicenseViweModel licenseModel)
        {
            if (ModelState.IsValid)
            {
                await repository.AddLicense(licenseModel.CarId, licenseModel.LicesneNumber);
                return RedirectToAction("Cars");
            }
            return View(licenseModel);
        }
        public async Task<ActionResult> Edit(int id = 0)
        {
            var car = await repository.GetRentalById(id);
            return View(car);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(RentalRecord rentalRecord, HttpPostedFileBase image = null)
        {
            var car = await repository.EditCar(rentalRecord, image);
            return RedirectToAction("Cars");
        }
        public async Task<ActionResult> RemoveLicensePlate(int id)
        {
            LicesnePlate license = await repository.RemoveLicense(id);
            var car = await repository.GetCarById(license.CarId);
            var rentalRecord = await repository.GetRentalByName(car.Brand, car.Model);
            TempData["message"] = $"Numer rejestracyjny {license.LicenseNumber} został usunięty";
            return RedirectToAction("Edit", new { id = rentalRecord.Id });
        }
        [HttpPost]
        public async Task<ActionResult> RemoveCar(int id)
        {
            var car = await repository.RemoveCar(id);
            if (car != null)
                TempData["message"] = $"Wszystkie samochody typu {car} zostały usunięte.";
            return Json(new { redirectToUrl = Url.Action("Cars", "Admin") });
        }
        public ActionResult AddCar()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AddCar(RentalRecord rentalRecord, HttpPostedFileBase image = null)
        {
            var response = await repository.AddCar(rentalRecord, image);
            if (response != null)
                TempData["message"] = "Samochód został dodany";
            else
                TempData["message"] = "Taki samochód już widnieje w bazie";
            return RedirectToAction("Cars");
        }
        public async Task<ActionResult> Users()
        {
            var users = await _userRepository.GetUsers();
            return View(users);
        }
        public async Task<ActionResult> EditUser(string userName)
        {
            var user = await _userRepository.GetUser(userName);
            var userEditModel = new RegisterViewModel
            {
                UserName = user.UserName,
                Name = user.Name,
                Surname = user.Surname,
                AccountType = user.AccountType,
                Admin = true
            };
            return View(userEditModel);
        }
        [HttpPost]
        public async Task<ActionResult> EditUser(RegisterViewModel userModel)
        {
            var user = await _userRepository.GetUser(userModel.UserName);
            user.Name = userModel.Name;
            user.Surname = userModel.Surname;
            user.AccountType = userModel.AccountType;
            if (await _userRepository.AddUser(user))
                TempData["message"] = "Użytkownik został edytowany";
            return RedirectToAction("Users");
        }

        public async Task<ActionResult> ResetPassword(string username)
        {
            var user = await _userRepository.GetUser(username);
            var response = await _userRepository.ResetPassword(user);
            if (response != null)
                TempData["message"] = response;
            return RedirectToAction("Users");
        }

        public async Task<ActionResult> GetAllLoans(bool past = false, int page = 1, UserDataModel userDataModel = null)
        {
            ViewBag.past = past;
            ViewBag.Admin = true;
            Paging paging = new Paging
            {
                ItemsPerPage = pageSize,
                CurrentPage = page
            };
            var loans = await repository.GetAllLoans(past, paging, userDataModel);
            return View(loans);
        }

        public async Task<ActionResult> AcceptRent(int loanId)
        {
            await repository.AcceptRent(loanId);
            return Json(new { redirectToUrl = Url.Action("GetAllLoans", "Admin") });
        }

        [HttpPost]
        public async Task<ActionResult> ReturnCar(int id)
        {
            if (await repository.ReturnCar(id))
                TempData["message"] = "Samochód został zwrócony";
            return Json(new { redirectToUrl = Url.Action("GetAllLoans", "Admin") });
        }

    }
}