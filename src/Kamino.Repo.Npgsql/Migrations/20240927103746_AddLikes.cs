using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kamino.Repo.Npgsql.Migrations
{
    /// <inheritdoc />
    public partial class AddLikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "likes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    activity_uri = table.Column<string>(type: "text", nullable: false),
                    actor_uri = table.Column<string>(type: "text", nullable: false),
                    object_uri = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_likes", x => x.id);
                    table.UniqueConstraint("ak_likes_activity_uri", x => x.activity_uri);
                    table.UniqueConstraint("ak_likes_actor_uri_object_uri", x => new { x.actor_uri, x.object_uri });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "likes");
        }
    }
}
