namespace EventHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddForeignKeysToEvent : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Events", name: "Artist_Id", newName: "ArtistId");
            RenameColumn(table: "dbo.Events", name: "Genre_Id", newName: "GenreId");
            RenameIndex(table: "dbo.Events", name: "IX_Artist_Id", newName: "IX_ArtistId");
            RenameIndex(table: "dbo.Events", name: "IX_Genre_Id", newName: "IX_GenreId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Events", name: "IX_GenreId", newName: "IX_Genre_Id");
            RenameIndex(table: "dbo.Events", name: "IX_ArtistId", newName: "IX_Artist_Id");
            RenameColumn(table: "dbo.Events", name: "GenreId", newName: "Genre_Id");
            RenameColumn(table: "dbo.Events", name: "ArtistId", newName: "Artist_Id");
        }
    }
}
