namespace BandoApp.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ComunicacionesGeoPosicionDireccionTexto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comunicaciones", "GeoPosicionDireccion", c => c.String(maxLength: 500));
            AddColumn("dbo.Comunicaciones", "GeoPosicionLocalidad", c => c.String(maxLength: 100));
            AddColumn("dbo.Comunicaciones", "GeoPosicionProvincia", c => c.String(maxLength: 100));
            AddColumn("dbo.Comunicaciones", "GeoPosicionPais", c => c.String(maxLength: 100));
            AlterColumn("dbo.Comunicaciones", "GeoPosicionLatitud", c => c.Single());
            AlterColumn("dbo.Comunicaciones", "GeoPosicionLongitud", c => c.Single());
            
            // Poner a NULL la latitud y longitud de las filas que tenían coordenadas (0,0).
            Sql("UPDATE Comunicaciones SET GeoPosicionLongitud = NULL, GeoPosicionLatitud = NULL WHERE GeoPosicionLongitud = 0 and GeoPosicionLatitud = 0");
        }
        
        public override void Down()
        {
            // Poner a 0 la latitud y longitud de las filas que tenían coordenadas NULL.
            Sql("UPDATE Comunicaciones SET GeoPosicionLongitud = 0 WHERE GeoPosicionLongitud IS NULL");
            Sql("UPDATE Comunicaciones SET GeoPosicionLatitud = 0 WHERE GeoPosicionLatitud IS NULL");

            AlterColumn("dbo.Comunicaciones", "GeoPosicionLongitud", c => c.Single(nullable: false));
            AlterColumn("dbo.Comunicaciones", "GeoPosicionLatitud", c => c.Single(nullable: false));
            DropColumn("dbo.Comunicaciones", "GeoPosicionPais");
            DropColumn("dbo.Comunicaciones", "GeoPosicionProvincia");
            DropColumn("dbo.Comunicaciones", "GeoPosicionLocalidad");
            DropColumn("dbo.Comunicaciones", "GeoPosicionDireccion");
        }
    }
}
