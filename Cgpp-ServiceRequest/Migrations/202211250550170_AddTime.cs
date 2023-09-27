namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HardwareRequests", "TimeStarted", c => c.String());
            AddColumn("dbo.HardwareRequests", "TimeEnded", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HardwareRequests", "TimeEnded");
            DropColumn("dbo.HardwareRequests", "TimeStarted");
        }
    }
}
