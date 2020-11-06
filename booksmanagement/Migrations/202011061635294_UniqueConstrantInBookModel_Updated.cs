namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UniqueConstrantInBookModel_Updated : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Books", "IX_Car_CarPart_CarPartComp_CarPartCompDesc");
            CreateIndex("dbo.Books", new[] { "CarId", "CarPartId", "CarPartComponentId", "CarPartComponentDescId", "TypeId" }, unique: true, name: "IX_Car_CarPart_CarPartComp_CarPartCompDesc");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Books", "IX_Car_CarPart_CarPartComp_CarPartCompDesc");
            CreateIndex("dbo.Books", new[] { "CarId", "CarPartId", "CarPartComponentId", "CarPartComponentDescId" }, unique: true, name: "IX_Car_CarPart_CarPartComp_CarPartCompDesc");
        }
    }
}
