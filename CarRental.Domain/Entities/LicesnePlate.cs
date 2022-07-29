using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Entities
{
    public class LicesnePlate
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public string LicenseNumber { get; set; }
    }
}
