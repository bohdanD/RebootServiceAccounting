namespace projUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCostToSpendings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Spendings", "Cost", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Spendings", "Cost");
        }
    }
}
