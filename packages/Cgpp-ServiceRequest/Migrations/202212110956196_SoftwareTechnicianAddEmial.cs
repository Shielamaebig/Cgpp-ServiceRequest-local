namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SoftwareTechnicianAddEmial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SoftwareTechnicians", "TechEmail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SoftwareTechnicians", "TechEmail");
        }
    }
}
