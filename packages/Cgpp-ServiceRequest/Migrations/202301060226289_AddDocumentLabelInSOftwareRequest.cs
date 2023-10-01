namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDocumentLabelInSOftwareRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SoftwareRequests", "DocumentLabel", c => c.String());
            DropColumn("dbo.SoftwareRequests", "ImageFile");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SoftwareRequests", "ImageFile", c => c.String());
            DropColumn("dbo.SoftwareRequests", "DocumentLabel");
        }
    }
}
