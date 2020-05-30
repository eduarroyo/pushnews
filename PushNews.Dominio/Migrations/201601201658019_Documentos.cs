namespace PushNews.Dominio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Documentos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Documentos",
                c => new
                    {
                        DocumentoID = c.Long(nullable: false, identity: true),
                        AplicacionID = c.Long(nullable: false),
                        Tipo = c.Int(nullable: false),
                        Nombre = c.String(maxLength: 50),
                        Ruta = c.String(maxLength: 256),
                        Mime = c.String(maxLength: 50),
                        Tamano = c.Long(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DocumentoID)
                .ForeignKey("dbo.Aplicaciones", t => t.AplicacionID)
                .Index(t => t.AplicacionID);
            
            AddColumn("dbo.Comunicaciones", "ImagenDocumentoID", c => c.Long());
            AddColumn("dbo.Comunicaciones", "AdjuntoDocumentoID", c => c.Long());
            CreateIndex("dbo.Comunicaciones", "ImagenDocumentoID");
            CreateIndex("dbo.Comunicaciones", "AdjuntoDocumentoID");
            AddForeignKey("dbo.Comunicaciones", "AdjuntoDocumentoID", "dbo.Documentos", "DocumentoID");
            AddForeignKey("dbo.Comunicaciones", "ImagenDocumentoID", "dbo.Documentos", "DocumentoID");
            DropColumn("dbo.Comunicaciones", "Imagen");
            DropColumn("dbo.Comunicaciones", "Adjunto");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comunicaciones", "Adjunto", c => c.String(maxLength: 256));
            AddColumn("dbo.Comunicaciones", "Imagen", c => c.String(maxLength: 256));
            DropForeignKey("dbo.Comunicaciones", "ImagenDocumentoID", "dbo.Documentos");
            DropForeignKey("dbo.Comunicaciones", "AdjuntoDocumentoID", "dbo.Documentos");
            DropForeignKey("dbo.Documentos", "AplicacionID", "dbo.Aplicaciones");
            DropIndex("dbo.Documentos", new[] { "AplicacionID" });
            DropIndex("dbo.Comunicaciones", new[] { "AdjuntoDocumentoID" });
            DropIndex("dbo.Comunicaciones", new[] { "ImagenDocumentoID" });
            DropColumn("dbo.Comunicaciones", "AdjuntoDocumentoID");
            DropColumn("dbo.Comunicaciones", "ImagenDocumentoID");
            DropTable("dbo.Documentos");
        }
    }
}
