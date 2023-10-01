namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DatetimetoString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LoginActivities", "ActivityDate", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LoginActivities", "ActivityDate", c => c.DateTime());
        }
    }
}
