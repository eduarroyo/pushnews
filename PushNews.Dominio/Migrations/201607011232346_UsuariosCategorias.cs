namespace PushNews.Dominio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsuariosCategorias : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UsuariosCategorias",
                c => new
                    {
                        UsuarioID = c.Long(nullable: false),
                        CategoriaID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.UsuarioID, t.CategoriaID })
                .ForeignKey("dbo.Usuarios", t => t.UsuarioID)
                .ForeignKey("dbo.Categorias", t => t.CategoriaID)
                .Index(t => t.UsuarioID)
                .Index(t => t.CategoriaID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UsuariosCategorias", "CategoriaID", "dbo.Categorias");
            DropForeignKey("dbo.UsuariosCategorias", "UsuarioID", "dbo.Usuarios");
            DropIndex("dbo.UsuariosCategorias", new[] { "CategoriaID" });
            DropIndex("dbo.UsuariosCategorias", new[] { "UsuarioID" });
            DropTable("dbo.UsuariosCategorias");
        }
    }
}
