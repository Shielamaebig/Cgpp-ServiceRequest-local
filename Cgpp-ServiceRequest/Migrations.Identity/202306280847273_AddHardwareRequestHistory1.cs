namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHardwareRequestHistory1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String());
            AddColumn("dbo.AspNetUsers", "MiddleName", c => c.String());
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String());
            CreateIndex("dbo.SoftwareTaskLists", "SoftwareTaskId");
            AddForeignKey("dbo.SoftwareTaskLists", "SoftwareTaskId", "dbo.SoftwareTasks", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SoftwareTaskLists", "SoftwareTaskId", "dbo.SoftwareTasks");
            DropIndex("dbo.SoftwareTaskLists", new[] { "SoftwareTaskId" });
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "MiddleName");
            DropColumn("dbo.AspNetUsers", "FirstName");
        }
    }
}
