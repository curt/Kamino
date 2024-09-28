using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kamino.Repo.Npgsql.Migrations
{
    /// <inheritdoc />
    public partial class AddPingsPongs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    activity_uri = table.Column<string>(type: "text", nullable: false),
                    actor_uri = table.Column<string>(type: "text", nullable: false),
                    to_uri = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pings", x => x.id);
                    table.UniqueConstraint("ak_pings_activity_uri", x => x.activity_uri);
                });

            migrationBuilder.CreateTable(
                name: "pongs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    activity_uri = table.Column<string>(type: "text", nullable: false),
                    ping_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pongs", x => x.id);
                    table.UniqueConstraint("ak_pongs_activity_uri", x => x.activity_uri);
                    table.ForeignKey(
                        name: "fk_pongs_pings_ping_id",
                        column: x => x.ping_id,
                        principalTable: "pings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_pongs_ping_id",
                table: "pongs",
                column: "ping_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pongs");

            migrationBuilder.DropTable(
                name: "pings");
        }
    }
}
