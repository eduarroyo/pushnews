namespace BandoApp.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Gpss_Campo_Api : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Gpss", "Api", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Gpss", "Api");
        }
    }
}
