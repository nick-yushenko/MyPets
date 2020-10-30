namespace MrSimonAcademy2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateThemeModel6 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Themes", "groupId", "dbo.Groups");
            DropIndex("dbo.Themes", new[] { "groupId" });
            DropTable("dbo.Themes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Themes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        themeName = c.String(),
                        groupId = c.Int(nullable: false),
                        colorTheme = c.String(),
                        added = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Themes", "groupId");
            AddForeignKey("dbo.Themes", "groupId", "dbo.Groups", "Id", cascadeDelete: true);
        }
    }
}
