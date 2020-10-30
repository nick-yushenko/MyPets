namespace MrSimonAcademy2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateNewsModel3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "News_id", "dbo.News");
            DropIndex("dbo.AspNetUsers", new[] { "News_id" });
            CreateTable(
                "dbo.NewsUsers",
                c => new
                    {
                        News_id = c.Int(nullable: false),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.News_id, t.User_Id })
                .ForeignKey("dbo.News", t => t.News_id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.News_id)
                .Index(t => t.User_Id);
            
            DropColumn("dbo.AspNetUsers", "News_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "News_id", c => c.Int());
            DropForeignKey("dbo.NewsUsers", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.NewsUsers", "News_id", "dbo.News");
            DropIndex("dbo.NewsUsers", new[] { "User_Id" });
            DropIndex("dbo.NewsUsers", new[] { "News_id" });
            DropTable("dbo.NewsUsers");
            CreateIndex("dbo.AspNetUsers", "News_id");
            AddForeignKey("dbo.AspNetUsers", "News_id", "dbo.News", "id");
        }
    }
}
