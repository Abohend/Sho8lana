using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sho8lana.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Skill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FreelancerSkill",
                columns: table => new
                {
                    FreelancersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SkillsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FreelancerSkill", x => new { x.FreelancersId, x.SkillsId });
                    table.ForeignKey(
                        name: "FK_FreelancerSkill_AspNetUsers_FreelancersId",
                        column: x => x.FreelancersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FreelancerSkill_Skills_SkillsId",
                        column: x => x.SkillsId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FreelancerSkill_SkillsId",
                table: "FreelancerSkill",
                column: "SkillsId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_Name",
                table: "Skills",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FreelancerSkill");

            migrationBuilder.DropTable(
                name: "Skills");
        }
    }
}
