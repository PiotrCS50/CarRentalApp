namespace CarRental.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCarPhoto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cars", "ImageData", c => c.Binary());
            AddColumn("dbo.Cars", "ImageMimeType", c => c.String());
            CreateIndex("dbo.LicesnePlates", "CarId");
            AddForeignKey("dbo.LicesnePlates", "CarId", "dbo.Cars", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LicesnePlates", "CarId", "dbo.Cars");
            DropIndex("dbo.LicesnePlates", new[] { "CarId" });
            DropColumn("dbo.Cars", "ImageMimeType");
            DropColumn("dbo.Cars", "ImageData");
        }
    }
}
