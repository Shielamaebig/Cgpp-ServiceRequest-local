namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveTechAdminIdInTechReport : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TechnicianReports", "TechnicianAdminId", "dbo.TechnicianAdmins");
            DropIndex("dbo.TechnicianReports", new[] { "TechnicianAdminId" });
            DropColumn("dbo.TechnicianReports", "TechnicianAdminId");
            DropTable("dbo.TechnicianAdmins");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TechnicianAdmins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.TechnicianReports", "TechnicianAdminId", c => c.Int());
            CreateIndex("dbo.TechnicianReports", "TechnicianAdminId");
            AddForeignKey("dbo.TechnicianReports", "TechnicianAdminId", "dbo.TechnicianAdmins", "Id");
        }
    }
}
