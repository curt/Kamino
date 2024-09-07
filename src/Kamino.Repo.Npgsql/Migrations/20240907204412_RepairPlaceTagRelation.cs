using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kamino.Repo.Npgsql.Migrations
{
    /// <inheritdoc />
    public partial class RepairPlaceTagRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_places_tags_tag_id",
                table: "places");

            migrationBuilder.DropForeignKey(
                name: "fk_places_tags_posts_tags_id",
                table: "places_tags");

            migrationBuilder.DropForeignKey(
                name: "fk_posts_places_posts_tags_id",
                table: "posts_places");

            migrationBuilder.DropIndex(
                name: "ix_places_tag_id",
                table: "places");

            migrationBuilder.DropColumn(
                name: "tag_id",
                table: "places");

            migrationBuilder.RenameColumn(
                name: "tags_id",
                table: "posts_places",
                newName: "posts_id");

            migrationBuilder.RenameIndex(
                name: "ix_posts_places_tags_id",
                table: "posts_places",
                newName: "ix_posts_places_posts_id");

            migrationBuilder.AddForeignKey(
                name: "fk_places_tags_tags_tags_id",
                table: "places_tags",
                column: "tags_id",
                principalTable: "tags",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_posts_places_posts_posts_id",
                table: "posts_places",
                column: "posts_id",
                principalTable: "posts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_places_tags_tags_tags_id",
                table: "places_tags");

            migrationBuilder.DropForeignKey(
                name: "fk_posts_places_posts_posts_id",
                table: "posts_places");

            migrationBuilder.RenameColumn(
                name: "posts_id",
                table: "posts_places",
                newName: "tags_id");

            migrationBuilder.RenameIndex(
                name: "ix_posts_places_posts_id",
                table: "posts_places",
                newName: "ix_posts_places_tags_id");

            migrationBuilder.AddColumn<Guid>(
                name: "tag_id",
                table: "places",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_places_tag_id",
                table: "places",
                column: "tag_id");

            migrationBuilder.AddForeignKey(
                name: "fk_places_tags_tag_id",
                table: "places",
                column: "tag_id",
                principalTable: "tags",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_places_tags_posts_tags_id",
                table: "places_tags",
                column: "tags_id",
                principalTable: "posts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_posts_places_posts_tags_id",
                table: "posts_places",
                column: "tags_id",
                principalTable: "posts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
