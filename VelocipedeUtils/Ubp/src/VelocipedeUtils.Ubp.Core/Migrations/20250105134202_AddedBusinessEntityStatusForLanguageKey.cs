using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VelocipedeUtils.Ubp.Core.Migrations
{
    public partial class AddedBusinessEntityStatusForLanguageKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BusinessEntityStatus",
                table: "LanguageKeys",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessEntityStatus",
                table: "LanguageKeys");
        }
    }
}
