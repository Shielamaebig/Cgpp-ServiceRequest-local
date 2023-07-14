namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHardwareRequestHistory : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HardwareRequestHistories", "DivisionsId", "dbo.Divisions");
            DropForeignKey("dbo.HardwareRequestHistories", "DepartmentsId", "dbo.Departments");
            DropIndex("dbo.HardwareRequestHistories", new[] { "DivisionsId" });
            DropIndex("dbo.HardwareRequestHistories", new[] { "DepartmentsId" });
            DropTable("dbo.HardwareRequestHistories");
        }
    }
}
