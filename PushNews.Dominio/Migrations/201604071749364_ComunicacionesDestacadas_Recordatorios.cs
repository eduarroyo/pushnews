namespace BandoApp.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ComunicacionesDestacadas_Recordatorios : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comunicaciones", "Destacado", c => c.Boolean(nullable: false));
            AddColumn("dbo.Comunicaciones", "RecordatorioTitulo", c => c.String(maxLength: 100));
            AddColumn("dbo.Comunicaciones", "RecordatorioFecha", c => c.DateTime());
            AddColumn("dbo.Comunicaciones", "PushRecordatorio", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comunicaciones", "PushRecordatorio");
            DropColumn("dbo.Comunicaciones", "RecordatorioFecha");
            DropColumn("dbo.Comunicaciones", "RecordatorioTitulo");
            DropColumn("dbo.Comunicaciones", "Destacado");
        }
    }
}
