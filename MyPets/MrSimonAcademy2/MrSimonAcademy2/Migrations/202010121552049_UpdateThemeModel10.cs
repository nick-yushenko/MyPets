namespace MrSimonAcademy2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateThemeModel10 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Assignments", new[] { "Theme_Id" });
            AddColumn("dbo.Assignments", "link", c => c.String());
            CreateIndex("dbo.Assignments", "theme_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Assignments", new[] { "theme_Id" });
            DropColumn("dbo.Assignments", "link");
            CreateIndex("dbo.Assignments", "Theme_Id");
        }
    }
}
