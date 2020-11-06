namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BookMediaFileChanged : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BookMediaFiles", "File", c => c.Binary());
            AddColumn("dbo.BookMediaFiles", "FileName", c => c.String());
            AddColumn("dbo.BookMediaFiles", "SessionId", c => c.String());
            AddColumn("dbo.BookMediaFiles", "FileType", c => c.String());
            AddColumn("dbo.BookMediaFiles", "FileSize", c => c.Long(nullable: false));
            DropColumn("dbo.BookMediaFiles", "Name");
            DropColumn("dbo.BookMediaFiles", "FileId");
            DropColumn("dbo.BookMediaFiles", "ContentType");
            DropColumn("dbo.BookMediaFiles", "Size");
            DropColumn("dbo.BookMediaFiles", "CreateAt");
            DropColumn("dbo.BookMediaFiles", "CreatedAtTicks");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BookMediaFiles", "CreatedAtTicks", c => c.Long(nullable: false));
            AddColumn("dbo.BookMediaFiles", "CreateAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.BookMediaFiles", "Size", c => c.Int(nullable: false));
            AddColumn("dbo.BookMediaFiles", "ContentType", c => c.String());
            AddColumn("dbo.BookMediaFiles", "FileId", c => c.String());
            AddColumn("dbo.BookMediaFiles", "Name", c => c.String());
            DropColumn("dbo.BookMediaFiles", "FileSize");
            DropColumn("dbo.BookMediaFiles", "FileType");
            DropColumn("dbo.BookMediaFiles", "SessionId");
            DropColumn("dbo.BookMediaFiles", "FileName");
            DropColumn("dbo.BookMediaFiles", "File");
        }
    }
}
