namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequestHistory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequestHistories", "Category", c => c.Boolean(nullable: false));
            AddColumn("dbo.RequestHistories", "HardwareUserRequestId", c => c.Int());
            AddColumn("dbo.RequestHistories", "SoftwareUserRequestId", c => c.Int());
            CreateIndex("dbo.RequestHistories", "HardwareUserRequestId");
            CreateIndex("dbo.RequestHistories", "SoftwareUserRequestId");
            AddForeignKey("dbo.RequestHistories", "HardwareUserRequestId", "dbo.HardwareUserRequests", "Id");
            AddForeignKey("dbo.RequestHistories", "SoftwareUserRequestId", "dbo.SoftwareUserRequests", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RequestHistories", "SoftwareUserRequestId", "dbo.SoftwareUserRequests");
            DropForeignKey("dbo.RequestHistories", "HardwareUserRequestId", "dbo.HardwareUserRequests");
            DropIndex("dbo.RequestHistories", new[] { "SoftwareUserRequestId" });
            DropIndex("dbo.RequestHistories", new[] { "HardwareUserRequestId" });
            DropColumn("dbo.RequestHistories", "SoftwareUserRequestId");
            DropColumn("dbo.RequestHistories", "HardwareUserRequestId");
            DropColumn("dbo.RequestHistories", "Category");
        }
    }
}
