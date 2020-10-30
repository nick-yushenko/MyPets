namespace MrSimonAcademy2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateThemeModel9 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Assignments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        type = c.String(),
                        AssignmentTask = c.String(),
                        AssignmentFileName = c.String(),
                        AssignmentFileExpansion = c.String(),
                        Theme_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Themes", t => t.Theme_Id)
                .Index(t => t.Theme_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Assignments", "Theme_Id", "dbo.Themes");
            DropIndex("dbo.Assignments", new[] { "Theme_Id" });
            DropTable("dbo.Assignments");
        }
    }
}
