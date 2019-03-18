namespace Periodicals.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class magazineChange2 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Magazines", "HostId");
            AddForeignKey("dbo.Magazines", "HostId", "dbo.Hosts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Magazines", "HostId", "dbo.Hosts");
            DropIndex("dbo.Magazines", new[] { "HostId" });
        }
    }
}
