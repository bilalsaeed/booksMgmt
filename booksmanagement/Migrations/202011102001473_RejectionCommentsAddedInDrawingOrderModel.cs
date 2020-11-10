namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RejectionCommentsAddedInDrawingOrderModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DrawingOrders", "RejectionComments", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DrawingOrders", "RejectionComments");
        }
    }
}
