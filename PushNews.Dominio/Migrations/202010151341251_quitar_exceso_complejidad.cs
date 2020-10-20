namespace PushNews.Dominio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class quitar_exceso_complejidad : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AplicacionesAplicacionesAmigas", "AplicacionID", "dbo.Aplicaciones");
            DropForeignKey("dbo.AplicacionesAplicacionesAmigas", "AplicacionAmigaID", "dbo.Aplicaciones");
            DropForeignKey("dbo.Logins", "UsuarioID", "dbo.Usuarios");
            DropForeignKey("dbo.Gpss", "AplicacionID", "dbo.Aplicaciones");
            DropForeignKey("dbo.GpsPosiciones", "GpsID", "dbo.Gpss");
            DropForeignKey("dbo.Hermandades", "AplicacionID", "dbo.Aplicaciones");
            DropForeignKey("dbo.Hermandades", "LogotipoDocumentoID", "dbo.Documentos");
            DropForeignKey("dbo.Rutas", "GpsCabezaID", "dbo.Gpss");
            DropForeignKey("dbo.Rutas", "GpsColaID", "dbo.Gpss");
            DropForeignKey("dbo.Rutas", "HermandadID", "dbo.Hermandades");
            DropIndex("dbo.Logins", new[] { "UsuarioID" });
            DropIndex("dbo.Gpss", new[] { "AplicacionID" });
            DropIndex("dbo.GpsPosiciones", new[] { "GpsID" });
            DropIndex("dbo.Hermandades", new[] { "AplicacionID" });
            DropIndex("dbo.Hermandades", new[] { "LogotipoDocumentoID" });
            DropIndex("dbo.Rutas", new[] { "HermandadID" });
            DropIndex("dbo.Rutas", new[] { "GpsCabezaID" });
            DropIndex("dbo.Rutas", new[] { "GpsColaID" });
            DropIndex("dbo.AplicacionesAplicacionesAmigas", new[] { "AplicacionID" });
            DropIndex("dbo.AplicacionesAplicacionesAmigas", new[] { "AplicacionAmigaID" });
            DropTable("dbo.Logins");
            DropTable("dbo.Gpss");
            DropTable("dbo.GpsPosiciones");
            DropTable("dbo.Hermandades");
            DropTable("dbo.Rutas");
            DropTable("dbo.AplicacionesAplicacionesAmigas");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AplicacionesAplicacionesAmigas",
                c => new
                    {
                        AplicacionID = c.Long(nullable: false),
                        AplicacionAmigaID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.AplicacionID, t.AplicacionAmigaID });
            
            CreateTable(
                "dbo.Rutas",
                c => new
                    {
                        RutaID = c.Long(nullable: false, identity: true),
                        HermandadID = c.Long(nullable: false),
                        Descripcion = c.String(nullable: false, maxLength: 500),
                        InicioFecha = c.DateTime(nullable: false),
                        InicioDescripcion = c.String(nullable: false, maxLength: 200),
                        InicioLatitud = c.Double(nullable: false),
                        InicioLongitud = c.Double(nullable: false),
                        EntradaEnCarreraOficial = c.String(nullable: false, maxLength: 200),
                        EntradaEnCarreraOficialLatitud = c.Double(nullable: false),
                        EntradaEnCarreraOficialLongitud = c.Double(nullable: false),
                        FinFecha = c.DateTime(),
                        FinDescripcion = c.String(nullable: false, maxLength: 200),
                        FinLatitud = c.Double(nullable: false),
                        FinLongitud = c.Double(nullable: false),
                        GpsCabezaID = c.Long(nullable: false),
                        CabezaUltimaPosicionFecha = c.DateTime(),
                        CabezaUltimaPosicionDireccion = c.String(nullable: false, maxLength: 200),
                        CabezaUltimaPosicionLongitud = c.Double(),
                        CabezaUltimaPosicionLatitud = c.Double(),
                        GpsColaID = c.Long(),
                        ColaUltimaPosicionFecha = c.DateTime(),
                        ColaUltimaPosicionDireccion = c.String(nullable: false, maxLength: 200),
                        ColaUltimaPosicionLongitud = c.Double(),
                        ColaUltimaPosicionLatitud = c.Double(),
                        CalculoVelocidad = c.Double(nullable: false),
                        CalculoDistancia = c.Double(nullable: false),
                        CalculoTiempoHoras = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.RutaID);
            
            CreateTable(
                "dbo.Hermandades",
                c => new
                    {
                        HermandadID = c.Long(nullable: false, identity: true),
                        AplicacionID = c.Long(nullable: false),
                        Nombre = c.String(nullable: false, maxLength: 200),
                        LogotipoDocumentoID = c.Long(),
                        IglesiaNombre = c.String(nullable: false, maxLength: 200),
                        IglesiaDireccion = c.String(nullable: false, maxLength: 500),
                        IglesiaLatitud = c.Double(),
                        IglesiaLongitud = c.Double(),
                        Activo = c.Boolean(nullable: false),
                        Tags = c.String(nullable: false, maxLength: 500),
                    })
                .PrimaryKey(t => t.HermandadID);
            
            CreateTable(
                "dbo.GpsPosiciones",
                c => new
                    {
                        GpsPosicionID = c.Long(nullable: false, identity: true),
                        GpsID = c.Long(nullable: false),
                        LecturaFecha = c.DateTime(nullable: false),
                        PosicionFecha = c.DateTime(nullable: false),
                        Direccion = c.String(nullable: false, maxLength: 200),
                        Latitud = c.Double(nullable: false),
                        Longitud = c.Double(nullable: false),
                        Velocidad = c.Double(nullable: false),
                        Kilometros = c.Double(nullable: false),
                        Manual = c.Boolean(nullable: false),
                        Bateria = c.Double(),
                    })
                .PrimaryKey(t => t.GpsPosicionID);
            
            CreateTable(
                "dbo.Gpss",
                c => new
                    {
                        GpsID = c.Long(nullable: false, identity: true),
                        Api = c.String(nullable: false, maxLength: 100),
                        GpsApiID = c.Long(nullable: false),
                        AplicacionID = c.Long(nullable: false),
                        Activo = c.Boolean(nullable: false),
                        Matricula = c.String(nullable: false, maxLength: 200),
                        Estado = c.String(nullable: false, maxLength: 200),
                        Bateria = c.Double(),
                        Sensores = c.String(nullable: false, maxLength: 200),
                        UltimaLecturaFecha = c.DateTime(),
                        UltimaPosicionFecha = c.DateTime(),
                        UltimaPosicionDireccion = c.String(nullable: false, maxLength: 200),
                        UltimaPosicionLatitud = c.Double(),
                        UltimaPosicionLongitud = c.Double(),
                    })
                .PrimaryKey(t => t.GpsID);
            
            CreateTable(
                "dbo.Logins",
                c => new
                    {
                        ProveedorLogin = c.String(nullable: false, maxLength: 128),
                        ProveedorClave = c.String(nullable: false, maxLength: 128),
                        UsuarioID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProveedorLogin, t.ProveedorClave, t.UsuarioID });
            
            CreateIndex("dbo.AplicacionesAplicacionesAmigas", "AplicacionAmigaID");
            CreateIndex("dbo.AplicacionesAplicacionesAmigas", "AplicacionID");
            CreateIndex("dbo.Rutas", "GpsColaID");
            CreateIndex("dbo.Rutas", "GpsCabezaID");
            CreateIndex("dbo.Rutas", "HermandadID");
            CreateIndex("dbo.Hermandades", "LogotipoDocumentoID");
            CreateIndex("dbo.Hermandades", "AplicacionID");
            CreateIndex("dbo.GpsPosiciones", "GpsID");
            CreateIndex("dbo.Gpss", "AplicacionID");
            CreateIndex("dbo.Logins", "UsuarioID");
            AddForeignKey("dbo.Rutas", "HermandadID", "dbo.Hermandades", "HermandadID");
            AddForeignKey("dbo.Rutas", "GpsColaID", "dbo.Gpss", "GpsID");
            AddForeignKey("dbo.Rutas", "GpsCabezaID", "dbo.Gpss", "GpsID");
            AddForeignKey("dbo.Hermandades", "LogotipoDocumentoID", "dbo.Documentos", "DocumentoID");
            AddForeignKey("dbo.Hermandades", "AplicacionID", "dbo.Aplicaciones", "AplicacionID");
            AddForeignKey("dbo.GpsPosiciones", "GpsID", "dbo.Gpss", "GpsID");
            AddForeignKey("dbo.Gpss", "AplicacionID", "dbo.Aplicaciones", "AplicacionID");
            AddForeignKey("dbo.Logins", "UsuarioID", "dbo.Usuarios", "UsuarioID");
            AddForeignKey("dbo.AplicacionesAplicacionesAmigas", "AplicacionAmigaID", "dbo.Aplicaciones", "AplicacionID");
            AddForeignKey("dbo.AplicacionesAplicacionesAmigas", "AplicacionID", "dbo.Aplicaciones", "AplicacionID");
        }
    }
}
