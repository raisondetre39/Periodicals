namespace Periodicals.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tagChange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TagMagazines", "MagazineId", "dbo.Magazines");
            DropForeignKey("dbo.TagMagazines", "TagId", "dbo.Tags");
            DropIndex("dbo.TagMagazines", new[] { "TagId" });
            DropIndex("dbo.TagMagazines", new[] { "MagazineId" });
            DropTable("dbo.TagMagazines");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TagMagazines",
                c => new
                    {
                        TagMagazineId = c.Int(nullable: false, identity: true),
                        TagId = c.Int(),
                        MagazineId = c.Int(),
                    })
                .PrimaryKey(t => t.TagMagazineId);
            
            CreateIndex("dbo.TagMagazines", "MagazineId");
            CreateIndex("dbo.TagMagazines", "TagId");
            AddForeignKey("dbo.TagMagazines", "TagId", "dbo.Tags", "TagId");
            AddForeignKey("dbo.TagMagazines", "MagazineId", "dbo.Magazines", "MagazineId");
        }
    }
}
