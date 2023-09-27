namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSoftwareUploads : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SoftwareUploads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImagePath = c.String(),
                        FileName = c.String(),
                        SoftwareRequestId = c.Int(nullable: false),
                        DateAdded = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SoftwareRequests", t => t.SoftwareRequestId, cascadeDelete: true)
                .Index(t => t.SoftwareRequestId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SoftwareUploads", "SoftwareRequestId", "dbo.SoftwareRequests");
            DropIndex("dbo.SoftwareUploads", new[] { "SoftwareRequestId" });
            DropTable("dbo.SoftwareUploads");
        }
    }
}
