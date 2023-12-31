﻿namespace Cgpp_ServiceRequest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmailToTechnician : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HardwareTechnicians", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HardwareTechnicians", "Email");
        }
    }
}
