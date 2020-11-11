namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsArchivedAddedInBook : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "IsArchived", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "IsArchived");
        }
    }
}
