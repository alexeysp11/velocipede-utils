using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VelocipedeUtils.Ubp.Core.Migrations
{
    public partial class AbsensesIsPaid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Absenses",
                type: "boolean",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Absenses");
        }
    }
}
