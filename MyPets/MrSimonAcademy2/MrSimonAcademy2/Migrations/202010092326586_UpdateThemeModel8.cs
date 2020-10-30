namespace MrSimonAcademy2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateThemeModel8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Themes", "added", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Themes", "added");
        }
    }
}
