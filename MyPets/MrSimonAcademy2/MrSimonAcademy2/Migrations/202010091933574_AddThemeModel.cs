namespace MrSimonAcademy2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddThemeModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Assignments",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        type = c.String(),
                        AssignmentTask = c.String(),
                        AssignmentFileName = c.String(),
                        AssignmentFileExpansion = c.String(),
                        Group_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.Group_Id)
                .Index(t => t.Group_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Assignments", "Group_Id", "dbo.Groups");
            DropIndex("dbo.Assignments", new[] { "Group_Id" });
            DropTable("dbo.Assignments");
        }
    }
}
