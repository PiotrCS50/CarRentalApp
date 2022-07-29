namespace CarRental.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LoanEdited : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Loans", "Rented", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Loans", "Rented");
        }
    }
}
