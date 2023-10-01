namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImagePath2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SoftwareRequests", "ImageFile", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SoftwareRequests", "ImageFile");
        }
    }
}
