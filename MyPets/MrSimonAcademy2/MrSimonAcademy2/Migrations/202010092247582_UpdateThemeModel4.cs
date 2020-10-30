namespace MrSimonAcademy2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateThemeModel4 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Assignments", new[] { "Theme_id" });
            CreateIndex("dbo.Assignments", "Theme_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Assignments", new[] { "Theme_Id" });
            CreateIndex("dbo.Assignments", "Theme_id");
        }
    }
}
