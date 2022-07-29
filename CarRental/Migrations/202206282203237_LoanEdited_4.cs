namespace CarRental.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LoanEdited_4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Loans", "Car_Id", "dbo.Cars");
            DropForeignKey("dbo.Loans", "LoanUser_Id", "dbo.Users");
            DropIndex("dbo.Loans", new[] { "Car_Id" });
            DropIndex("dbo.Loans", new[] { "LoanUser_Id" });
            AddColumn("dbo.Loans", "CarId", c => c.Int(nullable: false));
            AddColumn("dbo.Loans", "UserId", c => c.Int(nullable: false));
            DropColumn("dbo.Loans", "TotalPrice");
            DropColumn("dbo.Loans", "LoanDays");
            DropColumn("dbo.Loans", "Car_Id");
            DropColumn("dbo.Loans", "LoanUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Loans", "LoanUser_Id", c => c.Int());
            AddColumn("dbo.Loans", "Car_Id", c => c.Int());
            AddColumn("dbo.Loans", "LoanDays", c => c.Int(nullable: false));
            AddColumn("dbo.Loans", "TotalPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Loans", "UserId");
            DropColumn("dbo.Loans", "CarId");
            CreateIndex("dbo.Loans", "LoanUser_Id");
            CreateIndex("dbo.Loans", "Car_Id");
            AddForeignKey("dbo.Loans", "LoanUser_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.Loans", "Car_Id", "dbo.Cars", "Id");
        }
    }
}
