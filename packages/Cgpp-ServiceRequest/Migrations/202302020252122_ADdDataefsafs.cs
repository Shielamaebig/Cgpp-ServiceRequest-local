namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ADdDataefsafs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HardwareRequests", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.SoftwareRequests", "DateCreated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SoftwareRequests", "DateCreated");
            DropColumn("dbo.HardwareRequests", "DateCreated");
        }
    }
}
