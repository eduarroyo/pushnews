namespace PushNews.Dominio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuitarRelacionUsuarioCategoria : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UsuariosCategorias", "UsuarioID", "dbo.Usuarios");
            DropForeignKey("dbo.UsuariosCategorias", "CategoriaID", "dbo.Categorias");
            DropForeignKey("dbo.Categorias", "UsuarioID", "dbo.Usuarios");
            DropIndex("dbo.Categorias", new[] { "UsuarioID" });
            DropIndex("dbo.UsuariosCategorias", new[] { "UsuarioID" });
            DropIndex("dbo.UsuariosCategorias", new[] { "CategoriaID" });
            DropColumn("dbo.Categorias", "UsuarioID");
            DropTable("dbo.UsuariosCategorias");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UsuariosCategorias",
                c => new
                    {
                        UsuarioID = c.Long(nullable: false),
                        CategoriaID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.UsuarioID, t.CategoriaID });
            
            AddColumn("dbo.Categorias", "UsuarioID", c => c.Long(nullable: false));
            CreateIndex("dbo.UsuariosCategorias", "CategoriaID");
            CreateIndex("dbo.UsuariosCategorias", "UsuarioID");
            CreateIndex("dbo.Categorias", "UsuarioID");
            AddForeignKey("dbo.Categorias", "UsuarioID", "dbo.Usuarios", "UsuarioID");
            AddForeignKey("dbo.UsuariosCategorias", "CategoriaID", "dbo.Categorias", "CategoriaID");
            AddForeignKey("dbo.UsuariosCategorias", "UsuarioID", "dbo.Usuarios", "UsuarioID");
        }
    }
}
