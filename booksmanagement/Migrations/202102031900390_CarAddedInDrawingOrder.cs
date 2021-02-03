namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CarAddedInDrawingOrder : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DrawingOrders", "CarPartId", "dbo.CarParts");
            DropIndex("dbo.DrawingOrders", new[] { "CarPartId" });
            AddColumn("dbo.DrawingOrders", "CarId", c => c.Int());
            AlterColumn("dbo.DrawingOrders", "CarPartId", c => c.Int());
            CreateIndex("dbo.DrawingOrders", "CarId");
            CreateIndex("dbo.DrawingOrders", "CarPartId");
            AddForeignKey("dbo.DrawingOrders", "CarId", "dbo.Cars", "Id");
            AddForeignKey("dbo.DrawingOrders", "CarPartId", "dbo.CarParts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DrawingOrders", "CarPartId", "dbo.CarParts");
            DropForeignKey("dbo.DrawingOrders", "CarId", "dbo.Cars");
            DropIndex("dbo.DrawingOrders", new[] { "CarPartId" });
            DropIndex("dbo.DrawingOrders", new[] { "CarId" });
            AlterColumn("dbo.DrawingOrders", "CarPartId", c => c.Int(nullable: false));
            DropColumn("dbo.DrawingOrders", "CarId");
            CreateIndex("dbo.DrawingOrders", "CarPartId");
            AddForeignKey("dbo.DrawingOrders", "CarPartId", "dbo.CarParts", "Id", cascadeDelete: true);
        }
    }
}
