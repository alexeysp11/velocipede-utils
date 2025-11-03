using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VelocipedeUtils.Ubp.Core.Migrations
{
    public partial class AddedLanguages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LanguageKeys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Key = table.Column<string>(type: "text", nullable: false),
                    Endonym = table.Column<string>(type: "text", nullable: false),
                    LanguageType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageKeys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LanguageKeyValuePairs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LanguageType = table.Column<int>(type: "integer", nullable: false),
                    LanguageKeyId = table.Column<long>(type: "bigint", nullable: true),
                    Key = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    ApplicationUid = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateChanged = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BusinessEntityStatus = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageKeyValuePairs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LanguageKeyValuePairs_LanguageKeys_LanguageKeyId",
                        column: x => x.LanguageKeyId,
                        principalTable: "LanguageKeys",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LanguageKeyValuePairs_LanguageKeyId",
                table: "LanguageKeyValuePairs",
                column: "LanguageKeyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LanguageKeyValuePairs");

            migrationBuilder.DropTable(
                name: "LanguageKeys");
        }
    }
}
