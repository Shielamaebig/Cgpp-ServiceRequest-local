namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DepartmentAddFtp : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FtpHardwareModels", "FtpId", "dbo.Ftps");
            DropForeignKey("dbo.FtpHardwareModels", "HardwareUserRequestId", "dbo.HardwareUserRequests");
            DropForeignKey("dbo.FtpHardwareModels", "HardwareUserUploadsId", "dbo.HardwareUserUploads");
            DropForeignKey("dbo.FtpHardwareModels", "TechnicianReportId", "dbo.TechnicianReports");
            DropForeignKey("dbo.FtpModels", "FtpId", "dbo.Ftps");
            DropForeignKey("dbo.FtpModels", "TechnicianReportId", "dbo.TechnicianReports");
            DropForeignKey("dbo.FtpSoftwareModels", "ProgrammerReportId", "dbo.ProgrammerReports");
            DropForeignKey("dbo.FtpSoftwareModels", "SoftwareUserRequestId", "dbo.SoftwareUserRequests");
            DropForeignKey("dbo.FtpSoftwareModels", "SoftwareUserUploadsId", "dbo.SoftwareUserUploads");
            DropIndex("dbo.FtpHardwareModels", new[] { "FtpId" });
            DropIndex("dbo.FtpHardwareModels", new[] { "HardwareUserRequestId" });
            DropIndex("dbo.FtpHardwareModels", new[] { "HardwareUserUploadsId" });
            DropIndex("dbo.FtpHardwareModels", new[] { "TechnicianReportId" });
            DropIndex("dbo.FtpModels", new[] { "FtpId" });
            DropIndex("dbo.FtpModels", new[] { "TechnicianReportId" });
            DropIndex("dbo.FtpSoftwareModels", new[] { "SoftwareUserRequestId" });
            DropIndex("dbo.FtpSoftwareModels", new[] { "SoftwareUserUploadsId" });
            DropIndex("dbo.FtpSoftwareModels", new[] { "ProgrammerReportId" });
            AddColumn("dbo.Departments", "FtpId", c => c.Int());
            CreateIndex("dbo.Departments", "FtpId");
            AddForeignKey("dbo.Departments", "FtpId", "dbo.Ftps", "Id");
            DropTable("dbo.FtpHardwareModels");
            DropTable("dbo.FtpModels");
            DropTable("dbo.FtpSoftwareModels");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.FtpSoftwareModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FtpHost = c.String(),
                        FtpPort = c.String(),
                        FtpUsername = c.String(),
                        FtpPassword = c.String(),
                        SoftwareUserRequestId = c.Int(),
                        SoftwareUserUploadsId = c.Int(),
                        ProgrammerReportId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FtpModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FtpId = c.Int(),
                        Name = c.String(),
                        Description = c.String(),
                        DateAdded = c.String(),
                        TechnicianReportId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FtpHardwareModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FtpId = c.Int(),
                        HardwareUserRequestId = c.Int(),
                        HardwareUserUploadsId = c.Int(),
                        TechnicianReportId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Departments", "FtpId", "dbo.Ftps");
            DropIndex("dbo.Departments", new[] { "FtpId" });
            DropColumn("dbo.Departments", "FtpId");
            CreateIndex("dbo.FtpSoftwareModels", "ProgrammerReportId");
            CreateIndex("dbo.FtpSoftwareModels", "SoftwareUserUploadsId");
            CreateIndex("dbo.FtpSoftwareModels", "SoftwareUserRequestId");
            CreateIndex("dbo.FtpModels", "TechnicianReportId");
            CreateIndex("dbo.FtpModels", "FtpId");
            CreateIndex("dbo.FtpHardwareModels", "TechnicianReportId");
            CreateIndex("dbo.FtpHardwareModels", "HardwareUserUploadsId");
            CreateIndex("dbo.FtpHardwareModels", "HardwareUserRequestId");
            CreateIndex("dbo.FtpHardwareModels", "FtpId");
            AddForeignKey("dbo.FtpSoftwareModels", "SoftwareUserUploadsId", "dbo.SoftwareUserUploads", "Id");
            AddForeignKey("dbo.FtpSoftwareModels", "SoftwareUserRequestId", "dbo.SoftwareUserRequests", "Id");
            AddForeignKey("dbo.FtpSoftwareModels", "ProgrammerReportId", "dbo.ProgrammerReports", "Id");
            AddForeignKey("dbo.FtpModels", "TechnicianReportId", "dbo.TechnicianReports", "Id");
            AddForeignKey("dbo.FtpModels", "FtpId", "dbo.Ftps", "Id");
            AddForeignKey("dbo.FtpHardwareModels", "TechnicianReportId", "dbo.TechnicianReports", "Id");
            AddForeignKey("dbo.FtpHardwareModels", "HardwareUserUploadsId", "dbo.HardwareUserUploads", "Id");
            AddForeignKey("dbo.FtpHardwareModels", "HardwareUserRequestId", "dbo.HardwareUserRequests", "Id");
            AddForeignKey("dbo.FtpHardwareModels", "FtpId", "dbo.Ftps", "Id");
        }
    }
}
