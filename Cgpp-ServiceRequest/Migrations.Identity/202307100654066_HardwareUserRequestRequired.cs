namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HardwareUserRequestRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.HardwareUserRequests", "DocumentLabel", c => c.String(nullable: false));
            AlterColumn("dbo.HardwareUserRequests", "Description", c => c.String(nullable: false));
            DropColumn("dbo.HardwareUserRequests", "TaskState");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HardwareUserRequests", "TaskState", c => c.String());
            AlterColumn("dbo.HardwareUserRequests", "Description", c => c.String());
            AlterColumn("dbo.HardwareUserRequests", "DocumentLabel", c => c.String());
        }
    }
}
