namespace BandoApp.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HermandadesTags : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Hermandades", "Tags", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Hermandades", "Tags");
        }
    }
}
