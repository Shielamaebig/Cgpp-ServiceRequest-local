namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameFtpModels : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.FtpModels", newName: "FtpHardwareModels");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.FtpHardwareModels", newName: "FtpModels");
        }
    }
}
