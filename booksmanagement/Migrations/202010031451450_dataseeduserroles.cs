namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class dataseeduserroles : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES ('64c3fc4a-ec04-4f8f-913d-d7b4c3ba1969', 'da4509f7-bfa1-403d-89a2-44ae3aecce37')");
        }

        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "IsActive");
            DropColumn("dbo.AspNetUsers", "ContactNumber");
        }
    }
}
