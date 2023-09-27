namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequestHistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RequestHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Email = c.String(),
                        DepartmentsId = c.Int(),
                        DivisionsId = c.Int(),
                        UploadMessage = c.String(),
                        UploadDate = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.DepartmentsId)
                .ForeignKey("dbo.Divisions", t => t.DivisionsId)
                .Index(t => t.DepartmentsId)
                .Index(t => t.DivisionsId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RequestHistories", "DivisionsId", "dbo.Divisions");
            DropForeignKey("dbo.RequestHistories", "DepartmentsId", "dbo.Departments");
            DropIndex("dbo.RequestHistories", new[] { "DivisionsId" });
            DropIndex("dbo.RequestHistories", new[] { "DepartmentsId" });
            DropTable("dbo.RequestHistories");
        }
    }
}
