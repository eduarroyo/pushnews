namespace PushNews.Dominio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuitarTipoAplicacion : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Aplicaciones", "Tipo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Aplicaciones", "Tipo", c => c.String(maxLength: 100));
        }
    }
}
