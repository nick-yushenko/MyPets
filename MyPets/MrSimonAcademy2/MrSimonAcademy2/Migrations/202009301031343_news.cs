namespace MrSimonAcademy2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class news : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Groups", "declaration");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Groups", "declaration", c => c.String());
        }
    }
}
