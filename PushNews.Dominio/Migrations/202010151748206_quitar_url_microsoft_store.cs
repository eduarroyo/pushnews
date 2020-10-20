namespace PushNews.Dominio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class quitar_url_microsoft_store : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Aplicaciones", "MicrosoftStoreUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Aplicaciones", "MicrosoftStoreUrl", c => c.String(maxLength: 1000));
        }
    }
}
