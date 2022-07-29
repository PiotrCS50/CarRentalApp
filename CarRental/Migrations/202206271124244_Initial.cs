namespace CarRental.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Loans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Caution = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(),
                        Car_Id = c.Int(),
                        LoanUser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cars", t => t.Car_Id)
                .ForeignKey("dbo.Users", t => t.LoanUser_Id)
                .Index(t => t.Car_Id)
                .Index(t => t.LoanUser_Id);
            
            CreateTable(
                "dbo.Cars",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Brand = c.String(),
                        Model = c.String(),
                        LicencePlate = c.String(),
                        Capacity = c.Int(nullable: false),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Name = c.String(),
                        Surname = c.String(),
                        Password = c.String(),
                        MyProperty = c.Int(nullable: false),
                        AccountType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Rental",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Caution = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(),
                        Car_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cars", t => t.Car_Id)
                .Index(t => t.Car_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rental", "Car_Id", "dbo.Cars");
            DropForeignKey("dbo.Loans", "LoanUser_Id", "dbo.Users");
            DropForeignKey("dbo.Loans", "Car_Id", "dbo.Cars");
            DropIndex("dbo.Rental", new[] { "Car_Id" });
            DropIndex("dbo.Loans", new[] { "LoanUser_Id" });
            DropIndex("dbo.Loans", new[] { "Car_Id" });
            DropTable("dbo.Rental");
            DropTable("dbo.Users");
            DropTable("dbo.Cars");
            DropTable("dbo.Loans");
        }
    }
}
