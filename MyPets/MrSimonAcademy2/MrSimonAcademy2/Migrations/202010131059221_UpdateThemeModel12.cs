namespace MrSimonAcademy2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateThemeModel12 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Assignments", "theme_Id", "dbo.Themes");
            DropIndex("dbo.Assignments", new[] { "theme_Id" });
            RenameColumn(table: "dbo.Assignments", name: "theme_Id", newName: "themeId");
            AlterColumn("dbo.Assignments", "themeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Assignments", "themeId");
            AddForeignKey("dbo.Assignments", "themeId", "dbo.Themes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Assignments", "themeId", "dbo.Themes");
            DropIndex("dbo.Assignments", new[] { "themeId" });
            AlterColumn("dbo.Assignments", "themeId", c => c.Int());
            RenameColumn(table: "dbo.Assignments", name: "themeId", newName: "theme_Id");
            CreateIndex("dbo.Assignments", "theme_Id");
            AddForeignKey("dbo.Assignments", "theme_Id", "dbo.Themes", "Id");
        }
    }
}
