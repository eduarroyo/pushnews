namespace PushNews.Dominio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SimplificarUsuarios : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Usuarios", "Movil");
            DropColumn("dbo.Usuarios", "MovilConfirmado");
            DropColumn("dbo.Usuarios", "AccesosFallidos");
            DropColumn("dbo.Usuarios", "FinalBloqueoUtc");
            DropColumn("dbo.Usuarios", "DosFactoresHabilitado");
            DropColumn("dbo.Usuarios", "ProveedorID");
            DropColumn("dbo.Usuarios", "UltimoLogin");
            DropColumn("dbo.Usuarios", "Locale");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Usuarios", "Locale", c => c.String(maxLength: 20));
            AddColumn("dbo.Usuarios", "UltimoLogin", c => c.DateTime());
            AddColumn("dbo.Usuarios", "ProveedorID", c => c.Long());
            AddColumn("dbo.Usuarios", "DosFactoresHabilitado", c => c.Boolean(nullable: false));
            AddColumn("dbo.Usuarios", "FinalBloqueoUtc", c => c.DateTime());
            AddColumn("dbo.Usuarios", "AccesosFallidos", c => c.Int(nullable: false));
            AddColumn("dbo.Usuarios", "MovilConfirmado", c => c.Boolean(nullable: false));
            AddColumn("dbo.Usuarios", "Movil", c => c.String(maxLength: 50));
        }
    }
}
