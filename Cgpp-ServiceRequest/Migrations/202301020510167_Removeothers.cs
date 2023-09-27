namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Removeothers : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HardwareUploads", "DepartmentsId", "dbo.Departments");
            DropForeignKey("dbo.HardwareUploads", "DivisionsId", "dbo.Divisions");
            DropIndex("dbo.HardwareUploads", new[] { "DepartmentsId" });
            DropIndex("dbo.HardwareUploads", new[] { "DivisionsId" });
            DropColumn("dbo.HardwareUploads", "Created");
            DropColumn("dbo.HardwareUploads", "DepartmentsId");
            DropColumn("dbo.HardwareUploads", "DivisionsId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HardwareUploads", "DivisionsId", c => c.Int());
            AddColumn("dbo.HardwareUploads", "DepartmentsId", c => c.Int());
            AddColumn("dbo.HardwareUploads", "Created", c => c.String());
            CreateIndex("dbo.HardwareUploads", "DivisionsId");
            CreateIndex("dbo.HardwareUploads", "DepartmentsId");
            AddForeignKey("dbo.HardwareUploads", "DivisionsId", "dbo.Divisions", "Id");
            AddForeignKey("dbo.HardwareUploads", "DepartmentsId", "dbo.Departments", "Id");
        }
    }
}
