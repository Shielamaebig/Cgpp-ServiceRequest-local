namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Removehardwarehistory : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HardwareRequestHistories", "DepartmentsId", "dbo.Departments");
            DropForeignKey("dbo.HardwareRequestHistories", "DivisionsId", "dbo.Divisions");
            DropIndex("dbo.HardwareRequestHistories", new[] { "DepartmentsId" });
            DropIndex("dbo.HardwareRequestHistories", new[] { "DivisionsId" });
            AlterColumn("dbo.AspNetUsers", "FullName", c => c.String());
            AlterColumn("dbo.AspNetUsers", "FirstName", c => c.String(nullable: false));
            AlterColumn("dbo.AspNetUsers", "MiddleName", c => c.String(nullable: false));
            AlterColumn("dbo.AspNetUsers", "LastName", c => c.String(nullable: false));
            DropTable("dbo.HardwareRequestHistories");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.AspNetUsers", "LastName", c => c.String());
            AlterColumn("dbo.AspNetUsers", "MiddleName", c => c.String());
            AlterColumn("dbo.AspNetUsers", "FirstName", c => c.String());
            AlterColumn("dbo.AspNetUsers", "FullName", c => c.String(nullable: false));
            CreateIndex("dbo.HardwareRequestHistories", "DivisionsId");
            CreateIndex("dbo.HardwareRequestHistories", "DepartmentsId");
            AddForeignKey("dbo.HardwareRequestHistories", "DivisionsId", "dbo.Divisions", "Id");
            AddForeignKey("dbo.HardwareRequestHistories", "DepartmentsId", "dbo.Departments", "Id");
        }
    }
}
