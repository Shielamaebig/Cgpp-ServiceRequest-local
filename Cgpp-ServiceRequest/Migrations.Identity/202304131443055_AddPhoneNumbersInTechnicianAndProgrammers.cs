namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPhoneNumbersInTechnicianAndProgrammers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HardwareTechnicians", "PhoneNumber", c => c.String());
            AddColumn("dbo.SoftwareTechnicians", "PhoneNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SoftwareTechnicians", "PhoneNumber");
            DropColumn("dbo.HardwareTechnicians", "PhoneNumber");
        }
    }
}
