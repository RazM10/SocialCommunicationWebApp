namespace SocialCommunicationWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MessageModelAdded_v5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FromId = c.Int(nullable: false),
                        ToId = c.Int(nullable: false),
                        MessageDetails = c.String(),
                        Seen = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Messages");
        }
    }
}
