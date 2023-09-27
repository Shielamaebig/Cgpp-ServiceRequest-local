namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHardwareUploads : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HardwareUploads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImagePath = c.String(),
                        HardwareRequestId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HardwareRequests", t => t.HardwareRequestId, cascadeDelete: true)
                .Index(t => t.HardwareRequestId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HardwareUploads", "HardwareRequestId", "dbo.HardwareRequests");
            DropIndex("dbo.HardwareUploads", new[] { "HardwareRequestId" });
            DropTable("dbo.HardwareUploads");
        }
    }
}
