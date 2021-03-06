namespace PushNews.Dominio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EliminarAplicacionesCaracteristicasProvisional : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AplicacionesAplicacionesCaracteristicas", "AplicacionID", "dbo.Aplicaciones");
            DropForeignKey("dbo.AplicacionesAplicacionesCaracteristicas", "AplicacionCaracteristicaID", "dbo.AplicacionesCaracteristicas");
            DropIndex("dbo.AplicacionesAplicacionesCaracteristicas", new[] { "AplicacionID" });
            DropIndex("dbo.AplicacionesAplicacionesCaracteristicas", new[] { "AplicacionCaracteristicaID" });
            DropTable("dbo.AplicacionesAplicacionesCaracteristicas");
            DropTable("dbo.AplicacionesCaracteristicas");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AplicacionesCaracteristicas",
                c => new
                {
                    AplicacionCaracteristicaID = c.Long(nullable: false, identity: true),
                    Nombre = c.String(maxLength: 100),
                })
                .PrimaryKey(t => t.AplicacionCaracteristicaID);

            CreateTable(
                "dbo.AplicacionesAplicacionesCaracteristicas",
                c => new
                {
                    AplicacionCaracteristicaID = c.Long(nullable: false),
                    AplicacionID = c.Long(nullable: false),
                })
                .PrimaryKey(t => new { t.AplicacionCaracteristicaID, t.AplicacionID })
                .ForeignKey("dbo.AplicacionesCaracteristicas", t => t.AplicacionCaracteristicaID)
                .ForeignKey("dbo.Aplicaciones", t => t.AplicacionID)
                .Index(t => t.AplicacionCaracteristicaID)
                .Index(t => t.AplicacionID);
        }
    }
}
