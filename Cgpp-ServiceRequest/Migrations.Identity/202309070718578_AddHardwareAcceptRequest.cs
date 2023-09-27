namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHardwareAcceptRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Divisions", "IsDivisionApprover", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Divisions", "IsDivisionApprover");
        }
    }
}
