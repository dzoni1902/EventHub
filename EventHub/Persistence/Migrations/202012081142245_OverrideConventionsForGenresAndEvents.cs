namespace EventHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OverrideConventionsForGenresAndEvents : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Events", "Artist_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Events", "Genre_Id", "dbo.Genres");
            DropIndex("dbo.Events", new[] { "Artist_Id" });
            DropIndex("dbo.Events", new[] { "Genre_Id" });
            AlterColumn("dbo.Events", "Venue", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Events", "Artist_Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Events", "Genre_Id", c => c.Byte(nullable: false));
            AlterColumn("dbo.Genres", "Name", c => c.String(nullable: false, maxLength: 255));
            CreateIndex("dbo.Events", "Artist_Id");
            CreateIndex("dbo.Events", "Genre_Id");
            AddForeignKey("dbo.Events", "Artist_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Events", "Genre_Id", "dbo.Genres", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "Genre_Id", "dbo.Genres");
            DropForeignKey("dbo.Events", "Artist_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Events", new[] { "Genre_Id" });
            DropIndex("dbo.Events", new[] { "Artist_Id" });
            AlterColumn("dbo.Genres", "Name", c => c.String());
            AlterColumn("dbo.Events", "Genre_Id", c => c.Byte());
            AlterColumn("dbo.Events", "Artist_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Events", "Venue", c => c.String());
            CreateIndex("dbo.Events", "Genre_Id");
            CreateIndex("dbo.Events", "Artist_Id");
            AddForeignKey("dbo.Events", "Genre_Id", "dbo.Genres", "Id");
            AddForeignKey("dbo.Events", "Artist_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
