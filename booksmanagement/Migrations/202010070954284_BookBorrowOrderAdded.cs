namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BookBorrowOrderAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookBorrowOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Purpose = c.String(),
                        BookId = c.Int(nullable: false),
                        FromDate = c.DateTime(nullable: false),
                        ToDate = c.DateTime(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        ApplicantId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicantId)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .Index(t => t.BookId)
                .Index(t => t.ApplicantId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BookBorrowOrders", "BookId", "dbo.Books");
            DropForeignKey("dbo.BookBorrowOrders", "ApplicantId", "dbo.AspNetUsers");
            DropIndex("dbo.BookBorrowOrders", new[] { "ApplicantId" });
            DropIndex("dbo.BookBorrowOrders", new[] { "BookId" });
            DropTable("dbo.BookBorrowOrders");
        }
    }
}
