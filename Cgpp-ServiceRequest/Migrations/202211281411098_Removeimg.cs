namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Removeimg : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "ImgPath");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "ImgPath", c => c.String());
        }
    }
}
