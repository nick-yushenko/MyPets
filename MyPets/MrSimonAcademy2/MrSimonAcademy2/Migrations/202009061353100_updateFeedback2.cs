namespace MrSimonAcademy2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateFeedback2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Feedbacks", "AdventuresPlus", c => c.String());
            AddColumn("dbo.Feedbacks", "AdventuresMinus", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Feedbacks", "AdventuresMinus");
            DropColumn("dbo.Feedbacks", "AdventuresPlus");
        }
    }
}
