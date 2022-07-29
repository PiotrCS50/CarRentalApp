using CarRental.Domain.Entities;
using CarRental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRental.DTOs
{
    public class LoanDto
    {
        public int LoanId { get; set; }
        public Car Car { get; set; }
        public string LicensePlate { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get { return Price * LoanDays; } }
        public decimal Caution { get; set; }
        public decimal Surcharge
        {
            get
            {
                int days = (DateTime.Today - ReturnDate).Days;
                if (days > 0)
                    return days * Price;
                return 0;
            }
        }
        public User LoanUser { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public DateTime? CarReturnedDate { get; set; }
        public int LoanDays { get { return (ReturnDate - LoanDate).Days + 1; } }
        public bool Returned { get; set; } = true;
        public bool Rented { get; set; } = true;
    }
}