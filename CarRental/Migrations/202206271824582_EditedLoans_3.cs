namespace CarRental.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditedLoans_3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Loans", "Returned", c => c.Boolean(nullable: false));
            DropColumn("dbo.Loans", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Loans", "Description", c => c.String());
            DropColumn("dbo.Loans", "Returned");
        }
    }
}
