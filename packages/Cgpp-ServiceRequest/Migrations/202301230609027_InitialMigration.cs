namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
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
                "dbo.HardwareRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ticket = c.String(),
                        DateAdded = c.String(),
                        Email = c.String(),
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.DepartmentsId)
                .ForeignKey("dbo.Divisions", t => t.DivisionsId)
                .ForeignKey("dbo.Findings", t => t.FindingId)
                .ForeignKey("dbo.Hardwares", t => t.HardwareId, cascadeDelete: true)
                .ForeignKey("dbo.HardwareTechnicians", t => t.HardwareTechnicianId)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .ForeignKey("dbo.UnitTypes", t => t.UnitTypeId)
                .Index(t => t.HardwareId)
                .Index(t => t.DepartmentsId)
                .Index(t => t.UnitTypeId)
                .Index(t => t.DivisionsId)
                .Index(t => t.StatusId)
                .Index(t => t.HardwareTechnicianId)
                .Index(t => t.FindingId);
            
            CreateTable(
                "dbo.HardwareTechnicians",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        DateAdded = c.String(),
                        Email = c.String(),
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
                "dbo.UnitTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateAdded = c.String(),
                        Name = c.String(nullable: false),
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.DepartmentsId)
                .ForeignKey("dbo.Divisions", t => t.DivisionsId)
                .Index(t => t.DepartmentsId)
                .Index(t => t.DivisionsId);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HardwareRequests", t => t.HardwareRequestId, cascadeDelete: true)
                .Index(t => t.HardwareRequestId);
            
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
                "dbo.Softwares",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        DateAdded = c.String(),
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.DepartmentsId)
                .ForeignKey("dbo.Divisions", t => t.DivisionsId)
                .ForeignKey("dbo.InformationSystems", t => t.InformationSystemId)
                .ForeignKey("dbo.Softwares", t => t.SoftwareId, cascadeDelete: true)
                .ForeignKey("dbo.SoftwareTechnicians", t => t.SoftwareTechnicianId)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .Index(t => t.InformationSystemId)
                .Index(t => t.DepartmentsId)
                .Index(t => t.DivisionsId)
                .Index(t => t.SoftwareId)
                .Index(t => t.StatusId)
                .Index(t => t.SoftwareTechnicianId);
            
            CreateTable(
                "dbo.SoftwareTechnicians",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        TechEmail = c.String(),
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.DepartmentsId)
                .ForeignKey("dbo.Divisions", t => t.DivisionsId)
                .Index(t => t.DepartmentsId)
                .Index(t => t.DivisionsId);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SoftwareRequests", t => t.SoftwareRequestId, cascadeDelete: true)
                .Index(t => t.SoftwareRequestId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(),
                        DateCreated = c.DateTime(),
                        MobileNumber = c.String(),
                        DepartmentsId = c.Int(),
                        ImagePath = c.String(),
                        DivisionsId = c.Int(),
                        IsLogged = c.Boolean(nullable: false),
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
            DropForeignKey("dbo.SoftwareUploads", "SoftwareRequestId", "dbo.SoftwareRequests");
            DropForeignKey("dbo.SoftwareRequestHistories", "DivisionsId", "dbo.Divisions");
            DropForeignKey("dbo.SoftwareRequestHistories", "DepartmentsId", "dbo.Departments");
            DropForeignKey("dbo.SoftwareRequests", "StatusId", "dbo.Status");
            DropForeignKey("dbo.SoftwareRequests", "SoftwareTechnicianId", "dbo.SoftwareTechnicians");
            DropForeignKey("dbo.SoftwareRequests", "SoftwareId", "dbo.Softwares");
            DropForeignKey("dbo.SoftwareRequests", "InformationSystemId", "dbo.InformationSystems");
            DropForeignKey("dbo.SoftwareRequests", "DivisionsId", "dbo.Divisions");
            DropForeignKey("dbo.SoftwareRequests", "DepartmentsId", "dbo.Departments");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.RequestHistories", "DivisionsId", "dbo.Divisions");
            DropForeignKey("dbo.RequestHistories", "DepartmentsId", "dbo.Departments");
            DropForeignKey("dbo.HardwareUploads", "HardwareRequestId", "dbo.HardwareRequests");
            DropForeignKey("dbo.HardwareRequestHistories", "DivisionsId", "dbo.Divisions");
            DropForeignKey("dbo.HardwareRequestHistories", "DepartmentsId", "dbo.Departments");
            DropForeignKey("dbo.HardwareRequests", "UnitTypeId", "dbo.UnitTypes");
            DropForeignKey("dbo.HardwareRequests", "StatusId", "dbo.Status");
            DropForeignKey("dbo.HardwareRequests", "HardwareTechnicianId", "dbo.HardwareTechnicians");
            DropForeignKey("dbo.HardwareRequests", "HardwareId", "dbo.Hardwares");
            DropForeignKey("dbo.HardwareRequests", "FindingId", "dbo.Findings");
            DropForeignKey("dbo.HardwareRequests", "DivisionsId", "dbo.Divisions");
            DropForeignKey("dbo.HardwareRequests", "DepartmentsId", "dbo.Departments");
            DropForeignKey("dbo.Divisions", "DepartmentsId", "dbo.Departments");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "DivisionsId" });
            DropIndex("dbo.AspNetUsers", new[] { "DepartmentsId" });
            DropIndex("dbo.SoftwareUploads", new[] { "SoftwareRequestId" });
            DropIndex("dbo.SoftwareRequestHistories", new[] { "DivisionsId" });
            DropIndex("dbo.SoftwareRequestHistories", new[] { "DepartmentsId" });
            DropIndex("dbo.SoftwareRequests", new[] { "SoftwareTechnicianId" });
            DropIndex("dbo.SoftwareRequests", new[] { "StatusId" });
            DropIndex("dbo.SoftwareRequests", new[] { "SoftwareId" });
            DropIndex("dbo.SoftwareRequests", new[] { "DivisionsId" });
            DropIndex("dbo.SoftwareRequests", new[] { "DepartmentsId" });
            DropIndex("dbo.SoftwareRequests", new[] { "InformationSystemId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.RequestHistories", new[] { "DivisionsId" });
            DropIndex("dbo.RequestHistories", new[] { "DepartmentsId" });
            DropIndex("dbo.HardwareUploads", new[] { "HardwareRequestId" });
            DropIndex("dbo.HardwareRequestHistories", new[] { "DivisionsId" });
            DropIndex("dbo.HardwareRequestHistories", new[] { "DepartmentsId" });
            DropIndex("dbo.HardwareRequests", new[] { "FindingId" });
            DropIndex("dbo.HardwareRequests", new[] { "HardwareTechnicianId" });
            DropIndex("dbo.HardwareRequests", new[] { "StatusId" });
            DropIndex("dbo.HardwareRequests", new[] { "DivisionsId" });
            DropIndex("dbo.HardwareRequests", new[] { "UnitTypeId" });
            DropIndex("dbo.HardwareRequests", new[] { "DepartmentsId" });
            DropIndex("dbo.HardwareRequests", new[] { "HardwareId" });
            DropIndex("dbo.Divisions", new[] { "DepartmentsId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.SoftwareUploads");
            DropTable("dbo.SoftwareRequestHistories");
            DropTable("dbo.SoftwareTechnicians");
            DropTable("dbo.SoftwareRequests");
            DropTable("dbo.Softwares");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.RequestHistories");
            DropTable("dbo.MaintenanceModes");
            DropTable("dbo.LoginActivities");
            DropTable("dbo.InformationSystems");
            DropTable("dbo.HardwareUploads");
            DropTable("dbo.HardwareRequestHistories");
            DropTable("dbo.UnitTypes");
            DropTable("dbo.Status");
            DropTable("dbo.HardwareTechnicians");
            DropTable("dbo.HardwareRequests");
            DropTable("dbo.Hardwares");
            DropTable("dbo.FullCalendars");
            DropTable("dbo.FtpModels");
            DropTable("dbo.Findings");
            DropTable("dbo.Divisions");
            DropTable("dbo.Departments");
        }
    }
}
