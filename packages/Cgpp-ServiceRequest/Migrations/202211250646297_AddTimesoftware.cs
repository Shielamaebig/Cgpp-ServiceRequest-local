namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTimesoftware : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SoftwareRequests", "TimeStarted", c => c.String());
            AddColumn("dbo.SoftwareRequests", "TimeEnded", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SoftwareRequests", "TimeEnded");
            DropColumn("dbo.SoftwareRequests", "TimeStarted");
        }
    }
}
