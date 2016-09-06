namespace projUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 128),
                        PhoneNumber = c.String(),
                        Model = c.String(),
                        Problem = c.String(),
                        Cost = c.Int(nullable: false),
                        Income = c.Int(),
                        ReceptionDate = c.DateTime(nullable: false),
                        GivingDate = c.DateTime(),
                        IsDone = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Name)
                .Index(t => t.Name);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        Password = c.String(),
                        IsAdmin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Name);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Clients", "Name", "dbo.Users");
            DropIndex("dbo.Clients", new[] { "Name" });
            DropTable("dbo.Users");
            DropTable("dbo.Clients");
        }
    }
}
