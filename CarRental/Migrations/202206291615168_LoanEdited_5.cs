namespace CarRental.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LoanEdited_5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Loans", "CarReturnedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Loans", "CarReturnedDate");
        }
    }
}
