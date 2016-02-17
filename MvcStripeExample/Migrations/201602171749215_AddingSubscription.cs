namespace MvcStripeExample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingSubscription : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Subscriptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AdminEmail = c.String(),
                        StripeCustomerId = c.String(),
                        Status = c.Int(nullable: false),
                        StatusDetail = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "SubscriptionId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "SubscriptionId");
            DropTable("dbo.Subscriptions");
        }
    }
}
