namespace MrSimonAcademy2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateNewsModel4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "title", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.News", "title");
        }
    }
}
