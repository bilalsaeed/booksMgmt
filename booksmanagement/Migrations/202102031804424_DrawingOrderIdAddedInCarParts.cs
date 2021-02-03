namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DrawingOrderIdAddedInCarParts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CarParts", "DrawingOrderId", c => c.Int());
            AddColumn("dbo.CarPartComponents", "DrawingOrderId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CarPartComponents", "DrawingOrderId");
            DropColumn("dbo.CarParts", "DrawingOrderId");
        }
    }
}
