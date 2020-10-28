namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserRolesUpdate : DbMigration
    {
        public override void Up()
        {
            Sql(string.Format(@"
                  INSERT INTO AspNetRoles (Id,Name) VALUES ('{0}','Drawer');
                  UPDATE AspNetRoles SET Name='Engineering' WHERE Name='User';", Guid.NewGuid())
               );
        }
        
        public override void Down()
        {
        }
    }
}
