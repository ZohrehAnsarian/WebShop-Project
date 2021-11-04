namespace WebShop.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AllowAcceptReject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "AllowAcceptReject", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "AllowAcceptReject");
        }
    }
}
