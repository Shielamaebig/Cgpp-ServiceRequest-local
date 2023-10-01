namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateCreated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserForgotPasswords", "DateCreated", c => c.String());
            AddColumn("dbo.UserRegistrations", "DateCreated", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserRegistrations", "DateCreated");
            DropColumn("dbo.UserForgotPasswords", "DateCreated");
        }
    }
}
