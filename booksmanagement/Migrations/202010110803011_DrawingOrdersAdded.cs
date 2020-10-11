namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DrawingOrdersAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DrawingOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Purpose = c.String(),
                        Location = c.String(),
                        CarPartId = c.Int(nullable: false),
                        ApplicantId = c.Int(nullable: false),
                        AppliedDate = c.DateTime(nullable: false),
                        AssignedToId = c.Int(),
                        AssignedDate = c.DateTime(),
                        IsApproved = c.Boolean(nullable: false),
                        ApprovalDate = c.DateTime(),
                        Applicant_Id = c.String(maxLength: 128),
                        AssignedTo_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Applicant_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AssignedTo_Id)
                .ForeignKey("dbo.CarParts", t => t.CarPartId, cascadeDelete: true)
                .Index(t => t.CarPartId)
                .Index(t => t.Applicant_Id)
                .Index(t => t.AssignedTo_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DrawingOrders", "CarPartId", "dbo.CarParts");
            DropForeignKey("dbo.DrawingOrders", "AssignedTo_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.DrawingOrders", "Applicant_Id", "dbo.AspNetUsers");
            DropIndex("dbo.DrawingOrders", new[] { "AssignedTo_Id" });
            DropIndex("dbo.DrawingOrders", new[] { "Applicant_Id" });
            DropIndex("dbo.DrawingOrders", new[] { "CarPartId" });
            DropTable("dbo.DrawingOrders");
        }
    }
}
