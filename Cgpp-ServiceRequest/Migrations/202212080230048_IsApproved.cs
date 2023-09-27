namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsApproved : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HardwareRequests", "IsApproved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HardwareRequests", "IsApproved");
        }
    }
}
