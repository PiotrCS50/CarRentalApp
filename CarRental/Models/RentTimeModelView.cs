using CarRental.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRental.Models
{
    public class RentTimeModelView
    {
        [Required]
        [Display(Name = "Data odbioru")]
        [CorrectDate(ErrorMessage = "Proszę podać poprawną datę")]
        [DataType(DataType.Date)]
        public DateTime LoanDate { get; set; }
        [Required]
        [Display(Name ="Data zwrotu")]
        [CorrectDate(ErrorMessage = "Proszę podać poprawną datę")]
        [DataType(DataType.Date)]
        public DateTime ReturnDate { get; set; }
    }
}