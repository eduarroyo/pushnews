namespace BandoApp.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RutasNulidadCamposCabeza : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Rutas", new[] { "GpsColaID" });
            AddColumn("dbo.Rutas", "CalculoVelocidad", c => c.Double(nullable: false));
            AlterColumn("dbo.Rutas", "CabezaUltimaPosicionFecha", c => c.DateTime());
            AlterColumn("dbo.Rutas", "CabezaUltimaPosicionLongitud", c => c.Double());
            AlterColumn("dbo.Rutas", "CabezaUltimaPosicionLatidud", c => c.Double());
            AlterColumn("dbo.Rutas", "GpsColaID", c => c.Long());
            CreateIndex("dbo.Rutas", "GpsColaID");
            DropColumn("dbo.Rutas", "CalculoVelodicad");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rutas", "CalculoVelodicad", c => c.Double(nullable: false));
            DropIndex("dbo.Rutas", new[] { "GpsColaID" });
            AlterColumn("dbo.Rutas", "GpsColaID", c => c.Long(nullable: false));
            AlterColumn("dbo.Rutas", "CabezaUltimaPosicionLatidud", c => c.Double(nullable: false));
            AlterColumn("dbo.Rutas", "CabezaUltimaPosicionLongitud", c => c.Double(nullable: false));
            AlterColumn("dbo.Rutas", "CabezaUltimaPosicionFecha", c => c.DateTime(nullable: false));
            DropColumn("dbo.Rutas", "CalculoVelocidad");
            CreateIndex("dbo.Rutas", "GpsColaID");
        }
    }
}
