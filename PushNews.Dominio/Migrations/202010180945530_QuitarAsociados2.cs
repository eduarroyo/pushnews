namespace PushNews.Dominio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuitarAsociados2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ComunicacionesAccesos", "AsociadoID");
            DropColumn("dbo.Categorias", "Privada");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Categorias", "Privada", c => c.Boolean(nullable: false));
            AddColumn("dbo.ComunicacionesAccesos", "AsociadoID", c => c.Long());
        }
    }
}
