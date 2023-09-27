namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequired : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "DepartmentsId", "dbo.Departments");
            DropIndex("dbo.AspNetUsers", new[] { "DepartmentsId" });
            AlterColumn("dbo.AspNetUsers", "FullName", c => c.String(nullable: false));
            AlterColumn("dbo.AspNetUsers", "MobileNumber", c => c.String(nullable: false));
            AlterColumn("dbo.AspNetUsers", "DepartmentsId", c => c.Int(nullable: false));
            CreateIndex("dbo.AspNetUsers", "DepartmentsId");
            AddForeignKey("dbo.AspNetUsers", "DepartmentsId", "dbo.Departments", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "DepartmentsId", "dbo.Departments");
            DropIndex("dbo.AspNetUsers", new[] { "DepartmentsId" });
            AlterColumn("dbo.AspNetUsers", "DepartmentsId", c => c.Int());
            AlterColumn("dbo.AspNetUsers", "MobileNumber", c => c.String());
            AlterColumn("dbo.AspNetUsers", "FullName", c => c.String());
            CreateIndex("dbo.AspNetUsers", "DepartmentsId");
            AddForeignKey("dbo.AspNetUsers", "DepartmentsId", "dbo.Departments", "Id");
        }
    }
}
