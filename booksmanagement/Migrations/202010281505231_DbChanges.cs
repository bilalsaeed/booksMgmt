namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CarParts", "CarPartTypeId", c => c.Int(nullable: true));
            CreateIndex("dbo.CarParts", "CarPartTypeId");
            AddForeignKey("dbo.CarParts", "CarPartTypeId", "dbo.CarPartTypes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CarParts", "CarPartTypeId", "dbo.CarPartTypes");
            DropIndex("dbo.CarParts", new[] { "CarPartTypeId" });
            DropColumn("dbo.CarParts", "CarPartTypeId");
        }
    }
}
