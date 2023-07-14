namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FtpCredentoas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FtpCredentials",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FtpHost = c.String(),
                        FtpPort = c.String(),
                        FtpUsername = c.String(),
                        FtpPassword = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FtpCredentials");
        }
    }
}
