using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Entities
{
    [Table("Rental")]
    public class RentalRecord
    {
        public int Id { get; set; }
        public Car Car { get; set; }
        public decimal Price { get; set; }
        public decimal Caution { get; set; }
        public string Description { get; set; }
    }
}
