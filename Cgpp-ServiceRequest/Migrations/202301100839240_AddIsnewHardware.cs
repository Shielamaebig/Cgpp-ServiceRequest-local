namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsnewHardware : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HardwareRequests", "IsNew", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HardwareRequests", "IsNew");
        }
    }
}
