using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VelocipedeUtils.Ubp.Core.Migrations
{
    public partial class AddedWfApplicationAndForm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FormName",
                table: "LanguageKeyForms",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<long>(
                name: "WfApplicationFormId",
                table: "LanguageKeyForms",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WfApplication",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Uid = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateChanged = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BusinessEntityStatus = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WfApplication", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WfApplicationForm",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WfApplicationId = table.Column<long>(type: "bigint", nullable: false),
                    Uid = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateChanged = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BusinessEntityStatus = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WfApplicationForm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WfApplicationForm_WfApplication_WfApplicationId",
                        column: x => x.WfApplicationId,
                        principalTable: "WfApplication",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LanguageKeyForms_WfApplicationFormId",
                table: "LanguageKeyForms",
                column: "WfApplicationFormId");

            migrationBuilder.CreateIndex(
                name: "IX_WfApplicationForm_WfApplicationId",
                table: "WfApplicationForm",
                column: "WfApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_LanguageKeyForms_WfApplicationForm_WfApplicationFormId",
                table: "LanguageKeyForms",
                column: "WfApplicationFormId",
                principalTable: "WfApplicationForm",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LanguageKeyForms_WfApplicationForm_WfApplicationFormId",
                table: "LanguageKeyForms");

            migrationBuilder.DropTable(
                name: "WfApplicationForm");

            migrationBuilder.DropTable(
                name: "WfApplication");

            migrationBuilder.DropIndex(
                name: "IX_LanguageKeyForms_WfApplicationFormId",
                table: "LanguageKeyForms");

            migrationBuilder.DropColumn(
                name: "WfApplicationFormId",
                table: "LanguageKeyForms");

            migrationBuilder.AlterColumn<string>(
                name: "FormName",
                table: "LanguageKeyForms",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
