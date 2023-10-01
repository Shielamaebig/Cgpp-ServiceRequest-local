namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTelANydesk : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HardwareUserRequests", "TelNumber", c => c.String());
            AddColumn("dbo.HardwareUserRequests", "AnyDesk", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HardwareUserRequests", "AnyDesk");
            DropColumn("dbo.HardwareUserRequests", "TelNumber");
        }
    }
}
