namespace BandoApp.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PublicacionesPrivadas : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categorias", "Privada", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Categorias", "Privada");
        }
    }
}
