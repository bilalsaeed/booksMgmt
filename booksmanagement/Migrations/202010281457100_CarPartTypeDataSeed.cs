namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CarPartTypeDataSeed : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                INSERT INTO CarPartTypes VALUES('Mechanical');
                INSERT INTO CarPartTypes VALUES('Electrical');
                INSERT INTO CarPartTypes VALUES('Car body');
                ");
        }
        
        public override void Down()
        {
        }
    }
}
