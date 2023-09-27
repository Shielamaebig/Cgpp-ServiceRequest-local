namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ADdNewIsDepartmentApprover : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Departments", "IsDepartmentApprover", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Departments", "IsDepartmentApprover");
        }
    }
}
