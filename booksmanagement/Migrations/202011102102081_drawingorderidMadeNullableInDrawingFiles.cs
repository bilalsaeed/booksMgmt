namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class drawingorderidMadeNullableInDrawingFiles : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DrawingFiles", "DrawingOrderId", "dbo.DrawingOrders");
            DropIndex("dbo.DrawingFiles", new[] { "DrawingOrderId" });
            AlterColumn("dbo.DrawingFiles", "DrawingOrderId", c => c.Int());
            CreateIndex("dbo.DrawingFiles", "DrawingOrderId");
            AddForeignKey("dbo.DrawingFiles", "DrawingOrderId", "dbo.DrawingOrders", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DrawingFiles", "DrawingOrderId", "dbo.DrawingOrders");
            DropIndex("dbo.DrawingFiles", new[] { "DrawingOrderId" });
            AlterColumn("dbo.DrawingFiles", "DrawingOrderId", c => c.Int(nullable: false));
            CreateIndex("dbo.DrawingFiles", "DrawingOrderId");
            AddForeignKey("dbo.DrawingFiles", "DrawingOrderId", "dbo.DrawingOrders", "Id", cascadeDelete: true);
        }
    }
}
