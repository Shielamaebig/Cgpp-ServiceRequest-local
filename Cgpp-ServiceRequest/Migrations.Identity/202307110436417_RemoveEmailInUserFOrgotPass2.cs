namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveEmailInUserFOrgotPass2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserForgotPasswords", "FirstName", c => c.String(nullable: false));
            DropColumn("dbo.UserForgotPasswords", "FirsName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserForgotPasswords", "FirsName", c => c.String(nullable: false));
            DropColumn("dbo.UserForgotPasswords", "FirstName");
        }
    }
}
