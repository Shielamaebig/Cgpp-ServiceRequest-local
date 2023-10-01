namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsSend : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HardwareUploads", "isSend", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HardwareUploads", "isSend");
        }
    }
}
