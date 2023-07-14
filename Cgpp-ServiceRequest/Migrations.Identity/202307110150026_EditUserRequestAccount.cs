namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditUserRequestAccount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserForgotPasswords", "FirsName", c => c.String(nullable: false));
            AddColumn("dbo.UserForgotPasswords", "LastName", c => c.String(nullable: false));
            AddColumn("dbo.UserForgotPasswords", "MiddleName", c => c.String());
            AddColumn("dbo.UserRegistrations", "FirstName", c => c.String(nullable: false));
            AddColumn("dbo.UserRegistrations", "LastName", c => c.String(nullable: false));
            AddColumn("dbo.UserRegistrations", "MiddleName", c => c.String());
            AlterColumn("dbo.UserForgotPasswords", "Email", c => c.String());
            AlterColumn("dbo.UserRegistrations", "Email", c => c.String());
            DropColumn("dbo.UserForgotPasswords", "FullName");
            DropColumn("dbo.UserRegistrations", "FullName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserRegistrations", "FullName", c => c.String(nullable: false));
            AddColumn("dbo.UserForgotPasswords", "FullName", c => c.String(nullable: false));
            AlterColumn("dbo.UserRegistrations", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.UserForgotPasswords", "Email", c => c.String(nullable: false));
            DropColumn("dbo.UserRegistrations", "MiddleName");
            DropColumn("dbo.UserRegistrations", "LastName");
            DropColumn("dbo.UserRegistrations", "FirstName");
            DropColumn("dbo.UserForgotPasswords", "MiddleName");
            DropColumn("dbo.UserForgotPasswords", "LastName");
            DropColumn("dbo.UserForgotPasswords", "FirsName");
        }
    }
}
