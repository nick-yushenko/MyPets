namespace MrSimonAcademy2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateThemeModel14 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assignments", "added", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assignments", "added");
        }
    }
}
