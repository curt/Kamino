using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kamino.Shared.Migrations
{
    /// <inheritdoc />
    public partial class PingRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Pings_ActorUri",
                table: "Pings",
                column: "ActorUri");

            migrationBuilder.CreateIndex(
                name: "IX_Pings_ToUri",
                table: "Pings",
                column: "ToUri");

            migrationBuilder.AddForeignKey(
                name: "FK_Pings_Profiles_ActorUri",
                table: "Pings",
                column: "ActorUri",
                principalTable: "Profiles",
                principalColumn: "Uri",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pings_Profiles_ToUri",
                table: "Pings",
                column: "ToUri",
                principalTable: "Profiles",
                principalColumn: "Uri",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pings_Profiles_ActorUri",
                table: "Pings");

            migrationBuilder.DropForeignKey(
                name: "FK_Pings_Profiles_ToUri",
                table: "Pings");

            migrationBuilder.DropIndex(
                name: "IX_Pings_ActorUri",
                table: "Pings");

            migrationBuilder.DropIndex(
                name: "IX_Pings_ToUri",
                table: "Pings");
        }
    }
}
