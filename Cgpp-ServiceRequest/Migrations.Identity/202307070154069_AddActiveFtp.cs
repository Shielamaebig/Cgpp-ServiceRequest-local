namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddActiveFtp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FtpModels", "isActiveFtp", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FtpModels", "isActiveFtp");
        }
    }
}
