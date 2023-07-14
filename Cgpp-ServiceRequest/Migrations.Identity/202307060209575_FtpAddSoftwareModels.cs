namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FtpAddSoftwareModels : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FtpHardwareModels", "ProgrammerReportId", "dbo.ProgrammerReports");
            DropIndex("dbo.FtpHardwareModels", new[] { "ProgrammerReportId" });
            AddColumn("dbo.FtpHardwareModels", "TechnicianReportId", c => c.Int());
            CreateIndex("dbo.FtpHardwareModels", "TechnicianReportId");
            AddForeignKey("dbo.FtpHardwareModels", "TechnicianReportId", "dbo.TechnicianReports", "Id");
            DropColumn("dbo.FtpHardwareModels", "ProgrammerReportId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FtpHardwareModels", "ProgrammerReportId", c => c.Int());
            DropForeignKey("dbo.FtpHardwareModels", "TechnicianReportId", "dbo.TechnicianReports");
            DropIndex("dbo.FtpHardwareModels", new[] { "TechnicianReportId" });
            DropColumn("dbo.FtpHardwareModels", "TechnicianReportId");
            CreateIndex("dbo.FtpHardwareModels", "ProgrammerReportId");
            AddForeignKey("dbo.FtpHardwareModels", "ProgrammerReportId", "dbo.ProgrammerReports", "Id");
        }
    }
}
