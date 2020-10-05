namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PropsAddedInApplicationUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ServiceNumber", c => c.String());
            AddColumn("dbo.AspNetUsers", "Department", c => c.String());
            AddColumn("dbo.AspNetUsers", "CreatedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "CreatedDate");
            DropColumn("dbo.AspNetUsers", "Department");
            DropColumn("dbo.AspNetUsers", "ServiceNumber");
        }
    }
}
