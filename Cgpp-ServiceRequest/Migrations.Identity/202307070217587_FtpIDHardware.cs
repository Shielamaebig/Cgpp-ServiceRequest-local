namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FtpIDHardware : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FtpHardwareModels", "FtpId", c => c.Int());
            CreateIndex("dbo.FtpHardwareModels", "FtpId");
            AddForeignKey("dbo.FtpHardwareModels", "FtpId", "dbo.Ftps", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FtpHardwareModels", "FtpId", "dbo.Ftps");
            DropIndex("dbo.FtpHardwareModels", new[] { "FtpId" });
            DropColumn("dbo.FtpHardwareModels", "FtpId");
        }
    }
}
