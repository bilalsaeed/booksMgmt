namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuantityMadeNullableInBook : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Books", "Quantity", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Books", "Quantity", c => c.Int(nullable: false));
        }
    }
}
