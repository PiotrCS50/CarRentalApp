using CarRental.Domain.Abstract;
using CarRental.Domain.Entities;
using CarRental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CarRental.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IRentalRepository _rentalRepository;
        private int pageSize = 5;
        public AccountController(IUserRepository userRepository, IRentalRepository rentalRepository)
        {
            _userRepository = userRepository;
            _rentalRepository = rentalRepository;
        }

        // GET: Account
        public ActionResult Login(string returnUrl = null)
        {
            if (User.Identity.Name == "" || (returnUrl != null && returnUrl.Contains("Admin")))
            {
                ViewBag.returnUrl = returnUrl;
                return View();
            }
            return Redirect(Url.Action("Index", "Home"));
        }
        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel loginModel, string returnUrl = null) 
        {
            if (ModelState.IsValid)
            {
                bool validate = await _userRepository.CheckUser(loginModel.Username, loginModel.Password);
                if (validate)
                {
                    FormsAuthentication.SetAuthCookie(loginModel.Username, false);
                    if(string.IsNullOrEmpty(returnUrl) || string.IsNullOrWhiteSpace(returnUrl))
                        return RedirectToAction("Index", "Home");
                    else
                        return Redirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("Authy", "Nieprawidłowy login lub hasło");
                    return View();
                }
            }
            else
                return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Register()
        {
            if (User.Identity.Name != "")
                return Redirect(Url.Action("Index", "Home"));

            var user = new RegisterViewModel { AccountType = Role.Użytkownik };
            user.Admin = false;
            return View(user);
        }
        [HttpPost]
        public async Task<ActionResult> Register(RegisterViewModel registerModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var hmac = new HMACSHA512();
                if (await _userRepository.AddUser(new User
                {
                    UserName = registerModel.UserName,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerModel.Password)),
                    PasswordSalt = hmac.Key,
                    Name = registerModel.Name,
                    Surname = registerModel.Surname,
                    AccountType = Role.Użytkownik,
                }))
                {
                    FormsAuthentication.SetAuthCookie(registerModel.UserName, false);
                    if (string.IsNullOrEmpty(returnUrl) || string.IsNullOrWhiteSpace(returnUrl))
                        return RedirectToAction("Index", "Home");
                    else
                        return Redirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("UserNameUsed", "Ten login jest już zajęty");
                    return View(registerModel);
                }
            }
            else
                return View(registerModel);
        }

        public async Task<ActionResult> RentedCars(bool past = false, int page = 1)
        {
            ViewBag.Past = past;
            Paging paging = new Paging
            {
                ItemsPerPage = pageSize,
                CurrentPage = page
            };
            var user =  await _userRepository.GetUser(User.Identity.Name);
            var cars =  await _userRepository.GetRentedCars(user, past, paging);
            return View(cars);
        }

        public async Task<ActionResult> AccountData()
        {
            var user = await _userRepository.GetUser(User.Identity.Name);
            var userData = new RegisterViewModel
            {
                Name = user.Name,
                Surname = user.Surname,
                AccountType = user.AccountType,
                Admin = user.AccountType == Role.Administrator ? true : false,
            };
            return View(userData);
        }
        [HttpPost]
        public async Task<ActionResult> AccountData(RegisterViewModel dataModel)
        {
            var user = await _userRepository.GetUser(User.Identity.Name);
            user.Name = dataModel.Name;
            user.Surname = dataModel.Surname;
            user.AccountType = dataModel.AccountType;
            if (await _userRepository.AddUser(user))
            {
                TempData["message"] = "Zmiany zostały zapisane";
            }
            return View(dataModel);
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ChangePassword(PasswordViewModel passwordModel)
        {
            var user = await _userRepository.GetUser(User.Identity.Name);
            if(await _userRepository.ChangePassword(user, passwordModel.OldPassoword, passwordModel.NewPassoword))
            {
                TempData["message"] = "Hasło zostało zmienione";
                return RedirectToAction("AccountData");
            }
            else
            {
                ModelState.AddModelError("BadPassword", "Stare hasło jest niepoprawne");
                return View();
            }
        }
    }
}