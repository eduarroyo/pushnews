namespace BandoApp.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApiKeyExternos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Aplicaciones", "ApiKeyExternos", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Aplicaciones", "ApiKeyExternos");
        }
    }
}
