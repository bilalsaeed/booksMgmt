namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DrawingFileRefAddedInCarPartComponents : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CarPartComponents", "DrawingFilesId", c => c.Int());
            CreateIndex("dbo.CarPartComponents", "DrawingFilesId");
            AddForeignKey("dbo.CarPartComponents", "DrawingFilesId", "dbo.DrawingFiles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CarPartComponents", "DrawingFilesId", "dbo.DrawingFiles");
            DropIndex("dbo.CarPartComponents", new[] { "DrawingFilesId" });
            DropColumn("dbo.CarPartComponents", "DrawingFilesId");
        }
    }
}
