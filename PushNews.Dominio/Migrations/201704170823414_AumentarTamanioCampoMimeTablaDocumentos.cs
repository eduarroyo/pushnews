namespace BandoApp.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AumentarTamanioCampoMimeTablaDocumentos : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Documentos", "Mime", c => c.String(maxLength: 300));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Documentos", "Mime", c => c.String(maxLength: 50));
        }
    }
}
