using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CastingProject.Data.Migrations
{
    public partial class TalentProfileTableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TalentProfile_talents_TalentId",
                table: "TalentProfile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TalentProfile",
                table: "TalentProfile");

            migrationBuilder.RenameTable(
                name: "TalentProfile",
                newName: "talentProfiles");

            migrationBuilder.RenameIndex(
                name: "IX_TalentProfile_TalentId",
                table: "talentProfiles",
                newName: "IX_talentProfiles_TalentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_talentProfiles",
                table: "talentProfiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_talentProfiles_talents_TalentId",
                table: "talentProfiles",
                column: "TalentId",
                principalTable: "talents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_talentProfiles_talents_TalentId",
                table: "talentProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_talentProfiles",
                table: "talentProfiles");

            migrationBuilder.RenameTable(
                name: "talentProfiles",
                newName: "TalentProfile");

            migrationBuilder.RenameIndex(
                name: "IX_talentProfiles_TalentId",
                table: "TalentProfile",
                newName: "IX_TalentProfile_TalentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TalentProfile",
                table: "TalentProfile",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TalentProfile_talents_TalentId",
                table: "TalentProfile",
                column: "TalentId",
                principalTable: "talents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
