namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FieldsAddedInDrawingOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DrawingOrders", "DrawingSubmitted", c => c.Boolean(nullable: false));
            AddColumn("dbo.DrawingOrders", "DrawingSubmittedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DrawingOrders", "DrawingSubmittedDate");
            DropColumn("dbo.DrawingOrders", "DrawingSubmitted");
        }
    }
}
