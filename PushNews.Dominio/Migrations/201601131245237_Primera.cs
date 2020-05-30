namespace PushNews.Dominio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Primera : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ComunicacionesAccesos",
                c => new
                    {
                        ComunicacionAccesoID = c.Long(nullable: false, identity: true),
                        ComunicacionID = c.Long(nullable: false),
                        TerminalID = c.Long(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ComunicacionAccesoID)
                .ForeignKey("dbo.Comunicaciones", t => t.ComunicacionID)
                .ForeignKey("dbo.Terminales", t => t.TerminalID)
                .Index(t => t.ComunicacionID)
                .Index(t => t.TerminalID);
            
            CreateTable(
                "dbo.Comunicaciones",
                c => new
                    {
                        ComunicacionID = c.Long(nullable: false, identity: true),
                        UsuarioID = c.Long(nullable: false),
                        CategoriaID = c.Long(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaPublicacion = c.DateTime(nullable: false),
                        Titulo = c.String(maxLength: 100),
                        Descripcion = c.String(maxLength: 500),
                        Autor = c.String(maxLength: 100),
                        ImagenTitulo = c.String(maxLength: 100),
                        Imagen = c.String(maxLength: 256),
                        AdjuntoTitulo = c.String(maxLength: 100),
                        Adjunto = c.String(maxLength: 256),
                        EnlaceTitulo = c.String(maxLength: 100),
                        Enlace = c.String(maxLength: 256),
                        YoutubeTitulo = c.String(maxLength: 100),
                        Youtube = c.String(maxLength: 256),
                        GeoPosicionTitulo = c.String(),
                        GeoPosicionLatitud = c.Single(nullable: false),
                        GeoPosicionLongitud = c.Single(nullable: false),
                        UltimaEdicionIP = c.String(),
                        Activo = c.Boolean(nullable: false),
                        Borrado = c.Boolean(nullable: false),
                        FechaBorrado = c.DateTime(),
                    })
                .PrimaryKey(t => t.ComunicacionID)
                .ForeignKey("dbo.Categorias", t => t.CategoriaID)
                .ForeignKey("dbo.Usuarios", t => t.UsuarioID)
                .Index(t => t.UsuarioID)
                .Index(t => t.CategoriaID);
            
            CreateTable(
                "dbo.Categorias",
                c => new
                    {
                        CategoriaID = c.Long(nullable: false, identity: true),
                        AplicacionID = c.Long(nullable: false),
                        UsuarioID = c.Long(nullable: false),
                        Nombre = c.String(),
                        Icono = c.String(),
                        Orden = c.Int(nullable: false),
                        Activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CategoriaID)
                .ForeignKey("dbo.Aplicaciones", t => t.AplicacionID)
                .Index(t => t.AplicacionID);
            
            CreateTable(
                "dbo.Usuarios",
                c => new
                    {
                        UsuarioID = c.Long(nullable: false, identity: true),
                        UserName = c.String(maxLength: 50),
                        Nombre = c.String(maxLength: 50),
                        Apellidos = c.String(maxLength: 150),
                        Movil = c.String(maxLength: 50),
                        Email = c.String(maxLength: 250),
                        Clave = c.String(),
                        MarcaSeguridad = c.String(),
                        Activo = c.Boolean(nullable: false),
                        EmailConfirmado = c.Boolean(nullable: false),
                        MovilConfirmado = c.Boolean(nullable: false),
                        AccesosFallidos = c.Int(nullable: false),
                        Externo = c.Boolean(nullable: false),
                        Creado = c.DateTime(nullable: false),
                        Actualizado = c.DateTime(nullable: false),
                        FinalBloqueoUtc = c.DateTime(),
                        DosFactoresHabilitado = c.Boolean(nullable: false),
                        ProveedorID = c.Long(),
                        UltimoLogin = c.DateTime(),
                        Locale = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.UsuarioID);
            
            CreateTable(
                "dbo.Aplicaciones",
                c => new
                    {
                        AplicacionID = c.Long(nullable: false, identity: true),
                        Nombre = c.String(maxLength: 100),
                        Version = c.String(maxLength: 100),
                        Activo = c.Boolean(nullable: false),
                        CloudKey = c.String(maxLength: 100),
                        SubDominio = c.String(maxLength: 50),
                        Usuario = c.String(maxLength: 100),
                        Clave = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.AplicacionID);
            
            CreateTable(
                "dbo.Parametros",
                c => new
                    {
                        ParametroID = c.Long(nullable: false, identity: true),
                        AplicacionID = c.Long(),
                        Nombre = c.String(maxLength: 100),
                        Valor = c.String(maxLength: 500),
                        Descripcion = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.ParametroID)
                .ForeignKey("dbo.Aplicaciones", t => t.AplicacionID)
                .Index(t => t.AplicacionID);
            
            CreateTable(
                "dbo.Terminales",
                c => new
                    {
                        TerminalID = c.Long(nullable: false, identity: true),
                        AplicacionID = c.Long(nullable: false),
                        Nombre = c.String(maxLength: 100),
                        UltimaConexionFecha = c.DateTime(nullable: false),
                        UltimaConexionIP = c.String(maxLength: 40),
                    })
                .PrimaryKey(t => t.TerminalID)
                .ForeignKey("dbo.Aplicaciones", t => t.AplicacionID)
                .Index(t => t.AplicacionID);
            
            CreateTable(
                "dbo.Claims",
                c => new
                    {
                        ClaimID = c.Long(nullable: false, identity: true),
                        Tipo = c.String(),
                        Valor = c.String(),
                        UsuarioID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ClaimID)
                .ForeignKey("dbo.Usuarios", t => t.UsuarioID)
                .Index(t => t.UsuarioID);
            
            CreateTable(
                "dbo.Logins",
                c => new
                    {
                        ProveedorLogin = c.String(nullable: false, maxLength: 128),
                        ProveedorClave = c.String(nullable: false, maxLength: 128),
                        UsuarioID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProveedorLogin, t.ProveedorClave, t.UsuarioID })
                .ForeignKey("dbo.Usuarios", t => t.UsuarioID)
                .Index(t => t.UsuarioID);
            
            CreateTable(
                "dbo.Perfiles",
                c => new
                    {
                        PerfilID = c.Long(nullable: false, identity: true),
                        Nombre = c.String(maxLength: 50),
                        Activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PerfilID);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RolID = c.Long(nullable: false, identity: true),
                        Modulo = c.String(maxLength: 50),
                        Nombre = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.RolID);
            
            CreateTable(
                "dbo.Documentos",
                c => new
                    {
                        DocumentoID = c.Long(nullable: false, identity: true),
                        Nombre = c.String(maxLength: 250),
                        Ruta = c.String(),
                        Mime = c.String(maxLength: 200),
                        Tamano = c.Long(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        DocumentoTipoID = c.Long(nullable: false),
                        ClienteID = c.Long(),
                        PacienteID = c.Long(),
                        ProductoID = c.Long(),
                        ConsultaID = c.Long(),
                        PruebaID = c.Long(),
                    })
                .PrimaryKey(t => t.DocumentoID)
                .ForeignKey("dbo.DocumentosTipos", t => t.DocumentoTipoID)
                .Index(t => t.DocumentoTipoID);
            
            CreateTable(
                "dbo.DocumentosTipos",
                c => new
                    {
                        DocumentoTipoID = c.Long(nullable: false, identity: true),
                        Nombre = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.DocumentoTipoID);
            
            CreateTable(
                "dbo.Localidades",
                c => new
                    {
                        LocalidadID = c.Long(nullable: false, identity: true),
                        ProvinciaID = c.Long(nullable: false),
                        Nombre = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.LocalidadID)
                .ForeignKey("dbo.Provincias", t => t.ProvinciaID)
                .Index(t => t.ProvinciaID);
            
            CreateTable(
                "dbo.Provincias",
                c => new
                    {
                        ProvinciaID = c.Long(nullable: false, identity: true),
                        PaisID = c.Long(nullable: false),
                        Nombre = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.ProvinciaID)
                .ForeignKey("dbo.Paises", t => t.PaisID)
                .Index(t => t.PaisID);
            
            CreateTable(
                "dbo.Paises",
                c => new
                    {
                        PaisID = c.Long(nullable: false, identity: true),
                        Nombre = c.String(maxLength: 100),
                        Locale = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.PaisID);
            
            CreateTable(
                "dbo.AplicacionesUsuarios",
                c => new
                    {
                        UsuarioID = c.Long(nullable: false),
                        AplicacionID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.UsuarioID, t.AplicacionID })
                .ForeignKey("dbo.Usuarios", t => t.UsuarioID)
                .ForeignKey("dbo.Aplicaciones", t => t.AplicacionID)
                .Index(t => t.UsuarioID)
                .Index(t => t.AplicacionID);
            
            CreateTable(
                "dbo.PerfilesRoles",
                c => new
                    {
                        PerfilID = c.Long(nullable: false),
                        RolID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.PerfilID, t.RolID })
                .ForeignKey("dbo.Perfiles", t => t.PerfilID)
                .ForeignKey("dbo.Roles", t => t.RolID)
                .Index(t => t.PerfilID)
                .Index(t => t.RolID);
            
            CreateTable(
                "dbo.UsuariosPerfiles",
                c => new
                    {
                        UsuarioID = c.Long(nullable: false),
                        PerfilID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.UsuarioID, t.PerfilID })
                .ForeignKey("dbo.Usuarios", t => t.UsuarioID)
                .ForeignKey("dbo.Perfiles", t => t.PerfilID)
                .Index(t => t.UsuarioID)
                .Index(t => t.PerfilID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Localidades", "ProvinciaID", "dbo.Provincias");
            DropForeignKey("dbo.Provincias", "PaisID", "dbo.Paises");
            DropForeignKey("dbo.Documentos", "DocumentoTipoID", "dbo.DocumentosTipos");
            DropForeignKey("dbo.ComunicacionesAccesos", "TerminalID", "dbo.Terminales");
            DropForeignKey("dbo.Comunicaciones", "UsuarioID", "dbo.Usuarios");
            DropForeignKey("dbo.UsuariosPerfiles", "PerfilID", "dbo.Perfiles");
            DropForeignKey("dbo.UsuariosPerfiles", "UsuarioID", "dbo.Usuarios");
            DropForeignKey("dbo.PerfilesRoles", "RolID", "dbo.Roles");
            DropForeignKey("dbo.PerfilesRoles", "PerfilID", "dbo.Perfiles");
            DropForeignKey("dbo.Logins", "UsuarioID", "dbo.Usuarios");
            DropForeignKey("dbo.Claims", "UsuarioID", "dbo.Usuarios");
            DropForeignKey("dbo.AplicacionesUsuarios", "AplicacionID", "dbo.Aplicaciones");
            DropForeignKey("dbo.AplicacionesUsuarios", "UsuarioID", "dbo.Usuarios");
            DropForeignKey("dbo.Terminales", "AplicacionID", "dbo.Aplicaciones");
            DropForeignKey("dbo.Parametros", "AplicacionID", "dbo.Aplicaciones");
            DropForeignKey("dbo.Categorias", "AplicacionID", "dbo.Aplicaciones");
            DropForeignKey("dbo.Comunicaciones", "CategoriaID", "dbo.Categorias");
            DropForeignKey("dbo.ComunicacionesAccesos", "ComunicacionID", "dbo.Comunicaciones");
            DropIndex("dbo.UsuariosPerfiles", new[] { "PerfilID" });
            DropIndex("dbo.UsuariosPerfiles", new[] { "UsuarioID" });
            DropIndex("dbo.PerfilesRoles", new[] { "RolID" });
            DropIndex("dbo.PerfilesRoles", new[] { "PerfilID" });
            DropIndex("dbo.AplicacionesUsuarios", new[] { "AplicacionID" });
            DropIndex("dbo.AplicacionesUsuarios", new[] { "UsuarioID" });
            DropIndex("dbo.Provincias", new[] { "PaisID" });
            DropIndex("dbo.Localidades", new[] { "ProvinciaID" });
            DropIndex("dbo.Documentos", new[] { "DocumentoTipoID" });
            DropIndex("dbo.Logins", new[] { "UsuarioID" });
            DropIndex("dbo.Claims", new[] { "UsuarioID" });
            DropIndex("dbo.Terminales", new[] { "AplicacionID" });
            DropIndex("dbo.Parametros", new[] { "AplicacionID" });
            DropIndex("dbo.Categorias", new[] { "AplicacionID" });
            DropIndex("dbo.Comunicaciones", new[] { "CategoriaID" });
            DropIndex("dbo.Comunicaciones", new[] { "UsuarioID" });
            DropIndex("dbo.ComunicacionesAccesos", new[] { "TerminalID" });
            DropIndex("dbo.ComunicacionesAccesos", new[] { "ComunicacionID" });
            DropTable("dbo.UsuariosPerfiles");
            DropTable("dbo.PerfilesRoles");
            DropTable("dbo.AplicacionesUsuarios");
            DropTable("dbo.Paises");
            DropTable("dbo.Provincias");
            DropTable("dbo.Localidades");
            DropTable("dbo.DocumentosTipos");
            DropTable("dbo.Documentos");
            DropTable("dbo.Roles");
            DropTable("dbo.Perfiles");
            DropTable("dbo.Logins");
            DropTable("dbo.Claims");
            DropTable("dbo.Terminales");
            DropTable("dbo.Parametros");
            DropTable("dbo.Aplicaciones");
            DropTable("dbo.Usuarios");
            DropTable("dbo.Categorias");
            DropTable("dbo.Comunicaciones");
            DropTable("dbo.ComunicacionesAccesos");
        }
    }
}
