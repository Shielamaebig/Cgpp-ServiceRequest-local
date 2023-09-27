namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsViewed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SoftwareRequests", "IsViewed", c => c.Boolean(nullable: false));
            DropColumn("dbo.SoftwareRequests", "ViewedRemarks");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SoftwareRequests", "ViewedRemarks", c => c.String());
            DropColumn("dbo.SoftwareRequests", "IsViewed");
        }
    }
}
