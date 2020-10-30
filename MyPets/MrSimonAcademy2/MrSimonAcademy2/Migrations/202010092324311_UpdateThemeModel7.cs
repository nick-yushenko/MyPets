namespace MrSimonAcademy2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateThemeModel7 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Themes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        themeName = c.String(),
                        groupId = c.Int(nullable: false),
                        colorTheme = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.groupId, cascadeDelete: true)
                .Index(t => t.groupId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Themes", "groupId", "dbo.Groups");
            DropIndex("dbo.Themes", new[] { "groupId" });
            DropTable("dbo.Themes");
        }
    }
}
