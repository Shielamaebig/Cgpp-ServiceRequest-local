namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RatatillaTom : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HardwareTechnicians", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.LoginActivities", "DepartmentName", c => c.String());
            AddColumn("dbo.LoginActivities", "DivisionName", c => c.String());
            AddColumn("dbo.SoftwareTechnicians", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SoftwareTechnicians", "IsActive");
            DropColumn("dbo.LoginActivities", "DivisionName");
            DropColumn("dbo.LoginActivities", "DepartmentName");
            DropColumn("dbo.HardwareTechnicians", "IsActive");
        }
    }
}
