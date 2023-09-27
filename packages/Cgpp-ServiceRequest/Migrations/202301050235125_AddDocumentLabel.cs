namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDocumentLabel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HardwareRequests", "DocumentLabel", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HardwareRequests", "DocumentLabel");
        }
    }
}
