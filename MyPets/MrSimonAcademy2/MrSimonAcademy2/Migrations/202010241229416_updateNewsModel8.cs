namespace MrSimonAcademy2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateNewsModel8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "groupName", c => c.String());
            AddColumn("dbo.News", "groupId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.News", "groupId");
            DropColumn("dbo.News", "groupName");
        }
    }
}
