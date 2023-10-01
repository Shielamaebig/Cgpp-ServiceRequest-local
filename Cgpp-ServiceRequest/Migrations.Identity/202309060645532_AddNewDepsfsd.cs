namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewDepsfsd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Departments", "TryAdd", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Departments", "TryAdd");
        }
    }
}
