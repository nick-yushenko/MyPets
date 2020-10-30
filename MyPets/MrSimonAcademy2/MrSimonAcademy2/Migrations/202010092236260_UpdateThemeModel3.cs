namespace MrSimonAcademy2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateThemeModel3 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Assignments");
            AlterColumn("dbo.Assignments", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Assignments", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Assignments");
            AlterColumn("dbo.Assignments", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Assignments", "Id");
        }
    }
}
