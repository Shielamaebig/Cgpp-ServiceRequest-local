namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitializeIdentity1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HardwareUserRequests", "HardwareTaskId", "dbo.HardwareTasks");
            DropForeignKey("dbo.TechnicianReports", "HardwareTaskId", "dbo.HardwareTasks");
            DropIndex("dbo.HardwareUserRequests", new[] { "HardwareTaskId" });
            DropIndex("dbo.TechnicianReports", new[] { "HardwareTaskId" });
            DropColumn("dbo.HardwareUserRequests", "HardwareTaskId");
            DropColumn("dbo.TechnicianReports", "HardwareTaskId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TechnicianReports", "HardwareTaskId", c => c.Int(nullable: false));
            AddColumn("dbo.HardwareUserRequests", "HardwareTaskId", c => c.Int(nullable: false));
            CreateIndex("dbo.TechnicianReports", "HardwareTaskId");
            CreateIndex("dbo.HardwareUserRequests", "HardwareTaskId");
            AddForeignKey("dbo.TechnicianReports", "HardwareTaskId", "dbo.HardwareTasks", "Id", cascadeDelete: true);
            AddForeignKey("dbo.HardwareUserRequests", "HardwareTaskId", "dbo.HardwareTasks", "Id", cascadeDelete: true);
        }
    }
}
