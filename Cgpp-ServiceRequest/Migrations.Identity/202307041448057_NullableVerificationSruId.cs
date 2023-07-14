namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableVerificationSruId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SoftwareVerifications", "SoftwareUserRequestId", "dbo.SoftwareUserRequests");
            DropIndex("dbo.SoftwareVerifications", new[] { "SoftwareUserRequestId" });
            AlterColumn("dbo.SoftwareVerifications", "SoftwareUserRequestId", c => c.Int());
            CreateIndex("dbo.SoftwareVerifications", "SoftwareUserRequestId");
            AddForeignKey("dbo.SoftwareVerifications", "SoftwareUserRequestId", "dbo.SoftwareUserRequests", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SoftwareVerifications", "SoftwareUserRequestId", "dbo.SoftwareUserRequests");
            DropIndex("dbo.SoftwareVerifications", new[] { "SoftwareUserRequestId" });
            AlterColumn("dbo.SoftwareVerifications", "SoftwareUserRequestId", c => c.Int(nullable: false));
            CreateIndex("dbo.SoftwareVerifications", "SoftwareUserRequestId");
            AddForeignKey("dbo.SoftwareVerifications", "SoftwareUserRequestId", "dbo.SoftwareUserRequests", "Id", cascadeDelete: true);
        }
    }
}
