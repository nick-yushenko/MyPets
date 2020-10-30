namespace MrSimonAcademy2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateThemeModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Assignments", "Group_Id", "dbo.Groups");
            DropIndex("dbo.Assignments", new[] { "Group_Id" });
            CreateTable(
                "dbo.Themes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        themeName = c.String(),
                        groupId = c.Int(nullable: false),
                        colorTheme = c.String(),
                        added = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.groupId, cascadeDelete: true)
                .Index(t => t.groupId);
            
            AddColumn("dbo.Assignments", "Theme_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Assignments", "Theme_Id");
            AddForeignKey("dbo.Assignments", "Theme_Id", "dbo.Themes", "Id");
            DropColumn("dbo.Assignments", "Group_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Assignments", "Group_Id", c => c.Int());
            DropForeignKey("dbo.Themes", "groupId", "dbo.Groups");
            DropForeignKey("dbo.Assignments", "Theme_Id", "dbo.Themes");
            DropIndex("dbo.Assignments", new[] { "Theme_Id" });
            DropIndex("dbo.Themes", new[] { "groupId" });
            DropColumn("dbo.Assignments", "Theme_Id");
            DropTable("dbo.Themes");
            CreateIndex("dbo.Assignments", "Group_Id");
            AddForeignKey("dbo.Assignments", "Group_Id", "dbo.Groups", "Id");
        }
    }
}
