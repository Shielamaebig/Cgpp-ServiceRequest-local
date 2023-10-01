namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class plssAddFtp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SoftwareRequests", "FtpModelId", c => c.Int(nullable: true));
            CreateIndex("dbo.SoftwareRequests", "FtpModelId");
            AddForeignKey("dbo.SoftwareRequests", "FtpModelId", "dbo.FtpModels", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SoftwareRequests", "FtpModelId", "dbo.FtpModels");
            DropIndex("dbo.SoftwareRequests", new[] { "FtpModelId" });
            DropColumn("dbo.SoftwareRequests", "FtpModelId");
        }
    }
}
