namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BookMediaFilesAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookMediaFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FileId = c.String(),
                        Type = c.String(),
                        ContentType = c.String(),
                        Size = c.Int(nullable: false),
                        BookId = c.Int(nullable: false),
                        CreateAt = c.DateTime(nullable: false),
                        CreatedAtTicks = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .Index(t => t.BookId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BookMediaFiles", "BookId", "dbo.Books");
            DropIndex("dbo.BookMediaFiles", new[] { "BookId" });
            DropTable("dbo.BookMediaFiles");
        }
    }
}
