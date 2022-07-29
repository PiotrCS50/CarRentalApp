namespace CarRental.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CarModified : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Loans", "LicensePlate", c => c.String());
            DropColumn("dbo.Cars", "LicencePlate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cars", "LicencePlate", c => c.String());
            DropColumn("dbo.Loans", "LicensePlate");
        }
    }
}
