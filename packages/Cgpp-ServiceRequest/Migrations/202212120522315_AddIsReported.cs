namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsReported : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SoftwareRequests", "IsReported", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SoftwareRequests", "IsReported");
        }
    }
}
