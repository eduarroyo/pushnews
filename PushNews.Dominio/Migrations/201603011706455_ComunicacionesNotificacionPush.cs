namespace PushNews.Dominio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ComunicacionesNotificacionPush : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comunicaciones", "PushEnviada", c => c.Boolean(nullable: false));
            AddColumn("dbo.Comunicaciones", "PushFecha", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comunicaciones", "PushFecha");
            DropColumn("dbo.Comunicaciones", "PushEnviada");
        }
    }
}
