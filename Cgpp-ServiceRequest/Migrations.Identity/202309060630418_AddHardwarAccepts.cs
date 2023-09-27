namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHardwarAccepts : DbMigration
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
            
            AddColumn("dbo.HardwareTasksLists", "HardwareAcceptsRequestId", c => c.Int());
            CreateIndex("dbo.HardwareTasksLists", "HardwareAcceptsRequestId");
            AddForeignKey("dbo.HardwareTasksLists", "HardwareAcceptsRequestId", "dbo.HardwareAcceptsRequests", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HardwareTasksLists", "HardwareAcceptsRequestId", "dbo.HardwareAcceptsRequests");
            DropForeignKey("dbo.HardwareAcceptsRequests", "HardwareUserRequestId", "dbo.HardwareUserRequests");
            DropForeignKey("dbo.HardwareAcceptsRequests", "DivisionsId", "dbo.Divisions");
            DropForeignKey("dbo.HardwareAcceptsRequests", "DepartmentsId", "dbo.Departments");
            DropIndex("dbo.HardwareAcceptsRequests", new[] { "DivisionsId" });
            DropIndex("dbo.HardwareAcceptsRequests", new[] { "DepartmentsId" });
            DropIndex("dbo.HardwareAcceptsRequests", new[] { "HardwareUserRequestId" });
            DropIndex("dbo.HardwareTasksLists", new[] { "HardwareAcceptsRequestId" });
            DropColumn("dbo.HardwareTasksLists", "HardwareAcceptsRequestId");
            DropTable("dbo.HardwareAcceptsRequests");
        }
    }
}
