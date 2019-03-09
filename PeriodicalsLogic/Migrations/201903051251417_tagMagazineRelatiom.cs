namespace Periodicals.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tagMagazineRelatiom : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Hosts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        ProfilePicture = c.String(),
                        Role = c.String(),
                        IsBlocked = c.Boolean(nullable: false),
                        Wallet = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HostMagazines",
                c => new
                    {
                        HostMagazineId = c.Int(nullable: false, identity: true),
                        HostId = c.Int(),
                        MagazineId = c.Int(),
                    })
                .PrimaryKey(t => t.HostMagazineId)
                .ForeignKey("dbo.Hosts", t => t.HostId)
                .ForeignKey("dbo.Magazines", t => t.MagazineId)
                .Index(t => t.HostId)
                .Index(t => t.MagazineId);
            
            CreateTable(
                "dbo.Magazines",
                c => new
                    {
                        MagazineId = c.Int(nullable: false, identity: true),
                        MagazineName = c.String(),
                        PublishDate = c.DateTime(nullable: false),
                        Description = c.String(),
                        Price = c.Int(nullable: false),
                        Cover = c.String(),
                        HostId = c.Int(),
                    })
                .PrimaryKey(t => t.MagazineId)
                .ForeignKey("dbo.Hosts", t => t.HostId)
                .Index(t => t.HostId);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        TagId = c.Int(nullable: false, identity: true),
                        TagName = c.String(),
                    })
                .PrimaryKey(t => t.TagId);
            
            CreateTable(
                "dbo.TagMagazines",
                c => new
                    {
                        TagMagazineId = c.Int(nullable: false, identity: true),
                        TagId = c.Int(),
                        MagazineId = c.Int(),
                    })
                .PrimaryKey(t => t.TagMagazineId)
                .ForeignKey("dbo.Magazines", t => t.MagazineId)
                .ForeignKey("dbo.Tags", t => t.TagId)
                .Index(t => t.TagId)
                .Index(t => t.MagazineId);
            
            CreateTable(
                "dbo.IdentityRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityUserRoles",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        IdentityRole_Id = c.String(maxLength: 128),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.IdentityRoles", t => t.IdentityRole_Id)
                .ForeignKey("dbo.IdentityUsers", t => t.IdentityUser_Id)
                .Index(t => t.IdentityRole_Id)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.IdentityUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IdentityUsers", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.IdentityUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.IdentityUsers", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.TagMagazine",
                c => new
                    {
                        MagazineId = c.Int(nullable: false),
                        TagId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MagazineId, t.TagId })
                .ForeignKey("dbo.Magazines", t => t.MagazineId, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.TagId, cascadeDelete: true)
                .Index(t => t.MagazineId)
                .Index(t => t.TagId);
            
            CreateTable(
                "dbo.HostMagazine",
                c => new
                    {
                        HostId = c.Int(nullable: false),
                        MagazineId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.HostId, t.MagazineId })
                .ForeignKey("dbo.Hosts", t => t.HostId, cascadeDelete: true)
                .ForeignKey("dbo.Magazines", t => t.MagazineId, cascadeDelete: true)
                .Index(t => t.HostId)
                .Index(t => t.MagazineId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IdentityUserRoles", "IdentityUser_Id", "dbo.IdentityUsers");
            DropForeignKey("dbo.IdentityUserLogins", "IdentityUser_Id", "dbo.IdentityUsers");
            DropForeignKey("dbo.IdentityUserClaims", "IdentityUser_Id", "dbo.IdentityUsers");
            DropForeignKey("dbo.IdentityUserRoles", "IdentityRole_Id", "dbo.IdentityRoles");
            DropForeignKey("dbo.HostMagazine", "MagazineId", "dbo.Magazines");
            DropForeignKey("dbo.HostMagazine", "HostId", "dbo.Hosts");
            DropForeignKey("dbo.HostMagazines", "MagazineId", "dbo.Magazines");
            DropForeignKey("dbo.TagMagazine", "TagId", "dbo.Tags");
            DropForeignKey("dbo.TagMagazine", "MagazineId", "dbo.Magazines");
            DropForeignKey("dbo.TagMagazines", "TagId", "dbo.Tags");
            DropForeignKey("dbo.TagMagazines", "MagazineId", "dbo.Magazines");
            DropForeignKey("dbo.Magazines", "HostId", "dbo.Hosts");
            DropForeignKey("dbo.HostMagazines", "HostId", "dbo.Hosts");
            DropIndex("dbo.HostMagazine", new[] { "MagazineId" });
            DropIndex("dbo.HostMagazine", new[] { "HostId" });
            DropIndex("dbo.TagMagazine", new[] { "TagId" });
            DropIndex("dbo.TagMagazine", new[] { "MagazineId" });
            DropIndex("dbo.IdentityUserLogins", new[] { "IdentityUser_Id" });
            DropIndex("dbo.IdentityUserClaims", new[] { "IdentityUser_Id" });
            DropIndex("dbo.IdentityUserRoles", new[] { "IdentityUser_Id" });
            DropIndex("dbo.IdentityUserRoles", new[] { "IdentityRole_Id" });
            DropIndex("dbo.TagMagazines", new[] { "MagazineId" });
            DropIndex("dbo.TagMagazines", new[] { "TagId" });
            DropIndex("dbo.Magazines", new[] { "HostId" });
            DropIndex("dbo.HostMagazines", new[] { "MagazineId" });
            DropIndex("dbo.HostMagazines", new[] { "HostId" });
            DropTable("dbo.HostMagazine");
            DropTable("dbo.TagMagazine");
            DropTable("dbo.IdentityUserLogins");
            DropTable("dbo.IdentityUserClaims");
            DropTable("dbo.IdentityUsers");
            DropTable("dbo.IdentityUserRoles");
            DropTable("dbo.IdentityRoles");
            DropTable("dbo.TagMagazines");
            DropTable("dbo.Tags");
            DropTable("dbo.Magazines");
            DropTable("dbo.HostMagazines");
            DropTable("dbo.Hosts");
        }
    }
}
