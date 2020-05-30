namespace PushNews.Dominio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RutasCalculoTiempoHoras : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rutas", "CalculoTiempoHoras", c => c.Double(nullable: false));
            DropColumn("dbo.Rutas", "CalculoTiempo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rutas", "CalculoTiempo", c => c.Time(nullable: false, precision: 7));
            DropColumn("dbo.Rutas", "CalculoTiempoHoras");
        }
    }
}
