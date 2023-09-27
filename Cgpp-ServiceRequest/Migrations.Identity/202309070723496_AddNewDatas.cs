namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewDatas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HardwareAcceptsRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateAdded = c.String(),
                        HardwareUserRequestId = c.Int(nullable: false),
                        FullName = c.String(),
                        Email = c.String(),
                        DepartmentsId = c.Int(nullable: false),
                        DepartmentName = c.String(),
                        DivisionsId = c.Int(nullable: false),
                        DivisionName = c.String(),
                        IsAccept = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.DepartmentsId, cascadeDelete: false)
                .ForeignKey("dbo.Divisions", t => t.DivisionsId, cascadeDelete: false)
                .ForeignKey("dbo.HardwareUserRequests", t => t.HardwareUserRequestId, cascadeDelete: false)
                .Index(t => t.HardwareUserRequestId)
                .Index(t => t.DepartmentsId)
                .Index(t => t.DivisionsId);
            
            AddColumn("dbo.HardwareUserRequests", "TechRemarks", c => c.String());
            AddColumn("dbo.HardwareUserRequests", "DateRemarksTec", c => c.String());
            AddColumn("dbo.HardwareUserRequests", "NameTech", c => c.String());
            AddColumn("dbo.TechnicianReports", "SmsMessage", c => c.String());
            AddColumn("dbo.TechnicianReports", "DateSend", c => c.String());
            AddColumn("dbo.HardwareTasksLists", "HardwareAcceptsRequestId", c => c.Int());
            AddColumn("dbo.ProgrammerReports", "SmsMessage", c => c.String());
            AddColumn("dbo.ProgrammerReports", "DateSend", c => c.String());
            AddColumn("dbo.SoftwareUserRequests", "ProRemarks", c => c.String());
            AddColumn("dbo.SoftwareUserRequests", "DateRemarksProg", c => c.String());
            AddColumn("dbo.SoftwareUserRequests", "NameProg", c => c.String());
            AddColumn("dbo.SoftwareUserRequests", "TelNumber", c => c.String());
            CreateIndex("dbo.HardwareTasksLists", "HardwareAcceptsRequestId");
            AddForeignKey("dbo.HardwareTasksLists", "HardwareAcceptsRequestId", "dbo.HardwareAcceptsRequests", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HardwareTasksLists", "HardwareAcceptsRequestId", "dbo.HardwareAcceptsRequests");
            DropForeignKey("dbo.HardwareAcceptsRequests", "HardwareUserRequestId", "dbo.HardwareUserRequests");
            DropForeignKey("dbo.HardwareAcceptsRequests", "DivisionsId", "dbo.Divisions");
            DropForeignKey("dbo.HardwareAcceptsRequests", "DepartmentsId", "dbo.Departments");
            DropIndex("dbo.HardwareTasksLists", new[] { "HardwareAcceptsRequestId" });
            DropIndex("dbo.HardwareAcceptsRequests", new[] { "DivisionsId" });
            DropIndex("dbo.HardwareAcceptsRequests", new[] { "DepartmentsId" });
            DropIndex("dbo.HardwareAcceptsRequests", new[] { "HardwareUserRequestId" });
            DropColumn("dbo.SoftwareUserRequests", "TelNumber");
            DropColumn("dbo.SoftwareUserRequests", "NameProg");
            DropColumn("dbo.SoftwareUserRequests", "DateRemarksProg");
            DropColumn("dbo.SoftwareUserRequests", "ProRemarks");
            DropColumn("dbo.ProgrammerReports", "DateSend");
            DropColumn("dbo.ProgrammerReports", "SmsMessage");
            DropColumn("dbo.HardwareTasksLists", "HardwareAcceptsRequestId");
            DropColumn("dbo.TechnicianReports", "DateSend");
            DropColumn("dbo.TechnicianReports", "SmsMessage");
            DropColumn("dbo.HardwareUserRequests", "NameTech");
            DropColumn("dbo.HardwareUserRequests", "DateRemarksTec");
            DropColumn("dbo.HardwareUserRequests", "TechRemarks");
            DropTable("dbo.HardwareAcceptsRequests");
        }
    }
}
