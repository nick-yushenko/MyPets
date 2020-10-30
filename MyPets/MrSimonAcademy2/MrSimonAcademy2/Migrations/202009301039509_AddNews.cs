namespace MrSimonAcademy2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNews : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.News",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        fromName = c.String(),
                        fromId = c.String(),
                        isForUser = c.Boolean(nullable: false),
                        isForGroup = c.Boolean(nullable: false),
                        isSystem = c.Boolean(nullable: false),
                        isForAll = c.Boolean(nullable: false),
                        message = c.String(),
                        added = c.DateTime(nullable: false),
                        removed = c.DateTime(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.News");
        }
    }
}
