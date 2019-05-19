namespace Periodicals.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class magazineChange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Magazines", "HostId", "dbo.Hosts");
            DropIndex("dbo.Magazines", new[] { "HostId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Magazines", "HostId");
            AddForeignKey("dbo.Magazines", "HostId", "dbo.Hosts", "Id");
        }
    }
}
