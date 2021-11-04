namespace WebShop.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class UserDefiner : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "UserDefiner", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "UserDefiner");
        }
    }
}
