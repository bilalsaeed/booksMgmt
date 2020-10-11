namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DrawingRejectedAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DrawingOrders", "DrawingRejected", c => c.Boolean(nullable: false));
            AddColumn("dbo.DrawingOrders", "DrawingRejectedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DrawingOrders", "DrawingRejectedDate");
            DropColumn("dbo.DrawingOrders", "DrawingRejected");
        }
    }
}
