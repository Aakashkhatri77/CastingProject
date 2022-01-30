using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CastingProject.Data.Migrations
{
    public partial class TalentFieldAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DOB",
                table: "talents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "talents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Height",
                table: "talents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "talents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Weight",
                table: "talents",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DOB",
                table: "talents");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "talents");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "talents");

            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "talents");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "talents");
        }
    }
}
