namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFtpModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FtpModels",
                c => new
                    {
                        Id = c.Int(nullable: true, identity: true),
                        Address = c.String(),
                        Port = c.Int(nullable: true),
                        UserName = c.String(),
                        Label = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FtpModels");
        }
    }
}
