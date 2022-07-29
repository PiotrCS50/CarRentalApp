namespace CarRental.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditedLoans : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Loans", "TotalPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Loans", "LoanDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Loans", "ReturnDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Loans", "LoanDays", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Loans", "LoanDays");
            DropColumn("dbo.Loans", "ReturnDate");
            DropColumn("dbo.Loans", "LoanDate");
            DropColumn("dbo.Loans", "TotalPrice");
        }
    }
}
