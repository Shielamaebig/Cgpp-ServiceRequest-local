namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsActive : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TechnicianReports", "TechnicianAdminId", "dbo.TechnicianAdmins");
            DropForeignKey("dbo.HardwareRequests", "DepartmentsId", "dbo.Departments");
            DropForeignKey("dbo.HardwareRequests", "DivisionsId", "dbo.Divisions");
            DropForeignKey("dbo.HardwareRequests", "FindingId", "dbo.Findings");
            DropForeignKey("dbo.HardwareRequests", "HardwareId", "dbo.Hardwares");
            DropForeignKey("dbo.HardwareRequests", "HardwareTechnicianId", "dbo.HardwareTechnicians");
            DropForeignKey("dbo.HardwareRequests", "StatusId", "dbo.Status");
            DropForeignKey("dbo.HardwareRequests", "UnitTypeId", "dbo.UnitTypes");
            DropForeignKey("dbo.HardwareRequestHistories", "DepartmentsId", "dbo.Departments");
            DropForeignKey("dbo.HardwareRequestHistories", "DivisionsId", "dbo.Divisions");
            DropForeignKey("dbo.HardwareSubTasks", "HardwareTaskId", "dbo.HardwareTasks");
            DropForeignKey("dbo.HardwareTaskLists", "HardwareApprovalId", "dbo.HardwareApprovals");
            DropForeignKey("dbo.HardwareTaskLists", "HardwareTaskId", "dbo.HardwareTasks");
            DropForeignKey("dbo.HardwareTaskLists", "HardwareUserRequestId", "dbo.HardwareUserRequests");
            DropForeignKey("dbo.HardwareTaskLists", "HardwareVerifyId", "dbo.HardwareVerifies");
            DropForeignKey("dbo.HardwareTaskLists", "TechnicianReportId", "dbo.TechnicianReports");
            DropForeignKey("dbo.HardwareUploads", "HardwareRequestId", "dbo.HardwareRequests");
            DropForeignKey("dbo.SoftwareRequests", "DepartmentsId", "dbo.Departments");
            DropForeignKey("dbo.SoftwareRequests", "DivisionsId", "dbo.Divisions");
            DropForeignKey("dbo.SoftwareRequests", "InformationSystemId", "dbo.InformationSystems");
            DropForeignKey("dbo.SoftwareRequests", "SoftwareId", "dbo.Softwares");
            DropForeignKey("dbo.SoftwareRequests", "SoftwareTechnicianId", "dbo.SoftwareTechnicians");
            DropForeignKey("dbo.SoftwareRequests", "StatusId", "dbo.Status");
            DropForeignKey("dbo.SoftwareRequestHistories", "DepartmentsId", "dbo.Departments");
            DropForeignKey("dbo.SoftwareRequestHistories", "DivisionsId", "dbo.Divisions");
            DropForeignKey("dbo.SoftwareUploads", "SoftwareRequestId", "dbo.SoftwareRequests");
            DropForeignKey("dbo.SoftwareUserRequests", "DivisionsId", "dbo.Divisions");
            DropForeignKey("dbo.SoftwareVerifications", "SoftwareUserRequestId", "dbo.SoftwareUserRequests");
            DropIndex("dbo.TechnicianReports", new[] { "TechnicianAdminId" });
            DropIndex("dbo.HardwareRequests", new[] { "HardwareId" });
            DropIndex("dbo.HardwareRequests", new[] { "DepartmentsId" });
            DropIndex("dbo.HardwareRequests", new[] { "UnitTypeId" });
            DropIndex("dbo.HardwareRequests", new[] { "DivisionsId" });
            DropIndex("dbo.HardwareRequests", new[] { "StatusId" });
            DropIndex("dbo.HardwareRequests", new[] { "HardwareTechnicianId" });
            DropIndex("dbo.HardwareRequests", new[] { "FindingId" });
            DropIndex("dbo.HardwareRequestHistories", new[] { "DepartmentsId" });
            DropIndex("dbo.HardwareRequestHistories", new[] { "DivisionsId" });
            DropIndex("dbo.HardwareSubTasks", new[] { "HardwareTaskId" });
            DropIndex("dbo.HardwareTaskLists", new[] { "HardwareTaskId" });
            DropIndex("dbo.HardwareTaskLists", new[] { "HardwareUserRequestId" });
            DropIndex("dbo.HardwareTaskLists", new[] { "TechnicianReportId" });
            DropIndex("dbo.HardwareTaskLists", new[] { "HardwareVerifyId" });
            DropIndex("dbo.HardwareTaskLists", new[] { "HardwareApprovalId" });
            DropIndex("dbo.HardwareUploads", new[] { "HardwareRequestId" });
            DropIndex("dbo.SoftwareUserRequests", new[] { "DivisionsId" });
            DropIndex("dbo.SoftwareVerifications", new[] { "SoftwareUserRequestId" });
            DropIndex("dbo.SoftwareRequests", new[] { "InformationSystemId" });
            DropIndex("dbo.SoftwareRequests", new[] { "DepartmentsId" });
            DropIndex("dbo.SoftwareRequests", new[] { "DivisionsId" });
            DropIndex("dbo.SoftwareRequests", new[] { "SoftwareId" });
            DropIndex("dbo.SoftwareRequests", new[] { "StatusId" });
            DropIndex("dbo.SoftwareRequests", new[] { "SoftwareTechnicianId" });
            DropIndex("dbo.SoftwareRequestHistories", new[] { "DepartmentsId" });
            DropIndex("dbo.SoftwareRequestHistories", new[] { "DivisionsId" });
            DropIndex("dbo.SoftwareUploads", new[] { "SoftwareRequestId" });
            CreateTable(
                "dbo.Ftps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FtpHost = c.String(),
                        FtpPort = c.String(),
                        FtpUsername = c.String(),
                        FtpPassword = c.String(),
                        DateAdded = c.String(),
                        FolderName = c.String(),
                        Label = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Departments", "FtpId", c => c.Int());
            AddColumn("dbo.FullCalendars", "Name", c => c.String());
            AddColumn("dbo.HardwareUserRequests", "FirstName", c => c.String());
            AddColumn("dbo.HardwareUserRequests", "MiddleName", c => c.String());
            AddColumn("dbo.HardwareUserRequests", "LastName", c => c.String());
            AddColumn("dbo.HardwareTechnicians", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.HardwareUserUploads", "FileExtension", c => c.String());
            AddColumn("dbo.HardwareUserUploads", "FtpId", c => c.Int());
            AddColumn("dbo.LoginActivities", "DepartmentName", c => c.String());
            AddColumn("dbo.LoginActivities", "DivisionName", c => c.String());
            AddColumn("dbo.SoftwareUserRequests", "FirstName", c => c.String());
            AddColumn("dbo.SoftwareUserRequests", "MiddleName", c => c.String());
            AddColumn("dbo.SoftwareUserRequests", "LastName", c => c.String());
            AddColumn("dbo.SoftwareTechnicians", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.ProgrammerUploads", "FileExtension", c => c.String());
            AddColumn("dbo.ProgrammerUploads", "FtpId", c => c.Int());
            AddColumn("dbo.RequestHistories", "Category", c => c.Boolean(nullable: false));
            AddColumn("dbo.RequestHistories", "HardwareUserRequestId", c => c.Int());
            AddColumn("dbo.RequestHistories", "SoftwareUserRequestId", c => c.Int());
            AddColumn("dbo.SoftwareUserUploads", "FileExtension", c => c.String());
            AddColumn("dbo.SoftwareUserUploads", "FtpId", c => c.Int());
            AddColumn("dbo.TechnicianUploads", "FileExtension", c => c.String());
            AddColumn("dbo.TechnicianUploads", "FtpId", c => c.Int());
            AddColumn("dbo.UserForgotPasswords", "FirstName", c => c.String(nullable: false));
            AddColumn("dbo.UserForgotPasswords", "LastName", c => c.String(nullable: false));
            AddColumn("dbo.UserForgotPasswords", "MiddleName", c => c.String());
            AddColumn("dbo.UserRegistrations", "FirstName", c => c.String(nullable: false));
            AddColumn("dbo.UserRegistrations", "LastName", c => c.String(nullable: false));
            AddColumn("dbo.UserRegistrations", "MiddleName", c => c.String());
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "MiddleName", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String(nullable: false));
            AlterColumn("dbo.HardwareUserRequests", "DocumentLabel", c => c.String(nullable: false));
            AlterColumn("dbo.HardwareUserRequests", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.SoftwareUserRequests", "DivisionsId", c => c.Int());
            AlterColumn("dbo.SoftwareVerifications", "SoftwareUserRequestId", c => c.Int());
            AlterColumn("dbo.UserRegistrations", "Email", c => c.String());
            AlterColumn("dbo.AspNetUsers", "FullName", c => c.String());
            CreateIndex("dbo.Departments", "FtpId");
            CreateIndex("dbo.HardwareUserUploads", "FtpId");
            CreateIndex("dbo.SoftwareUserRequests", "DivisionsId");
            CreateIndex("dbo.ProgrammerUploads", "FtpId");
            CreateIndex("dbo.RequestHistories", "HardwareUserRequestId");
            CreateIndex("dbo.RequestHistories", "SoftwareUserRequestId");
            CreateIndex("dbo.SoftwareVerifications", "SoftwareUserRequestId");
            CreateIndex("dbo.SoftwareUserUploads", "FtpId");
            CreateIndex("dbo.TechnicianUploads", "FtpId");
            AddForeignKey("dbo.Departments", "FtpId", "dbo.Ftps", "Id");
            AddForeignKey("dbo.HardwareUserUploads", "FtpId", "dbo.Ftps", "Id");
            AddForeignKey("dbo.ProgrammerUploads", "FtpId", "dbo.Ftps", "Id");
            AddForeignKey("dbo.RequestHistories", "HardwareUserRequestId", "dbo.HardwareUserRequests", "Id");
            AddForeignKey("dbo.RequestHistories", "SoftwareUserRequestId", "dbo.SoftwareUserRequests", "Id");
            AddForeignKey("dbo.SoftwareUserUploads", "FtpId", "dbo.Ftps", "Id");
            AddForeignKey("dbo.TechnicianUploads", "FtpId", "dbo.Ftps", "Id");
            AddForeignKey("dbo.SoftwareUserRequests", "DivisionsId", "dbo.Divisions", "Id");
            AddForeignKey("dbo.SoftwareVerifications", "SoftwareUserRequestId", "dbo.SoftwareUserRequests", "Id");
            DropColumn("dbo.HardwareUserRequests", "TaskState");
            DropColumn("dbo.TechnicianReports", "TechnicianAdminId");
            DropColumn("dbo.UserForgotPasswords", "FullName");
            DropColumn("dbo.UserForgotPasswords", "Email");
            DropColumn("dbo.UserRegistrations", "FullName");
            DropTable("dbo.FtpModels");
            DropTable("dbo.TechnicianAdmins");
            DropTable("dbo.HardwareRequests");
            DropTable("dbo.Status");
            DropTable("dbo.HardwareRequestHistories");
            DropTable("dbo.HardwareSubTasks");
            DropTable("dbo.HardwareTaskLists");
            DropTable("dbo.HardwareUploads");
            DropTable("dbo.HardwareVerifications");
            DropTable("dbo.SoftwareRequests");
            DropTable("dbo.SoftwareRequestHistories");
            DropTable("dbo.SoftwareUploads");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SoftwareUploads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImagePath = c.String(),
                        FileName = c.String(),
                        SoftwareRequestId = c.Int(nullable: false),
                        DateAdded = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SoftwareRequestHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Email = c.String(),
                        DepartmentsId = c.Int(),
                        DivisionsId = c.Int(),
                        RequetMessage = c.String(),
                        RequetDate = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SoftwareRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ticket = c.String(),
                        DateAdded = c.String(),
                        Email = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        MobileNumber = c.String(),
                        FullName = c.String(),
                        RequestFor = c.String(),
                        Description = c.String(),
                        InformationSystemId = c.Int(),
                        DepartmentsId = c.Int(),
                        DivisionsId = c.Int(),
                        SoftwareId = c.Int(nullable: false),
                        StatusId = c.Int(),
                        SoftwareTechnicianId = c.Int(),
                        ProEmail = c.String(),
                        IsAssigned = c.Boolean(nullable: false),
                        TimeStarted = c.String(),
                        TimeEnded = c.String(),
                        DateStarted = c.String(),
                        DateEnded = c.String(),
                        Remarks = c.String(),
                        IsReported = c.Boolean(nullable: false),
                        DateApproved = c.String(),
                        Approver = c.String(),
                        ApproverRemarks = c.String(),
                        IsApproved = c.Boolean(nullable: false),
                        DateView = c.String(),
                        Viewer = c.String(),
                        IsViewed = c.Boolean(nullable: false),
                        DocumentLabel = c.String(),
                        IsNew = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HardwareVerifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateVerified = c.String(),
                        NameVerified = c.String(),
                        EmailVerified = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HardwareUploads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImagePath = c.String(),
                        FileName = c.String(),
                        HardwareRequestId = c.Int(nullable: false),
                        DateAdded = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HardwareTaskLists",
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
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HardwareSubTasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HardwareTaskId = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HardwareRequestHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Email = c.String(),
                        DepartmentsId = c.Int(),
                        DivisionsId = c.Int(),
                        RequetMessage = c.String(),
                        RequetDate = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HardwareRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ticket = c.String(),
                        DateAdded = c.String(),
                        Email = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        ModelName = c.String(),
                        MobileNumber = c.String(),
                        FullName = c.String(),
                        Description = c.String(),
                        BrandName = c.String(),
                        HardwareId = c.Int(nullable: false),
                        DepartmentsId = c.Int(),
                        UnitTypeId = c.Int(),
                        DivisionsId = c.Int(),
                        StatusId = c.Int(),
                        HardwareTechnicianId = c.Int(),
                        TechEmail = c.String(),
                        IsAssigned = c.Boolean(nullable: false),
                        PossibleCause = c.String(),
                        FindingId = c.Int(),
                        DateStarted = c.String(),
                        TimeStarted = c.String(),
                        TimeEnded = c.String(),
                        DateEnded = c.String(),
                        IsReported = c.Boolean(nullable: false),
                        IsVerified = c.Boolean(nullable: false),
                        Remarks = c.String(),
                        DateApproved = c.String(),
                        Approver = c.String(),
                        ApproverRemarks = c.String(),
                        IsApproved = c.Boolean(nullable: false),
                        DateView = c.String(),
                        Viewer = c.String(),
                        ViewedRemarks = c.String(),
                        DocumentLabel = c.String(),
                        IsNew = c.Boolean(nullable: false),
                        SerialNumber = c.String(),
                        ControlNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TechnicianAdmins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FtpModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Address = c.String(),
                        Port = c.Int(nullable: false),
                        UserName = c.String(),
                        Password = c.String(),
                        Label = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.UserRegistrations", "FullName", c => c.String(nullable: false));
            AddColumn("dbo.UserForgotPasswords", "Email", c => c.String(nullable: false));
            AddColumn("dbo.UserForgotPasswords", "FullName", c => c.String(nullable: false));
            AddColumn("dbo.TechnicianReports", "TechnicianAdminId", c => c.Int());
            AddColumn("dbo.HardwareUserRequests", "TaskState", c => c.String());
            DropForeignKey("dbo.SoftwareVerifications", "SoftwareUserRequestId", "dbo.SoftwareUserRequests");
            DropForeignKey("dbo.SoftwareUserRequests", "DivisionsId", "dbo.Divisions");
            DropForeignKey("dbo.TechnicianUploads", "FtpId", "dbo.Ftps");
            DropForeignKey("dbo.SoftwareUserUploads", "FtpId", "dbo.Ftps");
            DropForeignKey("dbo.RequestHistories", "SoftwareUserRequestId", "dbo.SoftwareUserRequests");
            DropForeignKey("dbo.RequestHistories", "HardwareUserRequestId", "dbo.HardwareUserRequests");
            DropForeignKey("dbo.ProgrammerUploads", "FtpId", "dbo.Ftps");
            DropForeignKey("dbo.HardwareUserUploads", "FtpId", "dbo.Ftps");
            DropForeignKey("dbo.Departments", "FtpId", "dbo.Ftps");
            DropIndex("dbo.TechnicianUploads", new[] { "FtpId" });
            DropIndex("dbo.SoftwareUserUploads", new[] { "FtpId" });
            DropIndex("dbo.SoftwareVerifications", new[] { "SoftwareUserRequestId" });
            DropIndex("dbo.RequestHistories", new[] { "SoftwareUserRequestId" });
            DropIndex("dbo.RequestHistories", new[] { "HardwareUserRequestId" });
            DropIndex("dbo.ProgrammerUploads", new[] { "FtpId" });
            DropIndex("dbo.SoftwareUserRequests", new[] { "DivisionsId" });
            DropIndex("dbo.HardwareUserUploads", new[] { "FtpId" });
            DropIndex("dbo.Departments", new[] { "FtpId" });
            AlterColumn("dbo.AspNetUsers", "FullName", c => c.String(nullable: false));
            AlterColumn("dbo.UserRegistrations", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.SoftwareVerifications", "SoftwareUserRequestId", c => c.Int(nullable: false));
            AlterColumn("dbo.SoftwareUserRequests", "DivisionsId", c => c.Int(nullable: false));
            AlterColumn("dbo.HardwareUserRequests", "Description", c => c.String());
            AlterColumn("dbo.HardwareUserRequests", "DocumentLabel", c => c.String());
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "MiddleName");
            DropColumn("dbo.AspNetUsers", "FirstName");
            DropColumn("dbo.UserRegistrations", "MiddleName");
            DropColumn("dbo.UserRegistrations", "LastName");
            DropColumn("dbo.UserRegistrations", "FirstName");
            DropColumn("dbo.UserForgotPasswords", "MiddleName");
            DropColumn("dbo.UserForgotPasswords", "LastName");
            DropColumn("dbo.UserForgotPasswords", "FirstName");
            DropColumn("dbo.TechnicianUploads", "FtpId");
            DropColumn("dbo.TechnicianUploads", "FileExtension");
            DropColumn("dbo.SoftwareUserUploads", "FtpId");
            DropColumn("dbo.SoftwareUserUploads", "FileExtension");
            DropColumn("dbo.RequestHistories", "SoftwareUserRequestId");
            DropColumn("dbo.RequestHistories", "HardwareUserRequestId");
            DropColumn("dbo.RequestHistories", "Category");
            DropColumn("dbo.ProgrammerUploads", "FtpId");
            DropColumn("dbo.ProgrammerUploads", "FileExtension");
            DropColumn("dbo.SoftwareTechnicians", "IsActive");
            DropColumn("dbo.SoftwareUserRequests", "LastName");
            DropColumn("dbo.SoftwareUserRequests", "MiddleName");
            DropColumn("dbo.SoftwareUserRequests", "FirstName");
            DropColumn("dbo.LoginActivities", "DivisionName");
            DropColumn("dbo.LoginActivities", "DepartmentName");
            DropColumn("dbo.HardwareUserUploads", "FtpId");
            DropColumn("dbo.HardwareUserUploads", "FileExtension");
            DropColumn("dbo.HardwareTechnicians", "IsActive");
            DropColumn("dbo.HardwareUserRequests", "LastName");
            DropColumn("dbo.HardwareUserRequests", "MiddleName");
            DropColumn("dbo.HardwareUserRequests", "FirstName");
            DropColumn("dbo.FullCalendars", "Name");
            DropColumn("dbo.Departments", "FtpId");
            DropTable("dbo.Ftps");
            CreateIndex("dbo.SoftwareUploads", "SoftwareRequestId");
            CreateIndex("dbo.SoftwareRequestHistories", "DivisionsId");
            CreateIndex("dbo.SoftwareRequestHistories", "DepartmentsId");
            CreateIndex("dbo.SoftwareRequests", "SoftwareTechnicianId");
            CreateIndex("dbo.SoftwareRequests", "StatusId");
            CreateIndex("dbo.SoftwareRequests", "SoftwareId");
            CreateIndex("dbo.SoftwareRequests", "DivisionsId");
            CreateIndex("dbo.SoftwareRequests", "DepartmentsId");
            CreateIndex("dbo.SoftwareRequests", "InformationSystemId");
            CreateIndex("dbo.SoftwareVerifications", "SoftwareUserRequestId");
            CreateIndex("dbo.SoftwareUserRequests", "DivisionsId");
            CreateIndex("dbo.HardwareUploads", "HardwareRequestId");
            CreateIndex("dbo.HardwareTaskLists", "HardwareApprovalId");
            CreateIndex("dbo.HardwareTaskLists", "HardwareVerifyId");
            CreateIndex("dbo.HardwareTaskLists", "TechnicianReportId");
            CreateIndex("dbo.HardwareTaskLists", "HardwareUserRequestId");
            CreateIndex("dbo.HardwareTaskLists", "HardwareTaskId");
            CreateIndex("dbo.HardwareSubTasks", "HardwareTaskId");
            CreateIndex("dbo.HardwareRequestHistories", "DivisionsId");
            CreateIndex("dbo.HardwareRequestHistories", "DepartmentsId");
            CreateIndex("dbo.HardwareRequests", "FindingId");
            CreateIndex("dbo.HardwareRequests", "HardwareTechnicianId");
            CreateIndex("dbo.HardwareRequests", "StatusId");
            CreateIndex("dbo.HardwareRequests", "DivisionsId");
            CreateIndex("dbo.HardwareRequests", "UnitTypeId");
            CreateIndex("dbo.HardwareRequests", "DepartmentsId");
            CreateIndex("dbo.HardwareRequests", "HardwareId");
            CreateIndex("dbo.TechnicianReports", "TechnicianAdminId");
            AddForeignKey("dbo.SoftwareVerifications", "SoftwareUserRequestId", "dbo.SoftwareUserRequests", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SoftwareUserRequests", "DivisionsId", "dbo.Divisions", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SoftwareUploads", "SoftwareRequestId", "dbo.SoftwareRequests", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SoftwareRequestHistories", "DivisionsId", "dbo.Divisions", "Id");
            AddForeignKey("dbo.SoftwareRequestHistories", "DepartmentsId", "dbo.Departments", "Id");
            AddForeignKey("dbo.SoftwareRequests", "StatusId", "dbo.Status", "Id");
            AddForeignKey("dbo.SoftwareRequests", "SoftwareTechnicianId", "dbo.SoftwareTechnicians", "Id");
            AddForeignKey("dbo.SoftwareRequests", "SoftwareId", "dbo.Softwares", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SoftwareRequests", "InformationSystemId", "dbo.InformationSystems", "Id");
            AddForeignKey("dbo.SoftwareRequests", "DivisionsId", "dbo.Divisions", "Id");
            AddForeignKey("dbo.SoftwareRequests", "DepartmentsId", "dbo.Departments", "Id");
            AddForeignKey("dbo.HardwareUploads", "HardwareRequestId", "dbo.HardwareRequests", "Id", cascadeDelete: true);
            AddForeignKey("dbo.HardwareTaskLists", "TechnicianReportId", "dbo.TechnicianReports", "Id");
            AddForeignKey("dbo.HardwareTaskLists", "HardwareVerifyId", "dbo.HardwareVerifies", "Id");
            AddForeignKey("dbo.HardwareTaskLists", "HardwareUserRequestId", "dbo.HardwareUserRequests", "Id");
            AddForeignKey("dbo.HardwareTaskLists", "HardwareTaskId", "dbo.HardwareTasks", "Id", cascadeDelete: true);
            AddForeignKey("dbo.HardwareTaskLists", "HardwareApprovalId", "dbo.HardwareApprovals", "Id");
            AddForeignKey("dbo.HardwareSubTasks", "HardwareTaskId", "dbo.HardwareTasks", "Id", cascadeDelete: true);
            AddForeignKey("dbo.HardwareRequestHistories", "DivisionsId", "dbo.Divisions", "Id");
            AddForeignKey("dbo.HardwareRequestHistories", "DepartmentsId", "dbo.Departments", "Id");
            AddForeignKey("dbo.HardwareRequests", "UnitTypeId", "dbo.UnitTypes", "Id");
            AddForeignKey("dbo.HardwareRequests", "StatusId", "dbo.Status", "Id");
            AddForeignKey("dbo.HardwareRequests", "HardwareTechnicianId", "dbo.HardwareTechnicians", "Id");
            AddForeignKey("dbo.HardwareRequests", "HardwareId", "dbo.Hardwares", "Id", cascadeDelete: true);
            AddForeignKey("dbo.HardwareRequests", "FindingId", "dbo.Findings", "Id");
            AddForeignKey("dbo.HardwareRequests", "DivisionsId", "dbo.Divisions", "Id");
            AddForeignKey("dbo.HardwareRequests", "DepartmentsId", "dbo.Departments", "Id");
            AddForeignKey("dbo.TechnicianReports", "TechnicianAdminId", "dbo.TechnicianAdmins", "Id");
        }
    }
}
