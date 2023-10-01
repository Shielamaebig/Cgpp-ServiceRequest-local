namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class taskListv2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HardwareTasksLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HardwareTaskId = c.Int(nullable: false),
                        DateAdded = c.String(),
                        HardwareUserRequestId = c.Int(),
                        TechnicianReportId = c.Int(),
                        HardwareVerifyId = c.Int(),
                        HardwareApprovalId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HardwareApprovals", t => t.HardwareApprovalId)
                .ForeignKey("dbo.HardwareTasks", t => t.HardwareTaskId, cascadeDelete: true)
                .ForeignKey("dbo.HardwareUserRequests", t => t.HardwareUserRequestId)
                .ForeignKey("dbo.HardwareVerifies", t => t.HardwareVerifyId)
                .ForeignKey("dbo.TechnicianReports", t => t.TechnicianReportId)
                .Index(t => t.HardwareTaskId)
                .Index(t => t.HardwareUserRequestId)
                .Index(t => t.TechnicianReportId)
                .Index(t => t.HardwareVerifyId)
                .Index(t => t.HardwareApprovalId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HardwareTasksLists", "TechnicianReportId", "dbo.TechnicianReports");
            DropForeignKey("dbo.HardwareTasksLists", "HardwareVerifyId", "dbo.HardwareVerifies");
            DropForeignKey("dbo.HardwareTasksLists", "HardwareUserRequestId", "dbo.HardwareUserRequests");
            DropForeignKey("dbo.HardwareTasksLists", "HardwareTaskId", "dbo.HardwareTasks");
            DropForeignKey("dbo.HardwareTasksLists", "HardwareApprovalId", "dbo.HardwareApprovals");
            DropIndex("dbo.HardwareTasksLists", new[] { "HardwareApprovalId" });
            DropIndex("dbo.HardwareTasksLists", new[] { "HardwareVerifyId" });
            DropIndex("dbo.HardwareTasksLists", new[] { "TechnicianReportId" });
            DropIndex("dbo.HardwareTasksLists", new[] { "HardwareUserRequestId" });
            DropIndex("dbo.HardwareTasksLists", new[] { "HardwareTaskId" });
            DropTable("dbo.HardwareTasksLists");
        }
    }
}
