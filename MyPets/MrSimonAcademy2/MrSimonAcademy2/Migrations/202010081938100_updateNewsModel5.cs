namespace MrSimonAcademy2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateNewsModel5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "onlyStudent", c => c.Boolean(nullable: false));
            AddColumn("dbo.News", "onlyTeacher", c => c.Boolean(nullable: false));
            AddColumn("dbo.News", "onlyAdmin", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.News", "onlyAdmin");
            DropColumn("dbo.News", "onlyTeacher");
            DropColumn("dbo.News", "onlyStudent");
        }
    }
}
