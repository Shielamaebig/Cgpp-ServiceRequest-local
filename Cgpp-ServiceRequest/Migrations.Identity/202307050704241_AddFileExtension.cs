namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFileExtension : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HardwareUserUploads", "FileExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HardwareUserUploads", "FileExtension");
        }
    }
}
