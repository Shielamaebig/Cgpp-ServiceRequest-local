namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDateAddedFtp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ftps", "DateAdded", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ftps", "DateAdded");
        }
    }
}
