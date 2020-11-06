namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DrawingFileModelChanged : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DrawingFiles", "File", c => c.Binary());
            AddColumn("dbo.DrawingFiles", "FileName", c => c.String());
            AddColumn("dbo.DrawingFiles", "SessionId", c => c.String());
            AddColumn("dbo.DrawingFiles", "FileType", c => c.String());
            AddColumn("dbo.DrawingFiles", "FileSize", c => c.Long(nullable: false));
            DropColumn("dbo.DrawingFiles", "Name");
            DropColumn("dbo.DrawingFiles", "FileId");
            DropColumn("dbo.DrawingFiles", "ContentType");
            DropColumn("dbo.DrawingFiles", "Size");
            DropColumn("dbo.DrawingFiles", "CreateAt");
            DropColumn("dbo.DrawingFiles", "CreatedAtTicks");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DrawingFiles", "CreatedAtTicks", c => c.Long(nullable: false));
            AddColumn("dbo.DrawingFiles", "CreateAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.DrawingFiles", "Size", c => c.Int(nullable: false));
            AddColumn("dbo.DrawingFiles", "ContentType", c => c.String());
            AddColumn("dbo.DrawingFiles", "FileId", c => c.String());
            AddColumn("dbo.DrawingFiles", "Name", c => c.String());
            DropColumn("dbo.DrawingFiles", "FileSize");
            DropColumn("dbo.DrawingFiles", "FileType");
            DropColumn("dbo.DrawingFiles", "SessionId");
            DropColumn("dbo.DrawingFiles", "FileName");
            DropColumn("dbo.DrawingFiles", "File");
        }
    }
}
