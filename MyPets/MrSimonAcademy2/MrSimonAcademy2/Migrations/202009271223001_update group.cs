namespace MrSimonAcademy2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updategroup : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Groups", "textbook", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Groups", "textbook");
        }
    }
}
