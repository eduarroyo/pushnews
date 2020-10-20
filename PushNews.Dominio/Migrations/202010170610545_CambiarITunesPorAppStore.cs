namespace PushNews.Dominio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CambiarITunesPorAppStore : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Aplicaciones", "AppStoreUrl", c => c.String(maxLength: 1000));
            DropColumn("dbo.Aplicaciones", "ITunesUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Aplicaciones", "ITunesUrl", c => c.String(maxLength: 1000));
            DropColumn("dbo.Aplicaciones", "AppStoreUrl");
        }
    }
}
