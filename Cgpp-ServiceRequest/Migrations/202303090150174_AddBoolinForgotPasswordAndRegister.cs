namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBoolinForgotPasswordAndRegister : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserForgotPasswords", "IsRequest", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserRegistrations", "IsNewAccount", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserRegistrations", "IsNewAccount");
            DropColumn("dbo.UserForgotPasswords", "IsRequest");
        }
    }
}
