namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigrations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        DateAdded = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Divisions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        DateAdded = c.String(),
                        DepartmentsId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.DepartmentsId, cascadeDelete: true)
                .Index(t => t.DepartmentsId);
            
            CreateTable(
                "dbo.Findings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FullCalendars",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Subject = c.String(),
                        Description = c.String(),
                        Start = c.String(),
                        End = c.String(),
                        ThemeColor = c.String(),
                        IsFullDay = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Hardwares",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        DateAdded = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HardwareApprovals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NameApproval = c.String(),
                        EmailApproval = c.String(),
                        RemarksApproval = c.String(),
                        DateApprove = c.String(),
                        HardwareUserRequestId = c.Int(),
                        TechnicianReportId = c.Int(),
                        HardwareVerifyId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HardwareUserRequests", t => t.HardwareUserRequestId)
                .ForeignKey("dbo.HardwareVerifies", t => t.HardwareVerifyId)
                .ForeignKey("dbo.TechnicianReports", t => t.TechnicianReportId)
                .Index(t => t.HardwareUserRequestId)
                .Index(t => t.TechnicianReportId)
                .Index(t => t.HardwareVerifyId);
            
            CreateTable(
                "dbo.HardwareUserRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ticket = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateAdded = c.String(),
                        FullName = c.String(),
                        Email = c.String(),
                        DepartmentsId = c.Int(nullable: false),
                        DepartmentName = c.String(),
                        DivisionsId = c.Int(),
                        DivisionName = c.String(),
                        MobileNumber = c.String(),
                        HardwareId = c.Int(nullable: false),
                        HardwareName = c.String(),
                        UnitTypeId = c.Int(nullable: false),
                        UniTypes = c.String(),
                        BrandName = c.String(),
                        ModelName = c.String(),
                        DocumentLabel = c.String(),
                        Description = c.String(),
                        TaskState = c.String(),
                        IsNew = c.Boolean(nullable: false),
                        Status = c.String(),
                        IsManual = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.DepartmentsId, cascadeDelete: true)
                .ForeignKey("dbo.Divisions", t => t.DivisionsId)
                .ForeignKey("dbo.Hardwares", t => t.HardwareId, cascadeDelete: true)
                .ForeignKey("dbo.UnitTypes", t => t.UnitTypeId, cascadeDelete: true)
                .Index(t => t.DepartmentsId)
                .Index(t => t.DivisionsId)
                .Index(t => t.HardwareId)
                .Index(t => t.UnitTypeId);
            
            CreateTable(
                "dbo.UnitTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateAdded = c.String(),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HardwareVerifies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateVerified = c.String(),
                        Name = c.String(),
                        Email = c.String(),
                        HardwareUserRequestId = c.Int(nullable: false),
                        TechnicianReportId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HardwareUserRequests", t => t.HardwareUserRequestId, cascadeDelete: true)
                .ForeignKey("dbo.TechnicianReports", t => t.TechnicianReportId)
                .Index(t => t.HardwareUserRequestId)
                .Index(t => t.TechnicianReportId);
            
            CreateTable(
                "dbo.TechnicianReports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        TechnicianName = c.String(),
                        TechEmail = c.String(),
                        DateAssigned = c.String(),
                        HardwareUserRequestId = c.Int(),
                        HardwareId = c.Int(),
                        HardwareName = c.String(),
                        UnitTypeId = c.Int(),
                        UniTypes = c.String(),
                        BrandName = c.String(),
                        ModelName = c.String(),
                        Status = c.String(),
                        DateStarted = c.String(),
                        DateEnded = c.String(),
                        HardwareTechnicianId = c.Int(nullable: false),
                        Remarks = c.String(),
                        FindingId = c.Int(),
                        FindingName = c.String(),
                        PossibleCause = c.String(),
                        SerialNumber = c.String(),
                        ControlNumber = c.String(),
                        TechnicianAdminId = c.Int(),
                        AdminName = c.String(),
                        SuperAdminId = c.Int(),
                        SuperName = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Findings", t => t.FindingId)
                .ForeignKey("dbo.Hardwares", t => t.HardwareId)
                .ForeignKey("dbo.HardwareTechnicians", t => t.HardwareTechnicianId, cascadeDelete: true)
                .ForeignKey("dbo.HardwareUserRequests", t => t.HardwareUserRequestId)
                .ForeignKey("dbo.SuperAdmins", t => t.SuperAdminId)
                .ForeignKey("dbo.TechnicianAdmins", t => t.TechnicianAdminId)
                .ForeignKey("dbo.UnitTypes", t => t.UnitTypeId)
                .Index(t => t.HardwareUserRequestId)
                .Index(t => t.HardwareId)
                .Index(t => t.UnitTypeId)
                .Index(t => t.HardwareTechnicianId)
                .Index(t => t.FindingId)
                .Index(t => t.TechnicianAdminId)
                .Index(t => t.SuperAdminId);
            
            CreateTable(
                "dbo.HardwareTechnicians",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        DateAdded = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SuperAdmins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Name = c.String(),
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
                "dbo.HardwareTasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
            
            CreateTable(
                "dbo.HardwareUserUploads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImagePath = c.String(),
                        FileName = c.String(),
                        HardwareUserRequestId = c.Int(nullable: false),
                        DateAdded = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HardwareUserRequests", t => t.HardwareUserRequestId, cascadeDelete: true)
                .Index(t => t.HardwareUserRequestId);
            
            CreateTable(
                "dbo.InformationSystems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DateAdded = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LoginActivities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Email = c.String(),
                        ActivityMessage = c.String(),
                        ActivityDate = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MaintenanceModes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Label = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        Message = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProgrammerReports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProgrammerName = c.String(),
                        ProgrammerEmail = c.String(),
                        SoftwareTechnicianId = c.Int(nullable: false),
                        SoftwareTechnicianName = c.String(),
                        DateAdded = c.String(),
                        DateAssigned = c.String(),
                        DateEnded = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        SoftwareId = c.Int(),
                        SoftwareName = c.String(),
                        InformationSystemId = c.Int(),
                        InformationName = c.String(),
                        RequestFor = c.String(),
                        Description = c.String(),
                        DocumentLabel = c.String(),
                        Remarks = c.String(),
                        AdminName = c.String(),
                        SuperAdminName = c.String(),
                        SoftwareUserRequestId = c.Int(),
                        Resulotion = c.String(),
                        ProgressStatus = c.String(),
                        ProgressRemarks = c.String(),
                        DateVerified = c.String(),
                        DateApproved = c.String(),
                        SoftwareAcceptsRequestId = c.Int(),
                        RemarksApproval = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InformationSystems", t => t.InformationSystemId)
                .ForeignKey("dbo.Softwares", t => t.SoftwareId)
                .ForeignKey("dbo.SoftwareAcceptsRequests", t => t.SoftwareAcceptsRequestId)
                .ForeignKey("dbo.SoftwareTechnicians", t => t.SoftwareTechnicianId, cascadeDelete: true)
                .ForeignKey("dbo.SoftwareUserRequests", t => t.SoftwareUserRequestId)
                .Index(t => t.SoftwareTechnicianId)
                .Index(t => t.SoftwareId)
                .Index(t => t.InformationSystemId)
                .Index(t => t.SoftwareUserRequestId)
                .Index(t => t.SoftwareAcceptsRequestId);
            
            CreateTable(
                "dbo.Softwares",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        DateAdded = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SoftwareAcceptsRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateAdded = c.String(),
                        SoftwareUserRequestId = c.Int(nullable: false),
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
                .ForeignKey("dbo.SoftwareUserRequests", t => t.SoftwareUserRequestId, cascadeDelete: false)
                .Index(t => t.SoftwareUserRequestId)
                .Index(t => t.DepartmentsId)
                .Index(t => t.DivisionsId);
            
            CreateTable(
                "dbo.SoftwareUserRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        DateAdded = c.String(),
                        FullName = c.String(),
                        MobileNumber = c.String(),
                        Email = c.String(),
                        DepartmentsId = c.Int(nullable: false),
                        DepartmentName = c.String(),
                        DivisionsId = c.Int(nullable: false),
                        DivisionName = c.String(),
                        Status = c.String(),
                        IsNew = c.Boolean(nullable: false),
                        IsManual = c.Boolean(nullable: false),
                        SoftwareId = c.Int(nullable: false),
                        SoftwareName = c.String(),
                        Ticket = c.String(),
                        RequestFor = c.String(),
                        Description = c.String(),
                        DocumentLabel = c.String(),
                        InformationSystemId = c.Int(nullable: false),
                        InformationName = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.DepartmentsId, cascadeDelete: false)
                .ForeignKey("dbo.Divisions", t => t.DivisionsId, cascadeDelete: false)
                .ForeignKey("dbo.InformationSystems", t => t.InformationSystemId, cascadeDelete: false)
                .ForeignKey("dbo.Softwares", t => t.SoftwareId, cascadeDelete: false)
                .Index(t => t.DepartmentsId)
                .Index(t => t.DivisionsId)
                .Index(t => t.SoftwareId)
                .Index(t => t.InformationSystemId);
            
            CreateTable(
                "dbo.SoftwareTechnicians",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        TechEmail = c.String(),
                        DateAdded = c.String(),
                        PhoneNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProgrammerUploads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImagePath = c.String(),
                        FileName = c.String(),
                        ProgrammerReportId = c.Int(nullable: false),
                        DateAdded = c.String(),
                        RemarksUploads = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProgrammerReports", t => t.ProgrammerReportId, cascadeDelete: true)
                .Index(t => t.ProgrammerReportId);
            
            CreateTable(
                "dbo.RequestHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Email = c.String(),
                        DepartmentsId = c.Int(),
                        DivisionsId = c.Int(),
                        UploadMessage = c.String(),
                        UploadDate = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.DepartmentsId)
                .ForeignKey("dbo.Divisions", t => t.DivisionsId)
                .Index(t => t.DepartmentsId)
                .Index(t => t.DivisionsId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.SoftwareApproveds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NameApproval = c.String(),
                        EmailApproval = c.String(),
                        RemarksApproval = c.String(),
                        DateApprove = c.String(),
                        SoftwareUserRequestId = c.Int(),
                        ProgrammerReportId = c.Int(),
                        SoftwareVerificationId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProgrammerReports", t => t.ProgrammerReportId)
                .ForeignKey("dbo.SoftwareUserRequests", t => t.SoftwareUserRequestId)
                .ForeignKey("dbo.SoftwareVerifications", t => t.SoftwareVerificationId)
                .Index(t => t.SoftwareUserRequestId)
                .Index(t => t.ProgrammerReportId)
                .Index(t => t.SoftwareVerificationId);
            
            CreateTable(
                "dbo.SoftwareVerifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateVerified = c.String(),
                        NameVerified = c.String(),
                        EmailVerified = c.String(),
                        SoftwareUserRequestId = c.Int(nullable: false),
                        ProgrammerReportId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProgrammerReports", t => t.ProgrammerReportId, cascadeDelete: true)
                .ForeignKey("dbo.SoftwareUserRequests", t => t.SoftwareUserRequestId, cascadeDelete: true)
                .Index(t => t.SoftwareUserRequestId)
                .Index(t => t.ProgrammerReportId);
            
            CreateTable(
                "dbo.SoftwareTaskLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SoftwareTaskId = c.Int(nullable: false),
                        DateAdded = c.String(),
                        State = c.String(),
                        SoftwareUserRequestId = c.Int(),
                        SoftwareAcceptsRequestId = c.Int(),
                        ProgrammerReportId = c.Int(),
                        SoftwareVerificationId = c.Int(),
                        SoftwareApprovedId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProgrammerReports", t => t.ProgrammerReportId)
                .ForeignKey("dbo.SoftwareAcceptsRequests", t => t.SoftwareAcceptsRequestId)
                .ForeignKey("dbo.SoftwareApproveds", t => t.SoftwareApprovedId)
                .ForeignKey("dbo.SoftwareUserRequests", t => t.SoftwareUserRequestId)
                .ForeignKey("dbo.SoftwareVerifications", t => t.SoftwareVerificationId)
                .Index(t => t.SoftwareUserRequestId)
                .Index(t => t.SoftwareAcceptsRequestId)
                .Index(t => t.ProgrammerReportId)
                .Index(t => t.SoftwareVerificationId)
                .Index(t => t.SoftwareApprovedId);
            
            CreateTable(
                "dbo.SoftwareTasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SoftwareUserUploads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImagePath = c.String(),
                        FileName = c.String(),
                        SoftwareUserRequestId = c.Int(nullable: false),
                        DateAdded = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SoftwareUserRequests", t => t.SoftwareUserRequestId, cascadeDelete: true)
                .Index(t => t.SoftwareUserRequestId);
            
            CreateTable(
                "dbo.TechnicianUploads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImagePath = c.String(),
                        FileName = c.String(),
                        TechnicianReportId = c.Int(nullable: false),
                        DateAdded = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TechnicianReports", t => t.TechnicianReportId, cascadeDelete: true)
                .Index(t => t.TechnicianReportId);
            
            CreateTable(
                "dbo.UserForgotPasswords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        DepartmentName = c.String(nullable: false),
                        DivisionName = c.String(nullable: false),
                        MobileNumber = c.String(nullable: false),
                        IsRequest = c.Boolean(nullable: false),
                        DateCreated = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRegistrations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        DepartmentName = c.String(nullable: false),
                        DivisionName = c.String(nullable: false),
                        MobileNumber = c.String(nullable: false),
                        IsNewAccount = c.Boolean(nullable: false),
                        DateCreated = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(nullable: false),
                        DateCreated = c.DateTime(),
                        MobileNumber = c.String(nullable: false),
                        DepartmentsId = c.Int(),
                        ImagePath = c.String(),
                        DivisionsId = c.Int(),
                        DepartmentName = c.String(),
                        DivisionName = c.String(),
                        IsLogged = c.Boolean(nullable: false),
                        IsUserApproved = c.Boolean(nullable: false),
                        LogEmail = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.DepartmentsId)
                .ForeignKey("dbo.Divisions", t => t.DivisionsId)
                .Index(t => t.DepartmentsId)
                .Index(t => t.DivisionsId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "DivisionsId", "dbo.Divisions");
            DropForeignKey("dbo.AspNetUsers", "DepartmentsId", "dbo.Departments");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.TechnicianUploads", "TechnicianReportId", "dbo.TechnicianReports");
            DropForeignKey("dbo.SoftwareUserUploads", "SoftwareUserRequestId", "dbo.SoftwareUserRequests");
            DropForeignKey("dbo.SoftwareTaskLists", "SoftwareVerificationId", "dbo.SoftwareVerifications");
            DropForeignKey("dbo.SoftwareTaskLists", "SoftwareUserRequestId", "dbo.SoftwareUserRequests");
            DropForeignKey("dbo.SoftwareTaskLists", "SoftwareApprovedId", "dbo.SoftwareApproveds");
            DropForeignKey("dbo.SoftwareTaskLists", "SoftwareAcceptsRequestId", "dbo.SoftwareAcceptsRequests");
            DropForeignKey("dbo.SoftwareTaskLists", "ProgrammerReportId", "dbo.ProgrammerReports");
            DropForeignKey("dbo.SoftwareApproveds", "SoftwareVerificationId", "dbo.SoftwareVerifications");
            DropForeignKey("dbo.SoftwareVerifications", "SoftwareUserRequestId", "dbo.SoftwareUserRequests");
            DropForeignKey("dbo.SoftwareVerifications", "ProgrammerReportId", "dbo.ProgrammerReports");
            DropForeignKey("dbo.SoftwareApproveds", "SoftwareUserRequestId", "dbo.SoftwareUserRequests");
            DropForeignKey("dbo.SoftwareApproveds", "ProgrammerReportId", "dbo.ProgrammerReports");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.RequestHistories", "DivisionsId", "dbo.Divisions");
            DropForeignKey("dbo.RequestHistories", "DepartmentsId", "dbo.Departments");
            DropForeignKey("dbo.ProgrammerUploads", "ProgrammerReportId", "dbo.ProgrammerReports");
            DropForeignKey("dbo.ProgrammerReports", "SoftwareUserRequestId", "dbo.SoftwareUserRequests");
            DropForeignKey("dbo.ProgrammerReports", "SoftwareTechnicianId", "dbo.SoftwareTechnicians");
            DropForeignKey("dbo.ProgrammerReports", "SoftwareAcceptsRequestId", "dbo.SoftwareAcceptsRequests");
            DropForeignKey("dbo.SoftwareAcceptsRequests", "SoftwareUserRequestId", "dbo.SoftwareUserRequests");
            DropForeignKey("dbo.SoftwareUserRequests", "SoftwareId", "dbo.Softwares");
            DropForeignKey("dbo.SoftwareUserRequests", "InformationSystemId", "dbo.InformationSystems");
            DropForeignKey("dbo.SoftwareUserRequests", "DivisionsId", "dbo.Divisions");
            DropForeignKey("dbo.SoftwareUserRequests", "DepartmentsId", "dbo.Departments");
            DropForeignKey("dbo.SoftwareAcceptsRequests", "DivisionsId", "dbo.Divisions");
            DropForeignKey("dbo.SoftwareAcceptsRequests", "DepartmentsId", "dbo.Departments");
            DropForeignKey("dbo.ProgrammerReports", "SoftwareId", "dbo.Softwares");
            DropForeignKey("dbo.ProgrammerReports", "InformationSystemId", "dbo.InformationSystems");
            DropForeignKey("dbo.HardwareUserUploads", "HardwareUserRequestId", "dbo.HardwareUserRequests");
            DropForeignKey("dbo.HardwareTasksLists", "TechnicianReportId", "dbo.TechnicianReports");
            DropForeignKey("dbo.HardwareTasksLists", "HardwareVerifyId", "dbo.HardwareVerifies");
            DropForeignKey("dbo.HardwareTasksLists", "HardwareUserRequestId", "dbo.HardwareUserRequests");
            DropForeignKey("dbo.HardwareTasksLists", "HardwareTaskId", "dbo.HardwareTasks");
            DropForeignKey("dbo.HardwareTasksLists", "HardwareApprovalId", "dbo.HardwareApprovals");
            DropForeignKey("dbo.HardwareApprovals", "TechnicianReportId", "dbo.TechnicianReports");
            DropForeignKey("dbo.HardwareApprovals", "HardwareVerifyId", "dbo.HardwareVerifies");
            DropForeignKey("dbo.HardwareVerifies", "TechnicianReportId", "dbo.TechnicianReports");
            DropForeignKey("dbo.TechnicianReports", "UnitTypeId", "dbo.UnitTypes");
            DropForeignKey("dbo.TechnicianReports", "TechnicianAdminId", "dbo.TechnicianAdmins");
            DropForeignKey("dbo.TechnicianReports", "SuperAdminId", "dbo.SuperAdmins");
            DropForeignKey("dbo.TechnicianReports", "HardwareUserRequestId", "dbo.HardwareUserRequests");
            DropForeignKey("dbo.TechnicianReports", "HardwareTechnicianId", "dbo.HardwareTechnicians");
            DropForeignKey("dbo.TechnicianReports", "HardwareId", "dbo.Hardwares");
            DropForeignKey("dbo.TechnicianReports", "FindingId", "dbo.Findings");
            DropForeignKey("dbo.HardwareVerifies", "HardwareUserRequestId", "dbo.HardwareUserRequests");
            DropForeignKey("dbo.HardwareApprovals", "HardwareUserRequestId", "dbo.HardwareUserRequests");
            DropForeignKey("dbo.HardwareUserRequests", "UnitTypeId", "dbo.UnitTypes");
            DropForeignKey("dbo.HardwareUserRequests", "HardwareId", "dbo.Hardwares");
            DropForeignKey("dbo.HardwareUserRequests", "DivisionsId", "dbo.Divisions");
            DropForeignKey("dbo.HardwareUserRequests", "DepartmentsId", "dbo.Departments");
            DropForeignKey("dbo.Divisions", "DepartmentsId", "dbo.Departments");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "DivisionsId" });
            DropIndex("dbo.AspNetUsers", new[] { "DepartmentsId" });
            DropIndex("dbo.TechnicianUploads", new[] { "TechnicianReportId" });
            DropIndex("dbo.SoftwareUserUploads", new[] { "SoftwareUserRequestId" });
            DropIndex("dbo.SoftwareTaskLists", new[] { "SoftwareApprovedId" });
            DropIndex("dbo.SoftwareTaskLists", new[] { "SoftwareVerificationId" });
            DropIndex("dbo.SoftwareTaskLists", new[] { "ProgrammerReportId" });
            DropIndex("dbo.SoftwareTaskLists", new[] { "SoftwareAcceptsRequestId" });
            DropIndex("dbo.SoftwareTaskLists", new[] { "SoftwareUserRequestId" });
            DropIndex("dbo.SoftwareVerifications", new[] { "ProgrammerReportId" });
            DropIndex("dbo.SoftwareVerifications", new[] { "SoftwareUserRequestId" });
            DropIndex("dbo.SoftwareApproveds", new[] { "SoftwareVerificationId" });
            DropIndex("dbo.SoftwareApproveds", new[] { "ProgrammerReportId" });
            DropIndex("dbo.SoftwareApproveds", new[] { "SoftwareUserRequestId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.RequestHistories", new[] { "DivisionsId" });
            DropIndex("dbo.RequestHistories", new[] { "DepartmentsId" });
            DropIndex("dbo.ProgrammerUploads", new[] { "ProgrammerReportId" });
            DropIndex("dbo.SoftwareUserRequests", new[] { "InformationSystemId" });
            DropIndex("dbo.SoftwareUserRequests", new[] { "SoftwareId" });
            DropIndex("dbo.SoftwareUserRequests", new[] { "DivisionsId" });
            DropIndex("dbo.SoftwareUserRequests", new[] { "DepartmentsId" });
            DropIndex("dbo.SoftwareAcceptsRequests", new[] { "DivisionsId" });
            DropIndex("dbo.SoftwareAcceptsRequests", new[] { "DepartmentsId" });
            DropIndex("dbo.SoftwareAcceptsRequests", new[] { "SoftwareUserRequestId" });
            DropIndex("dbo.ProgrammerReports", new[] { "SoftwareAcceptsRequestId" });
            DropIndex("dbo.ProgrammerReports", new[] { "SoftwareUserRequestId" });
            DropIndex("dbo.ProgrammerReports", new[] { "InformationSystemId" });
            DropIndex("dbo.ProgrammerReports", new[] { "SoftwareId" });
            DropIndex("dbo.ProgrammerReports", new[] { "SoftwareTechnicianId" });
            DropIndex("dbo.HardwareUserUploads", new[] { "HardwareUserRequestId" });
            DropIndex("dbo.HardwareTasksLists", new[] { "HardwareApprovalId" });
            DropIndex("dbo.HardwareTasksLists", new[] { "HardwareVerifyId" });
            DropIndex("dbo.HardwareTasksLists", new[] { "TechnicianReportId" });
            DropIndex("dbo.HardwareTasksLists", new[] { "HardwareUserRequestId" });
            DropIndex("dbo.HardwareTasksLists", new[] { "HardwareTaskId" });
            DropIndex("dbo.TechnicianReports", new[] { "SuperAdminId" });
            DropIndex("dbo.TechnicianReports", new[] { "TechnicianAdminId" });
            DropIndex("dbo.TechnicianReports", new[] { "FindingId" });
            DropIndex("dbo.TechnicianReports", new[] { "HardwareTechnicianId" });
            DropIndex("dbo.TechnicianReports", new[] { "UnitTypeId" });
            DropIndex("dbo.TechnicianReports", new[] { "HardwareId" });
            DropIndex("dbo.TechnicianReports", new[] { "HardwareUserRequestId" });
            DropIndex("dbo.HardwareVerifies", new[] { "TechnicianReportId" });
            DropIndex("dbo.HardwareVerifies", new[] { "HardwareUserRequestId" });
            DropIndex("dbo.HardwareUserRequests", new[] { "UnitTypeId" });
            DropIndex("dbo.HardwareUserRequests", new[] { "HardwareId" });
            DropIndex("dbo.HardwareUserRequests", new[] { "DivisionsId" });
            DropIndex("dbo.HardwareUserRequests", new[] { "DepartmentsId" });
            DropIndex("dbo.HardwareApprovals", new[] { "HardwareVerifyId" });
            DropIndex("dbo.HardwareApprovals", new[] { "TechnicianReportId" });
            DropIndex("dbo.HardwareApprovals", new[] { "HardwareUserRequestId" });
            DropIndex("dbo.Divisions", new[] { "DepartmentsId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.UserRegistrations");
            DropTable("dbo.UserForgotPasswords");
            DropTable("dbo.TechnicianUploads");
            DropTable("dbo.SoftwareUserUploads");
            DropTable("dbo.SoftwareTasks");
            DropTable("dbo.SoftwareTaskLists");
            DropTable("dbo.SoftwareVerifications");
            DropTable("dbo.SoftwareApproveds");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.RequestHistories");
            DropTable("dbo.ProgrammerUploads");
            DropTable("dbo.SoftwareTechnicians");
            DropTable("dbo.SoftwareUserRequests");
            DropTable("dbo.SoftwareAcceptsRequests");
            DropTable("dbo.Softwares");
            DropTable("dbo.ProgrammerReports");
            DropTable("dbo.MaintenanceModes");
            DropTable("dbo.LoginActivities");
            DropTable("dbo.InformationSystems");
            DropTable("dbo.HardwareUserUploads");
            DropTable("dbo.HardwareTasksLists");
            DropTable("dbo.HardwareTasks");
            DropTable("dbo.TechnicianAdmins");
            DropTable("dbo.SuperAdmins");
            DropTable("dbo.HardwareTechnicians");
            DropTable("dbo.TechnicianReports");
            DropTable("dbo.HardwareVerifies");
            DropTable("dbo.UnitTypes");
            DropTable("dbo.HardwareUserRequests");
            DropTable("dbo.HardwareApprovals");
            DropTable("dbo.Hardwares");
            DropTable("dbo.FullCalendars");
            DropTable("dbo.Findings");
            DropTable("dbo.Divisions");
            DropTable("dbo.Departments");
        }
    }
}
