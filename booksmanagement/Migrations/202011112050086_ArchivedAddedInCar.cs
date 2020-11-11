namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArchivedAddedInCar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cars", "IsArchived", c => c.Boolean(nullable: false));
            DropColumn("dbo.Books", "IsArchived");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Books", "IsArchived", c => c.Boolean(nullable: false));
            DropColumn("dbo.Cars", "IsArchived");
        }
    }
}
