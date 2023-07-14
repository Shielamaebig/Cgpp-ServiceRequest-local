namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFirstNameLastMiddleName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HardwareUserRequests", "FirstName", c => c.String());
            AddColumn("dbo.HardwareUserRequests", "MiddleName", c => c.String());
            AddColumn("dbo.HardwareUserRequests", "LastName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HardwareUserRequests", "LastName");
            DropColumn("dbo.HardwareUserRequests", "MiddleName");
            DropColumn("dbo.HardwareUserRequests", "FirstName");
        }
    }
}
