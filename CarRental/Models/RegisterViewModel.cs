using CarRental.Domain.Entities;
using CarRental.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRental.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Login jest wymagany")]
        [Display(Name="Login")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Imię jest wymagane")]
        [OnlyLetters(ErrorMessage = "Imię jest niepoprawne")]
        [Display(Name = "Imię")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        [OnlyLetters(ErrorMessage = "Nazwisko jest niepoprawne")]
        [Display(Name = "Nazwisko")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Hasło jest wymagane")]
        [Display(Name = "Hasło")]
        public string Password { get; set; }
        [UIHint("Enum")]
        public Role AccountType { get; set; }
        public bool Admin { get; set; }
    }
}