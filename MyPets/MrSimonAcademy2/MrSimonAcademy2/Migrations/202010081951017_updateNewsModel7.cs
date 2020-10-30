namespace MrSimonAcademy2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateNewsModel7 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.News", "pinned");
            DropColumn("dbo.News", "unpinned");
        }
        
        public override void Down()
        {
            AddColumn("dbo.News", "unpinned", c => c.DateTime());
            AddColumn("dbo.News", "pinned", c => c.DateTime());
        }
    }
}
