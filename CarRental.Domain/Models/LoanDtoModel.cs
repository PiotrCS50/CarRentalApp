using CarRental.DTOs;
using CarRental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Models
{
    public class LoanDtoModel
    {
        public IEnumerable<LoanDto> Loans { get; set; }
        public Paging Paging { get; set; }
    }
}
