namespace MrSimonAcademy2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateUser01 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "withoutAvatar", c => c.Boolean(nullable: false));
            DropColumn("dbo.AspNetUsers", "hasAvatar");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "hasAvatar", c => c.Boolean(nullable: false));
            DropColumn("dbo.AspNetUsers", "withoutAvatar");
        }
    }
}
