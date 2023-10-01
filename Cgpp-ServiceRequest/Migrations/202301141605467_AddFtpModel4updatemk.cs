namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFtpModel4updatemk : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FtpModels", "Password", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FtpModels", "Password");
        }
    }
}
