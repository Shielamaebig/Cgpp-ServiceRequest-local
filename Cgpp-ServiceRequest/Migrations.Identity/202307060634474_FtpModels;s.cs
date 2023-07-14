namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FtpModelss : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.FtpCredentials", newName: "Ftps");
            CreateTable(
                "dbo.FtpModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FtpId = c.Int(),
                        Name = c.String(),
                        Description = c.String(),
                        DateAdded = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ftps", t => t.FtpId)
                .Index(t => t.FtpId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FtpModels", "FtpId", "dbo.Ftps");
            DropIndex("dbo.FtpModels", new[] { "FtpId" });
            DropTable("dbo.FtpModels");
            RenameTable(name: "dbo.Ftps", newName: "FtpCredentials");
        }
    }
}
