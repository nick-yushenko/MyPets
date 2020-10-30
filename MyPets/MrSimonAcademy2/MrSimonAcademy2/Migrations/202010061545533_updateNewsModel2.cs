namespace MrSimonAcademy2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateNewsModel2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "News_id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "News_id");
            AddForeignKey("dbo.AspNetUsers", "News_id", "dbo.News", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "News_id", "dbo.News");
            DropIndex("dbo.AspNetUsers", new[] { "News_id" });
            DropColumn("dbo.AspNetUsers", "News_id");
        }
    }
}
