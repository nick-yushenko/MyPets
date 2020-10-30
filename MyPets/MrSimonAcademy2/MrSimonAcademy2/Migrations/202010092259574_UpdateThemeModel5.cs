namespace MrSimonAcademy2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateThemeModel5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Assignments", "Theme_Id", "dbo.Themes");
            DropIndex("dbo.Assignments", new[] { "Theme_Id" });
            DropTable("dbo.Assignments");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Assignments", "Theme_Id");
            AddForeignKey("dbo.Assignments", "Theme_Id", "dbo.Themes", "Id");
        }
    }
}
