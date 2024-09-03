using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sho8lana.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddictionalAttributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FreelancerId",
                table: "Projects",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExperienceYears",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProjectSkill",
                columns: table => new
                {
                    ProjectsId = table.Column<int>(type: "int", nullable: false),
                    SkillsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectSkill", x => new { x.ProjectsId, x.SkillsId });
                    table.ForeignKey(
                        name: "FK_ProjectSkill_Projects_ProjectsId",
                        column: x => x.ProjectsId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectSkill_Skills_SkillsId",
                        column: x => x.SkillsId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_FreelancerId",
                table: "Projects",
                column: "FreelancerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSkill_SkillsId",
                table: "ProjectSkill",
                column: "SkillsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_AspNetUsers_FreelancerId",
                table: "Projects",
                column: "FreelancerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_AspNetUsers_FreelancerId",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "ProjectSkill");

            migrationBuilder.DropIndex(
                name: "IX_Projects_FreelancerId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "FreelancerId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ExperienceYears",
                table: "AspNetUsers");
        }
    }
}
