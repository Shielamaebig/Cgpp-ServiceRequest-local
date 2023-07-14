namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequiredResulotion : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProgrammerReports", "Resulotion", c => c.String(nullable: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProgrammerReports", "Resulotion", c => c.String());
        }
    }
}
