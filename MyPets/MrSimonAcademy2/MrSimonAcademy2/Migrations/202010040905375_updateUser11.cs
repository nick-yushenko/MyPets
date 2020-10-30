namespace MrSimonAcademy2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateUser11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "avatarName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "avatarName");
        }
    }
}
