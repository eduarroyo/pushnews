namespace PushNews.Dominio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuitarRegionesYDocumentos_AniadirAplicacionesAmigas : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Documentos", "ComunicacionID", "dbo.Comunicaciones");
            DropForeignKey("dbo.Documentos", "DocumentoTipoID", "dbo.DocumentosTipos");
            DropForeignKey("dbo.Provincias", "PaisID", "dbo.Paises");
            DropForeignKey("dbo.Localidades", "ProvinciaID", "dbo.Provincias");
            DropIndex("dbo.Documentos", new[] { "DocumentoTipoID" });
            DropIndex("dbo.Documentos", new[] { "ComunicacionID" });
            DropIndex("dbo.Localidades", new[] { "ProvinciaID" });
            DropIndex("dbo.Provincias", new[] { "PaisID" });
            CreateTable(
                "dbo.AplicacionesAplicacionesAmigas",
                c => new
                    {
                        AplicacionID = c.Long(nullable: false),
                        AplicacionAmigaID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.AplicacionID, t.AplicacionAmigaID })
                .ForeignKey("dbo.Aplicaciones", t => t.AplicacionID)
                .ForeignKey("dbo.Aplicaciones", t => t.AplicacionAmigaID)
                .Index(t => t.AplicacionID)
                .Index(t => t.AplicacionAmigaID);
            
            DropTable("dbo.Documentos");
            DropTable("dbo.DocumentosTipos");
            DropTable("dbo.Localidades");
            DropTable("dbo.Provincias");
            DropTable("dbo.Paises");
        }
        
        public override void Down()
        {
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
                "dbo.Provincias",
                c => new
                    {
                        ProvinciaID = c.Long(nullable: false, identity: true),
                        PaisID = c.Long(nullable: false),
                        Nombre = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.ProvinciaID);
            
            CreateTable(
                "dbo.Localidades",
                c => new
                    {
                        LocalidadID = c.Long(nullable: false, identity: true),
                        ProvinciaID = c.Long(nullable: false),
                        Nombre = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.LocalidadID);
            
            CreateTable(
                "dbo.DocumentosTipos",
                c => new
                    {
                        DocumentoTipoID = c.Long(nullable: false, identity: true),
                        Nombre = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.DocumentoTipoID);
            
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
                        ComunicacionID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.DocumentoID);
            
            DropForeignKey("dbo.AplicacionesAplicacionesAmigas", "AplicacionAmigaID", "dbo.Aplicaciones");
            DropForeignKey("dbo.AplicacionesAplicacionesAmigas", "AplicacionID", "dbo.Aplicaciones");
            DropIndex("dbo.AplicacionesAplicacionesAmigas", new[] { "AplicacionAmigaID" });
            DropIndex("dbo.AplicacionesAplicacionesAmigas", new[] { "AplicacionID" });
            DropTable("dbo.AplicacionesAplicacionesAmigas");
            CreateIndex("dbo.Provincias", "PaisID");
            CreateIndex("dbo.Localidades", "ProvinciaID");
            CreateIndex("dbo.Documentos", "ComunicacionID");
            CreateIndex("dbo.Documentos", "DocumentoTipoID");
            AddForeignKey("dbo.Localidades", "ProvinciaID", "dbo.Provincias", "ProvinciaID");
            AddForeignKey("dbo.Provincias", "PaisID", "dbo.Paises", "PaisID");
            AddForeignKey("dbo.Documentos", "DocumentoTipoID", "dbo.DocumentosTipos", "DocumentoTipoID");
            AddForeignKey("dbo.Documentos", "ComunicacionID", "dbo.Comunicaciones", "ComunicacionID");
        }
    }
}
