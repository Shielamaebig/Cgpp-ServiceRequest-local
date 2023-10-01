namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddtechInHardwareRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HardwareRequests", "TechEmail", c => c.String());
            AddColumn("dbo.HardwareRequests", "IsReported", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HardwareRequests", "IsReported");
            DropColumn("dbo.HardwareRequests", "TechEmail");
        }
    }
}
