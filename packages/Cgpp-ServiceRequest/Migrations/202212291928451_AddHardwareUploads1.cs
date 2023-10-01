namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHardwareUploads1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HardwareUploads", "Created", c => c.String());
            AddColumn("dbo.HardwareUploads", "DepartmentsId", c => c.Int());
            AddColumn("dbo.HardwareUploads", "DivisionsId", c => c.Int());
            AddColumn("dbo.HardwareUploads", "DateAdded", c => c.String());
            CreateIndex("dbo.HardwareUploads", "DepartmentsId");
            CreateIndex("dbo.HardwareUploads", "DivisionsId");
            AddForeignKey("dbo.HardwareUploads", "DepartmentsId", "dbo.Departments", "Id");
            AddForeignKey("dbo.HardwareUploads", "DivisionsId", "dbo.Divisions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HardwareUploads", "DivisionsId", "dbo.Divisions");
            DropForeignKey("dbo.HardwareUploads", "DepartmentsId", "dbo.Departments");
            DropIndex("dbo.HardwareUploads", new[] { "DivisionsId" });
            DropIndex("dbo.HardwareUploads", new[] { "DepartmentsId" });
            DropColumn("dbo.HardwareUploads", "DateAdded");
            DropColumn("dbo.HardwareUploads", "DivisionsId");
            DropColumn("dbo.HardwareUploads", "DepartmentsId");
            DropColumn("dbo.HardwareUploads", "Created");
        }
    }
}
