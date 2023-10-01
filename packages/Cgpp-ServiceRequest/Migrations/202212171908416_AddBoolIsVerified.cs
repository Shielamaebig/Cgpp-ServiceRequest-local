namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBoolIsVerified : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HardwareRequests", "IsVerified", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HardwareRequests", "IsVerified");
        }
    }
}
