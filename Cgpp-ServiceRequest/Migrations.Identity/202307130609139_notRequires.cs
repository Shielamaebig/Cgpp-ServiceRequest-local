namespace Cgpp_ServiceRequest.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class notRequires : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProgrammerReports", "Resulotion", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProgrammerReports", "Resulotion", c => c.String(nullable: false));
        }
    }
}
