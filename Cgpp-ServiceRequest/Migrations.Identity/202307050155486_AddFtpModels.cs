namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFtpModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FtpModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FtpHost = c.String(),
                        FtpPort = c.String(),
                        FtpUsername = c.String(),
                        FtpPassword = c.String(),
                        HardwareUserRequestId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HardwareUserRequests", t => t.HardwareUserRequestId, cascadeDelete: true)
                .Index(t => t.HardwareUserRequestId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FtpModels", "HardwareUserRequestId", "dbo.HardwareUserRequests");
            DropIndex("dbo.FtpModels", new[] { "HardwareUserRequestId" });
            DropTable("dbo.FtpModels");
        }
    }
}
