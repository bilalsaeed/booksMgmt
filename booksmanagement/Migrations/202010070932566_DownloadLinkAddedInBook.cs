namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DownloadLinkAddedInBook : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "DownloadLink", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "DownloadLink");
        }
    }
}
