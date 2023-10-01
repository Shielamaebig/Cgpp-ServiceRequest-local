namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class idontknowjkbmsdf : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.FtpModels");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.FtpModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Address = c.String(),
                        Port = c.Int(nullable: false),
                        UserName = c.String(),
                        Password = c.String(),
                        Label = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
