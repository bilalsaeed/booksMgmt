namespace booksmanagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class dataseeduseradmin : DbMigration
    {
        public override void Up()
        {
            Sql(string.Format(@"
                  INSERT INTO AspNetRoles (Id,Name) VALUES ('{0}','Admin');
                  INSERT INTO AspNetRoles (Id,Name) VALUES ('{1}','User');", "da4509f7-bfa1-403d-89a2-44ae3aecce37", Guid.NewGuid())
               );

            Sql(@"INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [FirstName], [LastName], [ContactNumber], [IsActive]) VALUES (N'64c3fc4a-ec04-4f8f-913d-d7b4c3ba1969', N'admin@gmail.com', 0, N'AJB/txBa6fBBomChIbKITCVNjElU5ao4dj9fUePSXxlk/57Iahs4kg/r+t87pKq88g==', N'cb77957a-d570-4374-8b94-668186e4cc19', NULL, 0, 0, NULL, 1, 0, N'admin@gmail.com', N'Admin', N'User', NULL, 1)");
            //
        }

        public override void Down()
        {
        }
    }
}
