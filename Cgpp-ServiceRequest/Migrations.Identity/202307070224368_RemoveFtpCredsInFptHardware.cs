namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveFtpCredsInFptHardware : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.FtpHardwareModels", "FtpHost");
            DropColumn("dbo.FtpHardwareModels", "FtpPort");
            DropColumn("dbo.FtpHardwareModels", "FtpUsername");
            DropColumn("dbo.FtpHardwareModels", "FtpPassword");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FtpHardwareModels", "FtpPassword", c => c.String());
            AddColumn("dbo.FtpHardwareModels", "FtpUsername", c => c.String());
            AddColumn("dbo.FtpHardwareModels", "FtpPort", c => c.String());
            AddColumn("dbo.FtpHardwareModels", "FtpHost", c => c.String());
        }
    }
}
