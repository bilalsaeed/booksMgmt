namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataSeedPartCodeAvailale : DbMigration
    {
        public override void Up()
        {
            Sql(@"

                    update Books set PartCodeAvailable=1 where Id in (select BookId from BookMediaFiles where Type='P');
                    update Books set SoftCopyAvailable=1 where Id in (select BookId from BookMediaFiles where Type='S');
                ");
        }
        
        public override void Down()
        {
        }
    }
}
