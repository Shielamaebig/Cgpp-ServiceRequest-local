namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFolderName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ftps", "FolderName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ftps", "FolderName");
        }
    }
}
