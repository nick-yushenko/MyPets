namespace MrSimonAcademy2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateUser1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "hasAvatar", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "hasAvatar");
        }
    }
}
