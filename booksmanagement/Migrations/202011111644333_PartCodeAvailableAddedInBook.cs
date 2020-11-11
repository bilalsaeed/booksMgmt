namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PartCodeAvailableAddedInBook : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "PartCodeAvailable", c => c.Boolean(nullable: false));
            AddColumn("dbo.Books", "SoftCopyAvailable", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "SoftCopyAvailable");
            DropColumn("dbo.Books", "PartCodeAvailable");
        }
    }
}
