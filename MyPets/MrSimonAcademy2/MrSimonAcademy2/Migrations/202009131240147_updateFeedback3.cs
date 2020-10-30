namespace MrSimonAcademy2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateFeedback3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Feedbacks", "AddingData", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Feedbacks", "AddingData");
        }
    }
}
