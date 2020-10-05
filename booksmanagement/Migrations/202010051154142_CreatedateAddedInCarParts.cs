namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatedateAddedInCarParts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CarParts", "CreatedAt", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CarParts", "CreatedAt");
        }
    }
}
