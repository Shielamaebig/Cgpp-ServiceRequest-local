namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeFtp : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SoftwareRequests", "FtpModelId", "dbo.FtpModels");
            DropIndex("dbo.SoftwareRequests", new[] { "FtpModelId" });
            DropColumn("dbo.SoftwareRequests", "FtpModelId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SoftwareRequests", "FtpModelId", c => c.Int(nullable: false));
            CreateIndex("dbo.SoftwareRequests", "FtpModelId");
            AddForeignKey("dbo.SoftwareRequests", "FtpModelId", "dbo.FtpModels", "Id", cascadeDelete: true);
        }
    }
}
