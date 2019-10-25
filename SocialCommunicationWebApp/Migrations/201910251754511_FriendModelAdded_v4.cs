namespace SocialCommunicationWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FriendModelAdded_v4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                    "dbo.Friends",
                    c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserFromId = c.Int(nullable: false),
                        UserToId = c.Int(nullable: false),
                        Accept = c.Int(nullable: false),
                        Date = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserFromId)
                .ForeignKey("dbo.Users", t => t.UserToId)
                .Index(t => t.UserFromId)
                .Index(t => t.UserToId);

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Friends", "UserFromId", "dbo.Users");
            DropIndex("dbo.Friends", new[] { "UserFromId" });
            DropForeignKey("dbo.Friends", "UserToId", "dbo.Users");
            DropIndex("dbo.Friends", new[] { "UserToId" });
            DropTable("dbo.Friends");
        }
    }
}
