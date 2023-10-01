namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveTicketNumber : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.HardwareRequests", "TicketNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HardwareRequests", "TicketNumber", c => c.String());
        }
    }
}
