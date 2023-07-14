namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FtpSoftwareModels : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FtpHardwareModels", "ProgrammerReportId", c => c.Int());
            CreateIndex("dbo.FtpHardwareModels", "ProgrammerReportId");
            AddForeignKey("dbo.FtpHardwareModels", "ProgrammerReportId", "dbo.ProgrammerReports", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FtpHardwareModels", "ProgrammerReportId", "dbo.ProgrammerReports");
            DropIndex("dbo.FtpHardwareModels", new[] { "ProgrammerReportId" });
            DropColumn("dbo.FtpHardwareModels", "ProgrammerReportId");
        }
    }
}
