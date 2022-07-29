using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Entities
{
    [Table("Loans")]
    public class Loan
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public string LicensePlate { get; set; }
        public decimal Price { get; set; }
        public decimal Caution { get; set; }
        public int UserId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public DateTime? CarReturnedDate { get; set; }
        public bool Returned { get; set; } = true;
        public bool Rented { get; set; } = true;
        
    }
}
