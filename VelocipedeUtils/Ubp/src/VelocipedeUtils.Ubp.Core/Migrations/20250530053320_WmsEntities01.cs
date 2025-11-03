using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VelocipedeUtils.Ubp.Core.Migrations
{
    public partial class WmsEntities01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "Project",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "Project");
        }
    }
}
