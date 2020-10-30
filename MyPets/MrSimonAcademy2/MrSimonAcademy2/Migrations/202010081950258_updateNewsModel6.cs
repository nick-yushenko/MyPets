namespace MrSimonAcademy2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateNewsModel6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "isPinned", c => c.Boolean(nullable: false));
            AddColumn("dbo.News", "pinned", c => c.DateTime());
            AddColumn("dbo.News", "unpinned", c => c.DateTime());
            DropColumn("dbo.News", "removed");
        }
        
        public override void Down()
        {
            AddColumn("dbo.News", "removed", c => c.DateTime());
            DropColumn("dbo.News", "unpinned");
            DropColumn("dbo.News", "pinned");
            DropColumn("dbo.News", "isPinned");
        }
    }
}
