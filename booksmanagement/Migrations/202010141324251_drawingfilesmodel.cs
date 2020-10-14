namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class drawingfilesmodel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DrawingFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FileId = c.String(),
                        Type = c.String(),
                        ContentType = c.String(),
                        Size = c.Int(nullable: false),
                        DrawingOrderId = c.Int(nullable: false),
                        CreateAt = c.DateTime(nullable: false),
                        CreatedAtTicks = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DrawingOrders", t => t.DrawingOrderId, cascadeDelete: true)
                .Index(t => t.DrawingOrderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DrawingFiles", "DrawingOrderId", "dbo.DrawingOrders");
            DropIndex("dbo.DrawingFiles", new[] { "DrawingOrderId" });
            DropTable("dbo.DrawingFiles");
        }
    }
}
