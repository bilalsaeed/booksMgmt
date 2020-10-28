namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DatSeed : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                UPDATE CarParts SET CarPartTypeId=1 WHERE CarPartTypeId IS NULL;
");
        }
        
        public override void Down()
        {
        }
    }
}
