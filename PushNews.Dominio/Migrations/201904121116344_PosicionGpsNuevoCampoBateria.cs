namespace BandoApp.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PosicionGpsNuevoCampoBateria : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GpsPosiciones", "Bateria", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GpsPosiciones", "Bateria");
        }
    }
}
