namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CarGeneralMediaFileAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cars", "MaintenancePlanId", c => c.Int());
            CreateIndex("dbo.Cars", "MaintenancePlanId");
            AddForeignKey("dbo.Cars", "MaintenancePlanId", "dbo.GeneralMediaFiles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cars", "MaintenancePlanId", "dbo.GeneralMediaFiles");
            DropIndex("dbo.Cars", new[] { "MaintenancePlanId" });
            DropColumn("dbo.Cars", "MaintenancePlanId");
        }
    }
}
