namespace BandoApp.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActualizacionUsuarios : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Usuarios", "UserName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Usuarios", "UserName", c => c.String(maxLength: 50));
        }
    }
}
