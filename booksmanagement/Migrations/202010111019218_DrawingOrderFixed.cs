namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DrawingOrderFixed : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.DrawingOrders", new[] { "Applicant_Id" });
            DropIndex("dbo.DrawingOrders", new[] { "AssignedTo_Id" });
            DropColumn("dbo.DrawingOrders", "ApplicantId");
            DropColumn("dbo.DrawingOrders", "AssignedToId");
            RenameColumn(table: "dbo.DrawingOrders", name: "Applicant_Id", newName: "ApplicantId");
            RenameColumn(table: "dbo.DrawingOrders", name: "AssignedTo_Id", newName: "AssignedToId");
            AlterColumn("dbo.DrawingOrders", "ApplicantId", c => c.String(maxLength: 128));
            AlterColumn("dbo.DrawingOrders", "AssignedToId", c => c.String(maxLength: 128));
            CreateIndex("dbo.DrawingOrders", "ApplicantId");
            CreateIndex("dbo.DrawingOrders", "AssignedToId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.DrawingOrders", new[] { "AssignedToId" });
            DropIndex("dbo.DrawingOrders", new[] { "ApplicantId" });
            AlterColumn("dbo.DrawingOrders", "AssignedToId", c => c.Int());
            AlterColumn("dbo.DrawingOrders", "ApplicantId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.DrawingOrders", name: "AssignedToId", newName: "AssignedTo_Id");
            RenameColumn(table: "dbo.DrawingOrders", name: "ApplicantId", newName: "Applicant_Id");
            AddColumn("dbo.DrawingOrders", "AssignedToId", c => c.Int());
            AddColumn("dbo.DrawingOrders", "ApplicantId", c => c.Int(nullable: false));
            CreateIndex("dbo.DrawingOrders", "AssignedTo_Id");
            CreateIndex("dbo.DrawingOrders", "Applicant_Id");
        }
    }
}
