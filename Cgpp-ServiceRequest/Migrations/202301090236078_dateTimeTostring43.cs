namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dateTimeTostring43 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FullCalendars", "Start", c => c.String());
            AlterColumn("dbo.FullCalendars", "End", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FullCalendars", "End", c => c.DateTime(nullable: false));
            AlterColumn("dbo.FullCalendars", "Start", c => c.DateTime(nullable: false));
        }
    }
}
