namespace BandoApp.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EstadisticasPorAsociado : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ComunicacionesAccesos", "AsociadoID", c => c.Long());
            CreateIndex("dbo.ComunicacionesAccesos", "AsociadoID");
            AddForeignKey("dbo.ComunicacionesAccesos", "AsociadoID", "dbo.Asociados", "AsociadoID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ComunicacionesAccesos", "AsociadoID", "dbo.Asociados");
            DropIndex("dbo.ComunicacionesAccesos", new[] { "AsociadoID" });
            DropColumn("dbo.ComunicacionesAccesos", "AsociadoID");
        }
    }
}
