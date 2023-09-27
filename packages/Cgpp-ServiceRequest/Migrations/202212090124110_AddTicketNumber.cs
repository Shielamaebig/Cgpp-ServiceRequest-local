namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTicketNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HardwareRequests", "TicketNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HardwareRequests", "TicketNumber");
        }
    }
}
