namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedArcheterureOfDrawingFiles : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CarPartComponents", "DrawingFilesId", "dbo.DrawingFiles");
            DropForeignKey("dbo.CarParts", "DrawingFilesId", "dbo.DrawingFiles");
            DropIndex("dbo.CarParts", new[] { "DrawingFilesId" });
            DropIndex("dbo.CarPartComponents", new[] { "DrawingFilesId" });
            AddColumn("dbo.Cars", "IsDrawingAvailable", c => c.Boolean(nullable: false));
            AddColumn("dbo.CarParts", "IsDrawingAvailable", c => c.Boolean(nullable: false));
            AddColumn("dbo.DrawingFiles", "CarId", c => c.Int());
            AddColumn("dbo.DrawingFiles", "CarPartId", c => c.Int());
            AddColumn("dbo.DrawingFiles", "CarPartComponentId", c => c.Int());
            AddColumn("dbo.CarPartComponents", "IsDrawingAvailable", c => c.Boolean(nullable: false));
            CreateIndex("dbo.DrawingFiles", "CarId");
            CreateIndex("dbo.DrawingFiles", "CarPartId");
            CreateIndex("dbo.DrawingFiles", "CarPartComponentId");
            AddForeignKey("dbo.DrawingFiles", "CarId", "dbo.Cars", "Id");
            AddForeignKey("dbo.DrawingFiles", "CarPartId", "dbo.CarParts", "Id");
            AddForeignKey("dbo.DrawingFiles", "CarPartComponentId", "dbo.CarPartComponents", "Id");
            DropColumn("dbo.CarParts", "DrawingFilesId");
            DropColumn("dbo.CarPartComponents", "DrawingFilesId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CarPartComponents", "DrawingFilesId", c => c.Int());
            AddColumn("dbo.CarParts", "DrawingFilesId", c => c.Int());
            DropForeignKey("dbo.DrawingFiles", "CarPartComponentId", "dbo.CarPartComponents");
            DropForeignKey("dbo.DrawingFiles", "CarPartId", "dbo.CarParts");
            DropForeignKey("dbo.DrawingFiles", "CarId", "dbo.Cars");
            DropIndex("dbo.DrawingFiles", new[] { "CarPartComponentId" });
            DropIndex("dbo.DrawingFiles", new[] { "CarPartId" });
            DropIndex("dbo.DrawingFiles", new[] { "CarId" });
            DropColumn("dbo.CarPartComponents", "IsDrawingAvailable");
            DropColumn("dbo.DrawingFiles", "CarPartComponentId");
            DropColumn("dbo.DrawingFiles", "CarPartId");
            DropColumn("dbo.DrawingFiles", "CarId");
            DropColumn("dbo.CarParts", "IsDrawingAvailable");
            DropColumn("dbo.Cars", "IsDrawingAvailable");
            CreateIndex("dbo.CarPartComponents", "DrawingFilesId");
            CreateIndex("dbo.CarParts", "DrawingFilesId");
            AddForeignKey("dbo.CarParts", "DrawingFilesId", "dbo.DrawingFiles", "Id");
            AddForeignKey("dbo.CarPartComponents", "DrawingFilesId", "dbo.DrawingFiles", "Id");
        }
    }
}
