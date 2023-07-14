namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HardwareUserUpsFtpModels1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.FtpModels", name: "HardwareUserUpload_Id", newName: "HardwareUserUploadsId");
            RenameIndex(table: "dbo.FtpModels", name: "IX_HardwareUserUpload_Id", newName: "IX_HardwareUserUploadsId");
            DropColumn("dbo.FtpModels", "HardwareUserUploads");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FtpModels", "HardwareUserUploads", c => c.Int());
            RenameIndex(table: "dbo.FtpModels", name: "IX_HardwareUserUploadsId", newName: "IX_HardwareUserUpload_Id");
            RenameColumn(table: "dbo.FtpModels", name: "HardwareUserUploadsId", newName: "HardwareUserUpload_Id");
        }
    }
}
