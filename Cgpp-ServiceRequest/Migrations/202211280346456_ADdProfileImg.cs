namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ADdProfileImg : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ImgPath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ImgPath");
        }
    }
}
