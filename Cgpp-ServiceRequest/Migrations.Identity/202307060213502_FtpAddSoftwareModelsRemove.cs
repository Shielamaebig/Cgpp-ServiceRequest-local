namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FtpAddSoftwareModelsRemove : DbMigration
    {
        public override void Up()
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProgrammerReports", t => t.ProgrammerReportId)
                .ForeignKey("dbo.SoftwareUserRequests", t => t.SoftwareUserRequestId)
                .ForeignKey("dbo.SoftwareUserUploads", t => t.SoftwareUserUploadsId)
                .Index(t => t.SoftwareUserRequestId)
                .Index(t => t.SoftwareUserUploadsId)
                .Index(t => t.ProgrammerReportId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FtpSoftwareModels", "SoftwareUserUploadsId", "dbo.SoftwareUserUploads");
            DropForeignKey("dbo.FtpSoftwareModels", "SoftwareUserRequestId", "dbo.SoftwareUserRequests");
            DropForeignKey("dbo.FtpSoftwareModels", "ProgrammerReportId", "dbo.ProgrammerReports");
            DropIndex("dbo.FtpSoftwareModels", new[] { "ProgrammerReportId" });
            DropIndex("dbo.FtpSoftwareModels", new[] { "SoftwareUserUploadsId" });
            DropIndex("dbo.FtpSoftwareModels", new[] { "SoftwareUserRequestId" });
            DropTable("dbo.FtpSoftwareModels");
        }
    }
}
