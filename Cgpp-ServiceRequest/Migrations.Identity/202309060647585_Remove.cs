namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Remove : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Departments", "TryAdd");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Departments", "TryAdd", c => c.String());
        }
    }
}
