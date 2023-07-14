namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HardwareUserUpsFtpModels : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FtpModels", "HardwareUserRequestId", "dbo.HardwareUserRequests");
            DropIndex("dbo.FtpModels", new[] { "HardwareUserRequestId" });
            AddColumn("dbo.FtpModels", "HardwareUserUploads", c => c.Int());
            AddColumn("dbo.FtpModels", "HardwareUserUpload_Id", c => c.Int());
            AlterColumn("dbo.FtpModels", "HardwareUserRequestId", c => c.Int());
            CreateIndex("dbo.FtpModels", "HardwareUserRequestId");
            CreateIndex("dbo.FtpModels", "HardwareUserUpload_Id");
            AddForeignKey("dbo.FtpModels", "HardwareUserUpload_Id", "dbo.HardwareUserUploads", "Id");
            AddForeignKey("dbo.FtpModels", "HardwareUserRequestId", "dbo.HardwareUserRequests", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FtpModels", "HardwareUserRequestId", "dbo.HardwareUserRequests");
            DropForeignKey("dbo.FtpModels", "HardwareUserUpload_Id", "dbo.HardwareUserUploads");
            DropIndex("dbo.FtpModels", new[] { "HardwareUserUpload_Id" });
            DropIndex("dbo.FtpModels", new[] { "HardwareUserRequestId" });
            AlterColumn("dbo.FtpModels", "HardwareUserRequestId", c => c.Int(nullable: false));
            DropColumn("dbo.FtpModels", "HardwareUserUpload_Id");
            DropColumn("dbo.FtpModels", "HardwareUserUploads");
            CreateIndex("dbo.FtpModels", "HardwareUserRequestId");
            AddForeignKey("dbo.FtpModels", "HardwareUserRequestId", "dbo.HardwareUserRequests", "Id", cascadeDelete: true);
        }
    }
}
