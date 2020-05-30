namespace PushNews.Dominio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CamposIdentityClaseAsociado_AsociadoBoolEliminado_AplicacionBoolActivarApiExternos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Aplicaciones", "PermitirAccesoApiExternos", c => c.Boolean(nullable: false));
            AddColumn("dbo.Asociados", "Eliminado", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Asociados", "Eliminado");
            DropColumn("dbo.Aplicaciones", "PermitirAccesoApiExternos");
        }
    }
}
