namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CarPartComponentAndCarPartComponentDescAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CarPartComponentDescs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CarPartComponentId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CarPartComponents", t => t.CarPartComponentId, cascadeDelete: true)
                .Index(t => t.CarPartComponentId);
            
            CreateTable(
                "dbo.CarPartComponents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CarPartId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CarParts", t => t.CarPartId, cascadeDelete: true)
                .Index(t => t.CarPartId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CarPartComponentDescs", "CarPartComponentId", "dbo.CarPartComponents");
            DropForeignKey("dbo.CarPartComponents", "CarPartId", "dbo.CarParts");
            DropIndex("dbo.CarPartComponents", new[] { "CarPartId" });
            DropIndex("dbo.CarPartComponentDescs", new[] { "CarPartComponentId" });
            DropTable("dbo.CarPartComponents");
            DropTable("dbo.CarPartComponentDescs");
        }
    }
}
