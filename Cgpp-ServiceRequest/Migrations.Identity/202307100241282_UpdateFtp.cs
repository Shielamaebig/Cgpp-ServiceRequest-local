namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateFtp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ftps", "Label", c => c.String());
            DropColumn("dbo.Ftps", "isActiveFtp");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ftps", "isActiveFtp", c => c.Boolean(nullable: false));
            DropColumn("dbo.Ftps", "Label");
        }
    }
}
