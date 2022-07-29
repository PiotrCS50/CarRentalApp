namespace CarRental.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial_2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "MyProperty");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "MyProperty", c => c.Int(nullable: false));
        }
    }
}
