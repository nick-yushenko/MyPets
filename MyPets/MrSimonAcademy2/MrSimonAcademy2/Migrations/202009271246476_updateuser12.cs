namespace MrSimonAcademy2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateuser12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "shutdownDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "shutdownDate");
        }
    }
}
