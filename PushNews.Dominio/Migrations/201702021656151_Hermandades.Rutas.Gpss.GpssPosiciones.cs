namespace PushNews.Dominio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HermandadesRutasGpssGpssPosiciones : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Gpss",
                c => new
                    {
                        GpsID = c.Long(nullable: false, identity: true),
                        GpsApiID = c.Long(nullable: false),
                        AplicacionID = c.Long(nullable: false),
                        Activo = c.Boolean(nullable: false),
                        Matricula = c.String(nullable: false, maxLength: 200),
                        Estado = c.String(nullable: false, maxLength: 200, defaultValue: ""),
                        Bateria = c.Double(),
                        Sensores = c.String(nullable: false, maxLength: 200, defaultValue: ""),
                        UltimaLecturaFecha = c.DateTime(),
                        UltimaPosicionFecha = c.DateTime(),
                        UltimaPosicionDireccion = c.String(nullable: false, maxLength: 200, defaultValue: ""),
                        UltimaPosicionLatitud = c.Double(),
                        UltimaPosicionLongitud = c.Double(),
                    })
                .PrimaryKey(t => t.GpsID)
                .ForeignKey("dbo.Aplicaciones", t => t.AplicacionID)
                .Index(t => t.AplicacionID);
            
            CreateTable(
                "dbo.GpsPosiciones",
                c => new
                    {
                        GpsPosicionID = c.Long(nullable: false, identity: true),
                        GpsID = c.Long(nullable: false),
                        LecturaFecha = c.DateTime(nullable: false),
                        PosicionFecha = c.DateTime(nullable: false),
                        Latitud = c.Double(nullable: false),
                        Longitud = c.Double(nullable: false),
                        Velocidad = c.Double(nullable: false),
                        Kilometros = c.Double(nullable: false),
                        Manual = c.Boolean(nullable: false, defaultValue: false),
                    })
                .PrimaryKey(t => t.GpsPosicionID)
                .ForeignKey("dbo.Gpss", t => t.GpsID)
                .Index(t => t.GpsID);
            
            CreateTable(
                "dbo.Hermandades",
                c => new
                    {
                        HermandadID = c.Long(nullable: false, identity: true),
                        AplicacionID = c.Long(nullable: false),
                        Nombre = c.String(nullable: false, maxLength: 200),
                        LogotipoDocumentoID = c.Long(),
                        IglesiaNombre = c.String(nullable: false, maxLength: 200, defaultValue: ""),
                        IglesiaDireccion = c.String(nullable: false, maxLength: 500, defaultValue: ""),
                        IglesiaLatitud = c.Double(),
                        IglesiaLongitud = c.Double(),
                        Activo = c.Boolean(nullable: false, defaultValue: true),
                    })
                .PrimaryKey(t => t.HermandadID)
                .ForeignKey("dbo.Aplicaciones", t => t.AplicacionID)
                .ForeignKey("dbo.Documentos", t => t.LogotipoDocumentoID)
                .Index(t => t.AplicacionID)
                .Index(t => t.LogotipoDocumentoID);
            
            CreateTable(
                "dbo.Rutas",
                c => new
                    {
                        RutaID = c.Long(nullable: false, identity: true),
                        HermandadID = c.Long(nullable: false),
                        Descripcion = c.String(nullable: false, maxLength: 500),
                        InicioFecha = c.DateTime(nullable: false),
                        InicioDescripcion = c.String(nullable: false, maxLength: 200, defaultValue: ""),
                        InicioLatitud = c.Double(nullable: false),
                        InicioLongitud = c.Double(nullable: false),
                        EntradaEnCarreraOficial = c.String(nullable: false, maxLength: 200, defaultValue: ""),
                        EntradaEnCarreraOficialLatitud = c.Double(nullable: false),
                        EntradaEnCarreraOficialLongitud = c.Double(nullable: false),
                        FinFecha = c.DateTime(),
                        FinDescripcion = c.String(nullable: false, maxLength: 200, defaultValue: ""),
                        FinLatitud = c.Double(nullable: false),
                        FinLongitud = c.Double(nullable: false),
                        GpsCabezaID = c.Long(nullable: false),
                        CabezaUltimaPosicionFecha = c.DateTime(nullable: false),
                        CabezaUltimaPosicionDireccion = c.String(nullable: false, maxLength: 200, defaultValue: ""),
                        CabezaUltimaPosicionLongitud = c.Double(nullable: false),
                        CabezaUltimaPosicionLatidud = c.Double(nullable: false),
                        GpsColaID = c.Long(nullable: false),
                        ColaUltimaPosicionFecha = c.DateTime(),
                        ColaUltimaPosicionDireccion = c.String(nullable: false, maxLength: 200, defaultValue: ""),
                        ColaUltimaPosicionLongitud = c.Double(),
                        ColaUltimaPosicionLatidud = c.Double(),
                        CalculoVelodicad = c.Double(nullable: false),
                        CalculoDistancia = c.Double(nullable: false),
                        CalculoTiempo = c.Time(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.RutaID)
                .ForeignKey("dbo.Gpss", t => t.GpsCabezaID)
                .ForeignKey("dbo.Gpss", t => t.GpsColaID)
                .ForeignKey("dbo.Hermandades", t => t.HermandadID)
                .Index(t => t.HermandadID)
                .Index(t => t.GpsCabezaID)
                .Index(t => t.GpsColaID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rutas", "HermandadID", "dbo.Hermandades");
            DropForeignKey("dbo.Rutas", "GpsColaID", "dbo.Gpss");
            DropForeignKey("dbo.Rutas", "GpsCabezaID", "dbo.Gpss");
            DropForeignKey("dbo.Hermandades", "LogotipoDocumentoID", "dbo.Documentos");
            DropForeignKey("dbo.Hermandades", "AplicacionID", "dbo.Aplicaciones");
            DropForeignKey("dbo.GpsPosiciones", "GpsID", "dbo.Gpss");
            DropForeignKey("dbo.Gpss", "AplicacionID", "dbo.Aplicaciones");
            DropIndex("dbo.Rutas", new[] { "GpsColaID" });
            DropIndex("dbo.Rutas", new[] { "GpsCabezaID" });
            DropIndex("dbo.Rutas", new[] { "HermandadID" });
            DropIndex("dbo.Hermandades", new[] { "LogotipoDocumentoID" });
            DropIndex("dbo.Hermandades", new[] { "AplicacionID" });
            DropIndex("dbo.GpsPosiciones", new[] { "GpsID" });
            DropIndex("dbo.Gpss", new[] { "AplicacionID" });
            DropTable("dbo.Rutas");
            DropTable("dbo.Hermandades");
            DropTable("dbo.GpsPosiciones");
            DropTable("dbo.Gpss");
        }
    }
}
