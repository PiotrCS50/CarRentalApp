using CarRental.App_Start;
using CarRental.Domain.Abstract;
using CarRental.Domain.Concrete;
using CarRental.Domain.Entities;
using CarRental.Domain.Models;
using CarRental.DTOs;
using CarRental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CarRental.Controllers
{
    public class HomeController : Controller
    {
        private IRentalRepository _repository;
        private IUserRepository _userRepository;
        private int pageSize = 5;
        public HomeController(IRentalRepository repository, IUserRepository userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        public ActionResult Index()
        {
            RentTimeModelView rent = new RentTimeModelView { LoanDate = DateTime.Today, ReturnDate = DateTime.Today };
            return View(rent);
        }
        [HttpPost]
        public ActionResult SaveDates(RentTimeModelView rentTime)
        {
            if (ModelState.IsValid)
            {
                if (rentTime.LoanDate > rentTime.ReturnDate)
                {
                    ModelState.AddModelError("DateError", "Proszę podać poprawną datę, data zwrotu jest wcześniejsza niż data odbioru!");
                    return View("Index", rentTime);
                }

                Session["Days"] = (rentTime.ReturnDate - rentTime.LoanDate).Days + 1;
                Session["LoanDate"] = rentTime.LoanDate;
                Session["ReturnDate"] = rentTime.ReturnDate;

                return RedirectToAction("FreeCars");
            }
            else
            {
                return View("Index", rentTime);
            }
        }
        public async Task<ActionResult> FreeCars(int page = 1)
        {
            if (Session["LoanDate"] == null || Session["ReturnDate"] == null)
                return RedirectToAction("Index");
            if (Session["Filter"] == null)
                return RedirectToAction("FilterCars");
            FilterModel filter = (FilterModel)Session["Filter"];
            DateTime loanDate = (DateTime)Session["LoanDate"];
            DateTime retunrDate = (DateTime)Session["ReturnDate"];
            
            var cars = await _repository.GetFreeCars(loanDate, retunrDate, new Paging {
                CurrentPage = page,
                ItemsPerPage = pageSize,
            }, filter);


            //CarsListViewModel model = new CarsListViewModel
            //{
            //    Cars = cars.Skip((page-1)*pageSize).Take(pageSize),
            //    Paging = new Paging
            //    {
            //        CurrentPage = page,
            //        ItemsPerPage = pageSize,
            //        TotalItems = cars.Count()
            //    }
            //};
            
            return View(cars);
        }
        public async Task<ActionResult> FilterCars([Bind(Prefix = "Filter")]FilterModel filterData = null)
        {
            if (filterData != null && filterData.CarBrands != null && filterData.CarTypes != null )
            {
                if (filterData.MinPrice > filterData.MaxPrice)
                    filterData.MaxPrice = filterData.MinPrice + 1;
                Session["Filter"] = filterData;
                return RedirectToAction("FreeCars");
            }
            else
            {
                List<CheckBoxField> carTypes = new List<CheckBoxField>();
                foreach(var type in await _repository.GetCarTypes())
                {
                    carTypes.Add(new CheckBoxField { CheckBoxValue = true, Name = type });
                }
                List<CheckBoxField> carBrands = new List<CheckBoxField>();
                foreach (var brand in await _repository.GetCarBrands())
                {
                    carBrands.Add(new CheckBoxField { CheckBoxValue = true, Name = brand });
                }
                filterData = new FilterModel
                {
                    CarBrands = carBrands,
                    CarTypes = carTypes,
                    MaxPrice = 10000,
                    MinPrice = 1,
                    SortOprion = SortOprions.popularity
                };
                Session["Filter"] = filterData;
                return RedirectToAction("FreeCars");
            }
        }

        public async Task<ActionResult> CarDetails(string carBrand = null,string carModel = null)
        {
            
            if (carBrand != null && carModel != null)
            {
                ViewBag.rentTime = new RentTimeModelView() { ReturnDate = (DateTime)Session["ReturnDate"], LoanDate = (DateTime)Session["LoanDate"] };
                RentalRecord car = await _repository.GetRentalByName(carBrand, carModel);
                return View(car);
            }
            else
                return RedirectToAction("Index");
        }
        [Authorize]
        public async Task<ActionResult> ConfirmRezervation()
        {
            ViewBag.NotRented = true;
            var rentalCar = await _repository.GetRentalByName((string)RouteData.Values["carBrand"], (string)RouteData.Values["carModel"]);
            var loanDto = new LoanDto()
            {
                Car = rentalCar.Car,
                LoanUser = await _userRepository.GetUser(User.Identity.Name),
                LoanDate = (DateTime)Session["LoanDate"],
                ReturnDate = (DateTime)Session["ReturnDate"],
                CarReturnedDate = null,
                Returned = false,
                Caution = rentalCar.Caution,
                Price = rentalCar.Price,
                Rented = false
            };
            return View(loanDto);
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> RentCar(LoanDto DtoLoan)
        {
            LoanDto loan = await _repository.Rent(DtoLoan);
            if (loan != null)
                return View("Rented", loan);
            else
                return View("FailRent", DtoLoan.Car);
        }
        
        [HttpPost]
        public async Task<ActionResult> CancelRent(int id)
        {
            Loan loan = await _repository.Cancel(id);
            if (loan != null)
                TempData["message"] = "Rezerwacja została anulowanna";
            return Json(new { redirectToUrl = Url.Action("RentedCars", "Account") });
        }

        public async Task<FileContentResult> GetImage(int id)
        {
            var car = await _repository.GetCarById(id);
            if (car != null && car.ImageData != null)
                return File(car.ImageData, car.ImageMimeType);
            else
                return null;
        }
    }
}