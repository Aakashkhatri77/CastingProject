using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CastingProject.Data.Migrations
{
    public partial class TalentProfileAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Profile",
                table: "talents");

            migrationBuilder.CreateTable(
                name: "TalentProfile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TalentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TalentProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TalentProfile_talents_TalentId",
                        column: x => x.TalentId,
                        principalTable: "talents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TalentProfile_TalentId",
                table: "TalentProfile",
                column: "TalentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TalentProfile");

            migrationBuilder.AddColumn<string>(
                name: "Profile",
                table: "talents",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
