namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFileName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HardwareUploads", "FileName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HardwareUploads", "FileName");
        }
    }
}
