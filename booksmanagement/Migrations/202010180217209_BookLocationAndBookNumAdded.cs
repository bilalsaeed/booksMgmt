namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BookLocationAndBookNumAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "BookLocation", c => c.String());
            AddColumn("dbo.Books", "BookNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "BookNumber");
            DropColumn("dbo.Books", "BookLocation");
        }
    }
}
