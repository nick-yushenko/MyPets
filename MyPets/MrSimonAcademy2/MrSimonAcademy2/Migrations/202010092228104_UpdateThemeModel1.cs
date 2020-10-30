namespace MrSimonAcademy2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateThemeModel1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Assignments", "Theme_Id", "dbo.Themes");
            DropIndex("dbo.Assignments", new[] { "Theme_Id" });
            DropPrimaryKey("dbo.Themes");
            AlterColumn("dbo.Themes", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Assignments", "Theme_Id", c => c.Int());
            AddPrimaryKey("dbo.Themes", "Id");
            CreateIndex("dbo.Assignments", "Theme_Id");
            AddForeignKey("dbo.Assignments", "Theme_Id", "dbo.Themes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Assignments", "Theme_Id", "dbo.Themes");
            DropIndex("dbo.Assignments", new[] { "Theme_Id" });
            DropPrimaryKey("dbo.Themes");
            AlterColumn("dbo.Assignments", "Theme_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Themes", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Themes", "Id");
            CreateIndex("dbo.Assignments", "Theme_Id");
            AddForeignKey("dbo.Assignments", "Theme_Id", "dbo.Themes", "Id");
        }
    }
}
