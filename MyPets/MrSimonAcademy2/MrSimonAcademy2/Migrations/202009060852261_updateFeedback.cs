namespace MrSimonAcademy2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateFeedback : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Feedbacks", "HomeworkCount", c => c.Int(nullable: false));
            AddColumn("dbo.Feedbacks", "passedHomework", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Feedbacks", "passedHomework");
            DropColumn("dbo.Feedbacks", "HomeworkCount");
        }
    }
}
