using CarRental.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.DTOs
{
    public class RentalRecordDto
    {
        public int Id { get; set; }
        public Car Car { get; set; }
        public decimal Price { get; set; }
        public decimal Caution { get; set; }
        public string Description { get; set; }
        public int NumberOfLoans { get; set; }
    }
}
