namespace PushNews.Dominio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Empresas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Empresas",
                c => new
                    {
                        EmpresaID = c.Long(nullable: false, identity: true),
                        AplicacionID = c.Long(nullable: false),
                        Nombre = c.String(nullable: false, maxLength: 100, defaultValue: ""),
                        Direccion = c.String(nullable: false, maxLength: 500, defaultValue: ""),
                        Localidad = c.String(nullable: false, maxLength: 100, defaultValue: ""),
                        CodigoPostal = c.String(nullable: false, maxLength: 10, defaultValue: ""),
                        Provincia = c.String(nullable: false, maxLength: 100, defaultValue: ""),
                        Latitud = c.Double(),
                        Longitud = c.Double(),
                        Telefono = c.String(nullable: false, maxLength: 20, defaultValue: ""),
                        Email = c.String(nullable: false, maxLength: 250, defaultValue: ""),
                        Web = c.String(nullable: false, maxLength: 500, defaultValue: ""),
                        Facebook = c.String(nullable: false, maxLength: 500, defaultValue: ""),
                        Twitter = c.String(nullable: false, maxLength: 500, defaultValue: ""),
                        LogotipoDocumentoID = c.Long(),
                        Descripcion = c.String(nullable: false, maxLength: 500, defaultValue: ""),
                        Tags = c.String(nullable: false, maxLength: 500, defaultValue: ""),
                    })
                .PrimaryKey(t => t.EmpresaID)
                .ForeignKey("dbo.Aplicaciones", t => t.AplicacionID)
                .ForeignKey("dbo.Documentos", t => t.LogotipoDocumentoID)
                .Index(t => t.AplicacionID)
                .Index(t => t.LogotipoDocumentoID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Empresas", "LogotipoDocumentoID", "dbo.Documentos");
            DropForeignKey("dbo.Empresas", "AplicacionID", "dbo.Aplicaciones");
            DropIndex("dbo.Empresas", new[] { "LogotipoDocumentoID" });
            DropIndex("dbo.Empresas", new[] { "AplicacionID" });
            DropTable("dbo.Empresas");
        }
    }
}
