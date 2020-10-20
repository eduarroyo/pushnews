namespace PushNews.Dominio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SimplificarIdentity : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Claims", "UsuarioID", "dbo.Usuarios");
            DropForeignKey("dbo.PerfilesRoles", "PerfilID", "dbo.Perfiles");
            DropForeignKey("dbo.PerfilesRoles", "RolID", "dbo.Roles");
            DropForeignKey("dbo.UsuariosPerfiles", "UsuarioID", "dbo.Usuarios");
            DropForeignKey("dbo.UsuariosPerfiles", "PerfilID", "dbo.Perfiles");
            DropIndex("dbo.Claims", new[] { "UsuarioID" });
            DropIndex("dbo.PerfilesRoles", new[] { "PerfilID" });
            DropIndex("dbo.PerfilesRoles", new[] { "RolID" });
            DropIndex("dbo.UsuariosPerfiles", new[] { "UsuarioID" });
            DropIndex("dbo.UsuariosPerfiles", new[] { "PerfilID" });
            AddColumn("dbo.Usuarios", "RolID", c => c.Long(nullable: false));
            CreateIndex("dbo.Usuarios", "RolID");
            AddForeignKey("dbo.Usuarios", "RolID", "dbo.Roles", "RolID");
            DropColumn("dbo.ComunicacionesAccesos", "AsociadoID");
            DropColumn("dbo.Categorias", "Privada");
            DropColumn("dbo.Roles", "Modulo");
            DropTable("dbo.Claims");
            DropTable("dbo.Perfiles");
            DropTable("dbo.PerfilesRoles");
            DropTable("dbo.UsuariosPerfiles");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UsuariosPerfiles",
                c => new
                    {
                        UsuarioID = c.Long(nullable: false),
                        PerfilID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.UsuarioID, t.PerfilID });
            
            CreateTable(
                "dbo.PerfilesRoles",
                c => new
                    {
                        PerfilID = c.Long(nullable: false),
                        RolID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.PerfilID, t.RolID });
            
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
                "dbo.Claims",
                c => new
                    {
                        ClaimID = c.Long(nullable: false, identity: true),
                        Tipo = c.String(),
                        Valor = c.String(),
                        UsuarioID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ClaimID);
            
            AddColumn("dbo.Roles", "Modulo", c => c.String(maxLength: 50));
            AddColumn("dbo.Categorias", "Privada", c => c.Boolean(nullable: false));
            AddColumn("dbo.ComunicacionesAccesos", "AsociadoID", c => c.Long());
            DropForeignKey("dbo.Usuarios", "RolID", "dbo.Roles");
            DropIndex("dbo.Usuarios", new[] { "RolID" });
            DropColumn("dbo.Usuarios", "RolID");
            CreateIndex("dbo.UsuariosPerfiles", "PerfilID");
            CreateIndex("dbo.UsuariosPerfiles", "UsuarioID");
            CreateIndex("dbo.PerfilesRoles", "RolID");
            CreateIndex("dbo.PerfilesRoles", "PerfilID");
            CreateIndex("dbo.Claims", "UsuarioID");
            AddForeignKey("dbo.UsuariosPerfiles", "PerfilID", "dbo.Perfiles", "PerfilID");
            AddForeignKey("dbo.UsuariosPerfiles", "UsuarioID", "dbo.Usuarios", "UsuarioID");
            AddForeignKey("dbo.PerfilesRoles", "RolID", "dbo.Roles", "RolID");
            AddForeignKey("dbo.PerfilesRoles", "PerfilID", "dbo.Perfiles", "PerfilID");
            AddForeignKey("dbo.Claims", "UsuarioID", "dbo.Usuarios", "UsuarioID");
        }
    }
}
