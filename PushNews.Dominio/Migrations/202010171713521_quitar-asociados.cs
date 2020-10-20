namespace PushNews.Dominio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class quitarasociados : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Asociados", "AplicacionID", "dbo.Aplicaciones");
            DropForeignKey("dbo.ComunicacionesAccesos", "AsociadoID", "dbo.Asociados");
            DropIndex("dbo.ComunicacionesAccesos", new[] { "AsociadoID" });
            DropIndex("dbo.Asociados", new[] { "AplicacionID" });
            DropColumn("dbo.Aplicaciones", "ClaveSuscripcion");
            DropColumn("dbo.Aplicaciones", "RequerirClaveSuscripcion");
            DropColumn("dbo.Aplicaciones", "ApiKeyExternos");
            DropColumn("dbo.Aplicaciones", "PermitirAccesoApiExternos");
            DropTable("dbo.Asociados");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Asociados",
                c => new
                    {
                        AsociadoID = c.Long(nullable: false, identity: true),
                        AplicacionID = c.Long(nullable: false),
                        Codigo = c.String(maxLength: 100),
                        Clave = c.String(),
                        Nombre = c.String(maxLength: 100),
                        Apellidos = c.String(maxLength: 100),
                        Direccion = c.String(maxLength: 500),
                        Localidad = c.String(maxLength: 100),
                        CodigoPostal = c.String(maxLength: 10),
                        Provincia = c.String(maxLength: 100),
                        Telefono = c.String(maxLength: 20),
                        Email = c.String(maxLength: 250),
                        Latitud = c.Double(),
                        Longitud = c.Double(),
                        Observaciones = c.String(maxLength: 500),
                        Activo = c.Boolean(nullable: false),
                        Eliminado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AsociadoID);
            
            AddColumn("dbo.Aplicaciones", "PermitirAccesoApiExternos", c => c.Boolean(nullable: false));
            AddColumn("dbo.Aplicaciones", "ApiKeyExternos", c => c.String(maxLength: 500));
            AddColumn("dbo.Aplicaciones", "RequerirClaveSuscripcion", c => c.Boolean(nullable: false));
            AddColumn("dbo.Aplicaciones", "ClaveSuscripcion", c => c.String(maxLength: 100));
            CreateIndex("dbo.Asociados", "AplicacionID");
            CreateIndex("dbo.ComunicacionesAccesos", "AsociadoID");
            AddForeignKey("dbo.ComunicacionesAccesos", "AsociadoID", "dbo.Asociados", "AsociadoID");
            AddForeignKey("dbo.Asociados", "AplicacionID", "dbo.Aplicaciones", "AplicacionID");
        }
    }
}
