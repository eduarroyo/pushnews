namespace PushNews.Dominio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApiKey_UrlsStores : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Aplicaciones", "ApiKey", c => c.String(maxLength: 500));
            AddColumn("dbo.Aplicaciones", "PlayStoreUrl", c => c.String(maxLength: 1000));
            AddColumn("dbo.Aplicaciones", "ITunesUrl", c => c.String(maxLength: 1000));
            AddColumn("dbo.Aplicaciones", "MicrosoftStoreUrl", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Aplicaciones", "MicrosoftStoreUrl");
            DropColumn("dbo.Aplicaciones", "ITunesUrl");
            DropColumn("dbo.Aplicaciones", "PlayStoreUrl");
            DropColumn("dbo.Aplicaciones", "ApiKey");
        }
    }
}
