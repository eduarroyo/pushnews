namespace PushNews.Dominio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TelefonosLocalizaciones : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Localizaciones",
                c => new
                    {
                        LocalizacionID = c.Long(nullable: false, identity: true),
                        AplicacionID = c.Long(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        Longitud = c.Double(nullable: false),
                        Latitud = c.Double(nullable: false),
                        Descripcion = c.String(maxLength: 256),
                        Activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.LocalizacionID)
                .ForeignKey("dbo.Aplicaciones", t => t.AplicacionID)
                .Index(t => t.AplicacionID);
            
            CreateTable(
                "dbo.Telefonos",
                c => new
                    {
                        TelefonoID = c.Long(nullable: false, identity: true),
                        AplicacionID = c.Long(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        Numero = c.String(maxLength: 100),
                        Descripcion = c.String(maxLength: 256),
                        Activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TelefonoID)
                .ForeignKey("dbo.Aplicaciones", t => t.AplicacionID)
                .Index(t => t.AplicacionID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Telefonos", "AplicacionID", "dbo.Aplicaciones");
            DropForeignKey("dbo.Localizaciones", "AplicacionID", "dbo.Aplicaciones");
            DropIndex("dbo.Telefonos", new[] { "AplicacionID" });
            DropIndex("dbo.Localizaciones", new[] { "AplicacionID" });
            DropTable("dbo.Telefonos");
            DropTable("dbo.Localizaciones");
        }
    }
}
