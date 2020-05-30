namespace PushNews.Dominio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuitarLongitudMaximaDescripcionComunicaciones : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Comunicaciones", "Descripcion", c => c.String());
        }
        
        public override void Down()
        {
            Sql("UPDATE dbo.Comunicaciones SET Descripcion = RIGHT(Descripcion, 500)");
            AlterColumn("dbo.Comunicaciones", "Descripcion", c => c.String(maxLength: 500));
        }
    }
}
