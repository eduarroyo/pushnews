namespace PushNews.Dominio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuitarMarcaSeguridad : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuarios", "MarcaSeguridad", c => c.String());
            AddColumn("dbo.Usuarios", "Externo", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Usuarios", "Externo");
            DropColumn("dbo.Usuarios", "MarcaSeguridad");
        }
    }
}
