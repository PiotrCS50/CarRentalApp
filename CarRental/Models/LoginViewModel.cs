using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRental.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Login jest wymagany")]
        [Display(Name = "Login")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Hasło jest wymagane")]
        [Display(Name = "Hasło")]
        public string Password { get; set; }
    }
}