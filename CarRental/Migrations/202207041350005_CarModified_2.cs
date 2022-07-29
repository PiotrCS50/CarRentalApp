namespace CarRental.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CarModified_2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LicesnePlates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CarId = c.Int(nullable: false),
                        LicenseNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LicesnePlates");
        }
    }
}
