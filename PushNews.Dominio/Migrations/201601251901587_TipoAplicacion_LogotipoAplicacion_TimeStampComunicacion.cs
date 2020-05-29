namespace BandoApp.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TipoAplicacion_LogotipoAplicacion_TimeStampComunicacion : DbMigration
    {
        public override void Up()
        {
            // Añade la columna no nulable TimeStamp. El valor asignado es igual a los segundos transcurridos
            // entre el día 1/1/1970 y el instante actual.
            long unixTimeStamp = (long) Math.Round((DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds, 0);
            AddColumn("dbo.Comunicaciones", "TimeStamp", 
                c => c.Long(nullable: false, defaultValue: unixTimeStamp));
            AddColumn("dbo.Aplicaciones", "LogotipoID", c => c.Long());
            AddColumn("dbo.Aplicaciones", "Tipo", c => c.String(maxLength: 100));
            CreateIndex("dbo.Aplicaciones", "LogotipoID");
            AddForeignKey("dbo.Aplicaciones", "LogotipoID", "dbo.Documentos", "DocumentoID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Aplicaciones", "LogotipoID", "dbo.Documentos");
            DropIndex("dbo.Aplicaciones", new[] { "LogotipoID" });
            DropColumn("dbo.Aplicaciones", "Tipo");
            DropColumn("dbo.Aplicaciones", "LogotipoID");
            DropColumn("dbo.Comunicaciones", "TimeStamp");
        }
    }
}
