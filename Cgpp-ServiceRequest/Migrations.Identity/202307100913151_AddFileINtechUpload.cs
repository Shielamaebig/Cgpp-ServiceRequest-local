namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFileINtechUpload : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TechnicianUploads", "FileExtension", c => c.String());
            AddColumn("dbo.TechnicianUploads", "FtpId", c => c.Int());
            CreateIndex("dbo.TechnicianUploads", "FtpId");
            AddForeignKey("dbo.TechnicianUploads", "FtpId", "dbo.Ftps", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TechnicianUploads", "FtpId", "dbo.Ftps");
            DropIndex("dbo.TechnicianUploads", new[] { "FtpId" });
            DropColumn("dbo.TechnicianUploads", "FtpId");
            DropColumn("dbo.TechnicianUploads", "FileExtension");
        }
    }
}
