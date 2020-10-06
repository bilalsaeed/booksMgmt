namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CarAddedInBook : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "CarId", c => c.Int(nullable: false));
            CreateIndex("dbo.Books", "CarId");
            AddForeignKey("dbo.Books", "CarId", "dbo.Cars", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Books", "CarId", "dbo.Cars");
            DropIndex("dbo.Books", new[] { "CarId" });
            DropColumn("dbo.Books", "CarId");
        }
    }
}
