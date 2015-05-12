namespace BetterGroceryList.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initialize : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        FullName = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.BetterLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfile", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.BetterListMembers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        ListMembershipId = c.Int(nullable: false),
                        ListItemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BetterLists", t => t.ListMembershipId, cascadeDelete: true)
                .ForeignKey("dbo.BetterListItems", t => t.ListItemId, cascadeDelete: true)
                .Index(t => t.ListMembershipId)
                .Index(t => t.ListItemId);
            
            CreateTable(
                "dbo.BetterListItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfile", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ExtraUserInformation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SharedBetterLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        UserEmailAddress = c.String(),
                        ListId = c.Int(nullable: false),
                        SharingId = c.Guid(nullable: false),
                        SharingConfirmed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfile", t => t.UserId)
                .ForeignKey("dbo.BetterLists", t => t.ListId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ListId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.SharedBetterLists", new[] { "ListId" });
            DropIndex("dbo.SharedBetterLists", new[] { "UserId" });
            DropIndex("dbo.BetterListItems", new[] { "UserId" });
            DropIndex("dbo.BetterListMembers", new[] { "ListItemId" });
            DropIndex("dbo.BetterListMembers", new[] { "ListMembershipId" });
            DropIndex("dbo.BetterLists", new[] { "UserId" });
            DropForeignKey("dbo.SharedBetterLists", "ListId", "dbo.BetterLists");
            DropForeignKey("dbo.SharedBetterLists", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.BetterListItems", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.BetterListMembers", "ListItemId", "dbo.BetterListItems");
            DropForeignKey("dbo.BetterListMembers", "ListMembershipId", "dbo.BetterLists");
            DropForeignKey("dbo.BetterLists", "UserId", "dbo.UserProfile");
            DropTable("dbo.SharedBetterLists");
            DropTable("dbo.ExtraUserInformation");
            DropTable("dbo.BetterListItems");
            DropTable("dbo.BetterListMembers");
            DropTable("dbo.BetterLists");
            DropTable("dbo.UserProfile");
        }
    }
}
