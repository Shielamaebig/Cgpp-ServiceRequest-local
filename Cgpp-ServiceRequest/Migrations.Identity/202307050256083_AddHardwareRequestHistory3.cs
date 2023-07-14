namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHardwareRequestHistory3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.HardwareUserUploads", "FtpHostName");
            DropColumn("dbo.HardwareUserUploads", "FtpUserName");
            DropColumn("dbo.HardwareUserUploads", "FtpPort");
            DropColumn("dbo.HardwareUserUploads", "FtpPassword");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HardwareUserUploads", "FtpPassword", c => c.String());
            AddColumn("dbo.HardwareUserUploads", "FtpPort", c => c.String());
            AddColumn("dbo.HardwareUserUploads", "FtpUserName", c => c.String());
            AddColumn("dbo.HardwareUserUploads", "FtpHostName", c => c.String());
        }
    }
}
