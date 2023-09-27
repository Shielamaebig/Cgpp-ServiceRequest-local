namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsnew : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SoftwareRequests", "IsNew", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SoftwareRequests", "IsNew");
        }
    }
}
