using CarRental.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRental.Models
{
    public class CarsListViewModel
    {
        public IEnumerable<RentalRecord> Cars { get; set; }
        public Paging Paging { get; set; }
    }
}