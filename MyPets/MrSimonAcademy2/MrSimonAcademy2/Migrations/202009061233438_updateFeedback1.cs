namespace MrSimonAcademy2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateFeedback1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Feedbacks", "Activity", c => c.Int(nullable: false));
            AddColumn("dbo.Feedbacks", "Concentration", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Feedbacks", "Concentration");
            DropColumn("dbo.Feedbacks", "Activity");
        }
    }
}
