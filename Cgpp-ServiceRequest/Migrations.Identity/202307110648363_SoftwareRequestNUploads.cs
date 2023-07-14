namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SoftwareRequestNUploads : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SoftwareUserRequests", "DivisionsId", "dbo.Divisions");
            DropIndex("dbo.SoftwareUserRequests", new[] { "DivisionsId" });
            AddColumn("dbo.SoftwareUserRequests", "FirstName", c => c.String());
            AddColumn("dbo.SoftwareUserRequests", "MiddleName", c => c.String());
            AddColumn("dbo.SoftwareUserRequests", "LastName", c => c.String());
            AddColumn("dbo.SoftwareUserUploads", "FileExtension", c => c.String());
            AddColumn("dbo.SoftwareUserUploads", "FtpId", c => c.Int());
            AlterColumn("dbo.SoftwareUserRequests", "DivisionsId", c => c.Int());
            CreateIndex("dbo.SoftwareUserRequests", "DivisionsId");
            CreateIndex("dbo.SoftwareUserUploads", "FtpId");
            AddForeignKey("dbo.SoftwareUserUploads", "FtpId", "dbo.Ftps", "Id");
            AddForeignKey("dbo.SoftwareUserRequests", "DivisionsId", "dbo.Divisions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SoftwareUserRequests", "DivisionsId", "dbo.Divisions");
            DropForeignKey("dbo.SoftwareUserUploads", "FtpId", "dbo.Ftps");
            DropIndex("dbo.SoftwareUserUploads", new[] { "FtpId" });
            DropIndex("dbo.SoftwareUserRequests", new[] { "DivisionsId" });
            AlterColumn("dbo.SoftwareUserRequests", "DivisionsId", c => c.Int(nullable: false));
            DropColumn("dbo.SoftwareUserUploads", "FtpId");
            DropColumn("dbo.SoftwareUserUploads", "FileExtension");
            DropColumn("dbo.SoftwareUserRequests", "LastName");
            DropColumn("dbo.SoftwareUserRequests", "MiddleName");
            DropColumn("dbo.SoftwareUserRequests", "FirstName");
            CreateIndex("dbo.SoftwareUserRequests", "DivisionsId");
            AddForeignKey("dbo.SoftwareUserRequests", "DivisionsId", "dbo.Divisions", "Id", cascadeDelete: true);
        }
    }
}
