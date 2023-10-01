namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddprogrammerEmail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SoftwareRequests", "ProEmail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SoftwareRequests", "ProEmail");
        }
    }
}
