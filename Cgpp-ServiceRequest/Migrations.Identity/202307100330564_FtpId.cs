namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FtpId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HardwareUserUploads", "FtpId", c => c.Int());
            CreateIndex("dbo.HardwareUserUploads", "FtpId");
            AddForeignKey("dbo.HardwareUserUploads", "FtpId", "dbo.Ftps", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HardwareUserUploads", "FtpId", "dbo.Ftps");
            DropIndex("dbo.HardwareUserUploads", new[] { "FtpId" });
            DropColumn("dbo.HardwareUserUploads", "FtpId");
        }
    }
}
