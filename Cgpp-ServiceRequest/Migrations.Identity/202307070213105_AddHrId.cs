namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHrId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ftps", "isActiveFtp", c => c.Boolean(nullable: false));
            AddColumn("dbo.FtpModels", "TechnicianReportId", c => c.Int());
            CreateIndex("dbo.FtpModels", "TechnicianReportId");
            AddForeignKey("dbo.FtpModels", "TechnicianReportId", "dbo.TechnicianReports", "Id");
            DropColumn("dbo.FtpModels", "isActiveFtp");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FtpModels", "isActiveFtp", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.FtpModels", "TechnicianReportId", "dbo.TechnicianReports");
            DropIndex("dbo.FtpModels", new[] { "TechnicianReportId" });
            DropColumn("dbo.FtpModels", "TechnicianReportId");
            DropColumn("dbo.Ftps", "isActiveFtp");
        }
    }
}
