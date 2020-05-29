namespace BandoApp.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmpresasBanner : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Empresas", "BannerDocumentoID", c => c.Long());
            CreateIndex("dbo.Empresas", "BannerDocumentoID");
            AddForeignKey("dbo.Empresas", "BannerDocumentoID", "dbo.Documentos", "DocumentoID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Empresas", "BannerDocumentoID", "dbo.Documentos");
            DropIndex("dbo.Empresas", new[] { "BannerDocumentoID" });
            DropColumn("dbo.Empresas", "BannerDocumentoID");
        }
    }
}
