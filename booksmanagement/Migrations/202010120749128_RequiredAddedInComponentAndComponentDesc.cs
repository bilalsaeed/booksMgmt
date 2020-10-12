namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequiredAddedInComponentAndComponentDesc : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CarPartComponentDescs", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.CarPartComponents", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CarPartComponents", "Name", c => c.String());
            AlterColumn("dbo.CarPartComponentDescs", "Name", c => c.String());
        }
    }
}
