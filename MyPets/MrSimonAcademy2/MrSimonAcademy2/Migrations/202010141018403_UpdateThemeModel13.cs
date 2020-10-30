namespace MrSimonAcademy2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateThemeModel13 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assignments", "fileName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assignments", "fileName");
        }
    }
}
