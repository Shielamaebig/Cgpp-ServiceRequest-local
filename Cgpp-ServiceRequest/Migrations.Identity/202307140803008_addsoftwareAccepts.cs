namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addsoftwareAccepts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SoftwareVerifications", "SoftwareAcceptsRequestId", c => c.Int());
            CreateIndex("dbo.SoftwareVerifications", "SoftwareAcceptsRequestId");
            AddForeignKey("dbo.SoftwareVerifications", "SoftwareAcceptsRequestId", "dbo.SoftwareAcceptsRequests", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SoftwareVerifications", "SoftwareAcceptsRequestId", "dbo.SoftwareAcceptsRequests");
            DropIndex("dbo.SoftwareVerifications", new[] { "SoftwareAcceptsRequestId" });
            DropColumn("dbo.SoftwareVerifications", "SoftwareAcceptsRequestId");
        }
    }
}
