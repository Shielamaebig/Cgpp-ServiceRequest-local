namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemorveTaskstate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HardwareTaskLists", "HardwareUserRequestId", "dbo.HardwareUserRequests");
            DropIndex("dbo.HardwareTaskLists", new[] { "HardwareUserRequestId" });
            AlterColumn("dbo.HardwareTaskLists", "HardwareUserRequestId", c => c.Int());
            CreateIndex("dbo.HardwareTaskLists", "HardwareUserRequestId");
            AddForeignKey("dbo.HardwareTaskLists", "HardwareUserRequestId", "dbo.HardwareUserRequests", "Id");
            DropColumn("dbo.HardwareTaskLists", "State");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HardwareTaskLists", "State", c => c.String());
            DropForeignKey("dbo.HardwareTaskLists", "HardwareUserRequestId", "dbo.HardwareUserRequests");
            DropIndex("dbo.HardwareTaskLists", new[] { "HardwareUserRequestId" });
            AlterColumn("dbo.HardwareTaskLists", "HardwareUserRequestId", c => c.Int(nullable: false));
            CreateIndex("dbo.HardwareTaskLists", "HardwareUserRequestId");
            AddForeignKey("dbo.HardwareTaskLists", "HardwareUserRequestId", "dbo.HardwareUserRequests", "Id", cascadeDelete: true);
        }
    }
}
