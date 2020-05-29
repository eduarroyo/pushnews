namespace BandoApp.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GpsPosicion_nuevoCampoDireccion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GpsPosiciones", "Direccion", c => c.String(nullable: false, maxLength: 200, defaultValue: ""));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GpsPosiciones", "Direccion");
        }
    }
}
