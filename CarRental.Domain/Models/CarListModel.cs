using CarRental.Domain.DTOs;
using CarRental.Domain.Entities;
using CarRental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Models
{
    public class CarListModel
    {
        public IEnumerable<RentalRecordDto> Cars { get; set; }
        public Paging Paging { get; set; }
        public FilterModel Filter { get; set; }
    }
}
