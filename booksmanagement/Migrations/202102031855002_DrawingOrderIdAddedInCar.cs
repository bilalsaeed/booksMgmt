namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DrawingOrderIdAddedInCar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cars", "DrawingOrderId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cars", "DrawingOrderId");
        }
    }
}
