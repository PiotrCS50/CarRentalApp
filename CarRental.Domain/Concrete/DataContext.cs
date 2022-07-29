using CarRental.Domain.Entities;
using System;
using System.Data.Entity;
using System.Linq;

namespace CarRental.App_Start
{
    public class DataContext : DbContext
    {
        public DataContext()
            : base("name=DataContext")
        {
        }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<RentalRecord> Rental { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<LicesnePlate> LicensePlates { get; set; }
    }
}