namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSnAndCn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HardwareRequests", "SerialNumber", c => c.String());
            AddColumn("dbo.HardwareRequests", "ControlNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HardwareRequests", "ControlNumber");
            DropColumn("dbo.HardwareRequests", "SerialNumber");
        }
    }
}
