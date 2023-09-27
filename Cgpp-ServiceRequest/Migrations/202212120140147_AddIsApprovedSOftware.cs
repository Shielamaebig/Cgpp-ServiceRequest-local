namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsApprovedSOftware : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SoftwareRequests", "isApproved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SoftwareRequests", "isApproved");
        }
    }
}
