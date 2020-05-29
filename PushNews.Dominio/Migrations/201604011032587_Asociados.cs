namespace BandoApp.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Asociados : DbMigration
    {
        public override void Up()
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
                    })
                .PrimaryKey(t => t.AsociadoID)
                .ForeignKey("dbo.Aplicaciones", t => t.AplicacionID)
                .Index(t => t.AplicacionID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Asociados", "AplicacionID", "dbo.Aplicaciones");
            DropIndex("dbo.Asociados", new[] { "AplicacionID" });
            DropTable("dbo.Asociados");
        }
    }
}
