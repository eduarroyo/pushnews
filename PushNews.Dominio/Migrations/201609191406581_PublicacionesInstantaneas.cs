namespace BandoApp.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PublicacionesInstantaneas : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comunicaciones", "Instantanea", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comunicaciones", "Instantanea");
        }
    }
}
