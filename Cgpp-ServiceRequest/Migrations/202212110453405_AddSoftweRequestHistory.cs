namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSoftweRequestHistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SoftwareRequestHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Email = c.String(),
                        DepartmentsId = c.Int(),
                        DivisionsId = c.Int(),
                        RequetMessage = c.String(),
                        RequetDate = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.DepartmentsId)
                .ForeignKey("dbo.Divisions", t => t.DivisionsId)
                .Index(t => t.DepartmentsId)
                .Index(t => t.DivisionsId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SoftwareRequestHistories", "DivisionsId", "dbo.Divisions");
            DropForeignKey("dbo.SoftwareRequestHistories", "DepartmentsId", "dbo.Departments");
            DropIndex("dbo.SoftwareRequestHistories", new[] { "DivisionsId" });
            DropIndex("dbo.SoftwareRequestHistories", new[] { "DepartmentsId" });
            DropTable("dbo.SoftwareRequestHistories");
        }
    }
}
