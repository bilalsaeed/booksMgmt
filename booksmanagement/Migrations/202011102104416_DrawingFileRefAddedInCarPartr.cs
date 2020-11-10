namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DrawingFileRefAddedInCarPartr : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CarParts", "DrawingFilesId", c => c.Int());
            CreateIndex("dbo.CarParts", "DrawingFilesId");
            AddForeignKey("dbo.CarParts", "DrawingFilesId", "dbo.DrawingFiles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CarParts", "DrawingFilesId", "dbo.DrawingFiles");
            DropIndex("dbo.CarParts", new[] { "DrawingFilesId" });
            DropColumn("dbo.CarParts", "DrawingFilesId");
        }
    }
}
