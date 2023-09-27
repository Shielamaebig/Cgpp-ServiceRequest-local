namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsAssigned : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HardwareRequests", "IsAssigned", c => c.Boolean(nullable: false));
            AddColumn("dbo.SoftwareRequests", "IsAssigned", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SoftwareRequests", "IsAssigned");
            DropColumn("dbo.HardwareRequests", "IsAssigned");
        }
    }
}
