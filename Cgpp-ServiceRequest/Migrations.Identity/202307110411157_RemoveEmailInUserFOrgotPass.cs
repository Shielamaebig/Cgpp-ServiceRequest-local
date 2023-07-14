namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveEmailInUserFOrgotPass : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.UserForgotPasswords", "Email");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserForgotPasswords", "Email", c => c.String());
        }
    }
}
