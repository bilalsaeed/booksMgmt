namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UniqueConstrantInBookModel : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Books", new[] { "CarId" });
            DropIndex("dbo.Books", new[] { "CarPartId" });
            DropIndex("dbo.Books", new[] { "CarPartComponentId" });
            DropIndex("dbo.Books", new[] { "CarPartComponentDescId" });
            CreateIndex("dbo.Books", new[] { "CarId", "CarPartId", "CarPartComponentId", "CarPartComponentDescId" }, unique: true, name: "IX_Car_CarPart_CarPartComp_CarPartCompDesc");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Books", "IX_Car_CarPart_CarPartComp_CarPartCompDesc");
            CreateIndex("dbo.Books", "CarPartComponentDescId");
            CreateIndex("dbo.Books", "CarPartComponentId");
            CreateIndex("dbo.Books", "CarPartId");
            CreateIndex("dbo.Books", "CarId");
        }
    }
}
