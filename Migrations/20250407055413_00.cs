using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Roulette.Migrations
{
    /// <inheritdoc />
    public partial class _00 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RouletteNumber",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Color = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouletteNumber", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Balance = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "RouletteNumber",
                columns: new[] { "Id", "Color" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 1 },
                    { 4, 2 },
                    { 5, 1 },
                    { 6, 2 },
                    { 7, 1 },
                    { 8, 2 },
                    { 9, 1 },
                    { 10, 2 },
                    { 11, 2 },
                    { 12, 1 },
                    { 13, 2 },
                    { 14, 1 },
                    { 15, 2 },
                    { 16, 1 },
                    { 17, 2 },
                    { 18, 1 },
                    { 19, 1 },
                    { 20, 2 },
                    { 21, 1 },
                    { 22, 2 },
                    { 23, 1 },
                    { 24, 2 },
                    { 25, 1 },
                    { 26, 2 },
                    { 27, 1 },
                    { 28, 2 },
                    { 29, 2 },
                    { 30, 1 },
                    { 31, 2 },
                    { 32, 1 },
                    { 33, 2 },
                    { 34, 1 },
                    { 35, 2 },
                    { 36, 1 },
                    { 37, 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RouletteNumber_Id",
                table: "RouletteNumber",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Name",
                table: "User",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RouletteNumber");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
