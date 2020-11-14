namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobNumberAddedInDrawingOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DrawingOrders", "JobNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DrawingOrders", "JobNumber");
        }
    }
}
