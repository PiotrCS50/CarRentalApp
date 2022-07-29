using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRental.Models
{
    public class AddLicenseViweModel
    {
        public int CarId { get; set; }
        [Required(ErrorMessage = "Numer rejestracyjny jest wymagany")]
        [RegularExpression("[A-Z]{2,3} [A-Z0-9]{2,5}", ErrorMessage = "Numer rejestracyjny jest niepoprawny")]
        [Display(Name = "Numer rejestracyjny")]
        public string LicesneNumber { get; set; }
    }
}