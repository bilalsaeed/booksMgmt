namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GeneralMediaFilesAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GeneralMediaFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        File = c.Binary(),
                        FileName = c.String(),
                        Type = c.String(),
                        SessionId = c.String(),
                        FileType = c.String(),
                        FileSize = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GeneralMediaFiles");
        }
    }
}
