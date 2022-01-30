using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CastingProject.Data.Migrations
{
    public partial class updatedtalent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EyeColor",
                table: "talents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HairColor",
                table: "talents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HairLength",
                table: "talents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HairType",
                table: "talents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SkinColor",
                table: "talents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SkinType",
                table: "talents",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EyeColor",
                table: "talents");

            migrationBuilder.DropColumn(
                name: "HairColor",
                table: "talents");

            migrationBuilder.DropColumn(
                name: "HairLength",
                table: "talents");

            migrationBuilder.DropColumn(
                name: "HairType",
                table: "talents");

            migrationBuilder.DropColumn(
                name: "SkinColor",
                table: "talents");

            migrationBuilder.DropColumn(
                name: "SkinType",
                table: "talents");
        }
    }
}
