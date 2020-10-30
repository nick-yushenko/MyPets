namespace MrSimonAcademy2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateThemeModel11 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.GroupUsers", newName: "UserGroups");
            DropPrimaryKey("dbo.UserGroups");
            AddPrimaryKey("dbo.UserGroups", new[] { "User_Id", "Group_Id" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.UserGroups");
            AddPrimaryKey("dbo.UserGroups", new[] { "Group_Id", "User_Id" });
            RenameTable(name: "dbo.UserGroups", newName: "GroupUsers");
        }
    }
}
