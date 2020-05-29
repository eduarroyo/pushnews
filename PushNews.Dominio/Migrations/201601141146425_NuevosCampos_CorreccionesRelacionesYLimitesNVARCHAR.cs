namespace BandoApp.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NuevosCampos_CorreccionesRelacionesYLimitesNVARCHAR : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Aplicaciones", "ClaveSuscripcion", c => c.String(maxLength: 100));
            AddColumn("dbo.Aplicaciones", "RequerirClaveSuscripcion", c => c.Boolean(nullable: false));
            AddColumn("dbo.Documentos", "ComunicacionID", c => c.Long(nullable: false));
            AlterColumn("dbo.Comunicaciones", "GeoPosicionTitulo", c => c.String(maxLength: 100));
            AlterColumn("dbo.Comunicaciones", "UltimaEdicionIP", c => c.String(maxLength: 40));
            AlterColumn("dbo.Categorias", "Nombre", c => c.String(maxLength: 100));
            AlterColumn("dbo.Categorias", "Icono", c => c.String(maxLength: 256));
            AlterColumn("dbo.Aplicaciones", "SubDominio", c => c.String());
            CreateIndex("dbo.Categorias", "UsuarioID");
            CreateIndex("dbo.Documentos", "ComunicacionID");
            AddForeignKey("dbo.Categorias", "UsuarioID", "dbo.Usuarios", "UsuarioID");
            AddForeignKey("dbo.Documentos", "ComunicacionID", "dbo.Comunicaciones", "ComunicacionID");
            DropColumn("dbo.Documentos", "ClienteID");
            DropColumn("dbo.Documentos", "PacienteID");
            DropColumn("dbo.Documentos", "ProductoID");
            DropColumn("dbo.Documentos", "ConsultaID");
            DropColumn("dbo.Documentos", "PruebaID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Documentos", "PruebaID", c => c.Long());
            AddColumn("dbo.Documentos", "ConsultaID", c => c.Long());
            AddColumn("dbo.Documentos", "ProductoID", c => c.Long());
            AddColumn("dbo.Documentos", "PacienteID", c => c.Long());
            AddColumn("dbo.Documentos", "ClienteID", c => c.Long());
            DropForeignKey("dbo.Documentos", "ComunicacionID", "dbo.Comunicaciones");
            DropForeignKey("dbo.Categorias", "UsuarioID", "dbo.Usuarios");
            DropIndex("dbo.Documentos", new[] { "ComunicacionID" });
            DropIndex("dbo.Categorias", new[] { "UsuarioID" });
            AlterColumn("dbo.Aplicaciones", "SubDominio", c => c.String(maxLength: 50));
            AlterColumn("dbo.Categorias", "Icono", c => c.String());
            AlterColumn("dbo.Categorias", "Nombre", c => c.String());
            AlterColumn("dbo.Comunicaciones", "UltimaEdicionIP", c => c.String());
            AlterColumn("dbo.Comunicaciones", "GeoPosicionTitulo", c => c.String());
            DropColumn("dbo.Documentos", "ComunicacionID");
            DropColumn("dbo.Aplicaciones", "RequerirClaveSuscripcion");
            DropColumn("dbo.Aplicaciones", "ClaveSuscripcion");
        }
    }
}
