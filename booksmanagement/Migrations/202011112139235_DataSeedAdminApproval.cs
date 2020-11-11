namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataSeedAdminApproval : DbMigration
    {
        public override void Up()
        {
            Sql("update AspNetUsers set IsActive=1, IsApproved=1;");
        }
        
        public override void Down()
        {
        }
    }
}
