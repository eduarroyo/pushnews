namespace PushNews.Dominio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmpresasActivo_RutasCorreccionLatitud : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Empresas", "Activo", c => c.Boolean(nullable: false));
            AddColumn("dbo.Rutas", "CabezaUltimaPosicionLatitud", c => c.Double());
            AddColumn("dbo.Rutas", "ColaUltimaPosicionLatitud", c => c.Double());
            DropColumn("dbo.Rutas", "CabezaUltimaPosicionLatidud");
            DropColumn("dbo.Rutas", "ColaUltimaPosicionLatidud");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rutas", "ColaUltimaPosicionLatidud", c => c.Double());
            AddColumn("dbo.Rutas", "CabezaUltimaPosicionLatidud", c => c.Double());
            DropColumn("dbo.Rutas", "ColaUltimaPosicionLatitud");
            DropColumn("dbo.Rutas", "CabezaUltimaPosicionLatitud");
            DropColumn("dbo.Empresas", "Activo");
        }
    }
}
