using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRental.Models
{
    public class PasswordViewModel
    {
        [Display(Name ="Stare hasło")]
        [Required(ErrorMessage = "Stare hasło jest wymagane")]
        public string OldPassoword { get; set; }
        [Display(Name = "Nowe hasło")]
        [Required(ErrorMessage = "Nowe hasło jest wymagane")]
        public string NewPassoword { get; set; }
    }
}