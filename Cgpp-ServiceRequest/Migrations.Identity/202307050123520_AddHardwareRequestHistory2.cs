namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHardwareRequestHistory2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HardwareUserUploads", "FtpHostName", c => c.String());
            AddColumn("dbo.HardwareUserUploads", "FtpUserName", c => c.String());
            AddColumn("dbo.HardwareUserUploads", "FtpPort", c => c.String());
            AddColumn("dbo.HardwareUserUploads", "FtpPassword", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HardwareUserUploads", "FtpPassword");
            DropColumn("dbo.HardwareUserUploads", "FtpPort");
            DropColumn("dbo.HardwareUserUploads", "FtpUserName");
            DropColumn("dbo.HardwareUserUploads", "FtpHostName");
        }
    }
}
