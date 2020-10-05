namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CarsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cars",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CarBrandId = c.Int(nullable: false),
                        Class = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CarBrands", t => t.CarBrandId, cascadeDelete: true)
                .Index(t => t.CarBrandId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cars", "CarBrandId", "dbo.CarBrands");
            DropIndex("dbo.Cars", new[] { "CarBrandId" });
            DropTable("dbo.Cars");
        }
    }
}
