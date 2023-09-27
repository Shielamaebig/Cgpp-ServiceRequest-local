namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Removedateasdas : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.HardwareRequests", "DateCreated");
            DropColumn("dbo.SoftwareRequests", "DateCreated");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SoftwareRequests", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.HardwareRequests", "DateCreated", c => c.DateTime(nullable: false));
        }
    }
}
