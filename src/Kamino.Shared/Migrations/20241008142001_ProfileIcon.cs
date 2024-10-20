using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kamino.Shared.Migrations
{
    /// <inheritdoc />
    public partial class ProfileIcon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Profiles",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Profiles");
        }
    }
}
