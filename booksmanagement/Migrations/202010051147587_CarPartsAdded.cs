namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CarPartsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CarParts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CarId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cars", t => t.CarId, cascadeDelete: true)
                .Index(t => t.CarId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CarParts", "CarId", "dbo.Cars");
            DropIndex("dbo.CarParts", new[] { "CarId" });
            DropTable("dbo.CarParts");
        }
    }
}
