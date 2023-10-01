namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveIssend : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.HardwareUploads", "isSend");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HardwareUploads", "isSend", c => c.Boolean(nullable: false));
        }
    }
}
