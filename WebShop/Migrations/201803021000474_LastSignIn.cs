namespace WebShop.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class LastSignIn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "LastSignIn", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "LastSignIn");
        }
    }
}
