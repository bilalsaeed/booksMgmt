namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CarPartComponentAddedInDrawingRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DrawingOrders", "CarPartComponentId", c => c.Int());
            CreateIndex("dbo.DrawingOrders", "CarPartComponentId");
            AddForeignKey("dbo.DrawingOrders", "CarPartComponentId", "dbo.CarPartComponents", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DrawingOrders", "CarPartComponentId", "dbo.CarPartComponents");
            DropIndex("dbo.DrawingOrders", new[] { "CarPartComponentId" });
            DropColumn("dbo.DrawingOrders", "CarPartComponentId");
        }
    }
}
