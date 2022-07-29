namespace CarRental.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PasswordEdited : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "PasswordHash", c => c.Binary());
            AddColumn("dbo.Users", "PasswordSalt", c => c.Binary());
            DropColumn("dbo.Users", "Password");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Password", c => c.String());
            DropColumn("dbo.Users", "PasswordSalt");
            DropColumn("dbo.Users", "PasswordHash");
        }
    }
}
