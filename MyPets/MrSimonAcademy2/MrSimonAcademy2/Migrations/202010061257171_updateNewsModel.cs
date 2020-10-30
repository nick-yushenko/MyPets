namespace MrSimonAcademy2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateNewsModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "isFromSystem", c => c.Boolean(nullable: false));
            DropColumn("dbo.News", "fromName");
            DropColumn("dbo.News", "isSystem");
        }
        
        public override void Down()
        {
            AddColumn("dbo.News", "isSystem", c => c.Boolean(nullable: false));
            AddColumn("dbo.News", "fromName", c => c.String());
            DropColumn("dbo.News", "isFromSystem");
        }
    }
}
