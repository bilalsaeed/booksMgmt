namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FieldsAddedInBook : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "CarPartId", c => c.Int());
            AddColumn("dbo.Books", "CarPartComponentId", c => c.Int());
            AddColumn("dbo.Books", "CarPartComponentDescId", c => c.Int());
            CreateIndex("dbo.Books", "CarPartId");
            CreateIndex("dbo.Books", "CarPartComponentId");
            CreateIndex("dbo.Books", "CarPartComponentDescId");
            AddForeignKey("dbo.Books", "CarPartId", "dbo.CarParts", "Id");
            AddForeignKey("dbo.Books", "CarPartComponentId", "dbo.CarPartComponents", "Id");
            AddForeignKey("dbo.Books", "CarPartComponentDescId", "dbo.CarPartComponentDescs", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Books", "CarPartComponentDescId", "dbo.CarPartComponentDescs");
            DropForeignKey("dbo.Books", "CarPartComponentId", "dbo.CarPartComponents");
            DropForeignKey("dbo.Books", "CarPartId", "dbo.CarParts");
            DropIndex("dbo.Books", new[] { "CarPartComponentDescId" });
            DropIndex("dbo.Books", new[] { "CarPartComponentId" });
            DropIndex("dbo.Books", new[] { "CarPartId" });
            DropColumn("dbo.Books", "CarPartComponentDescId");
            DropColumn("dbo.Books", "CarPartComponentId");
            DropColumn("dbo.Books", "CarPartId");
        }
    }
}
