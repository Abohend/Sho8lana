using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sho8lana.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FinalProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveredProducts");

            migrationBuilder.CreateTable(
                name: "DeliveredJobs",
                columns: table => new
                {
                    JobId = table.Column<int>(type: "int", nullable: false),
                    GitHubUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Verified = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveredJobs", x => x.JobId);
                    table.ForeignKey(
                        name: "FK_DeliveredJobs_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveredProjects",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    GitHubUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Verified = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveredProjects", x => x.ProjectId);
                    table.ForeignKey(
                        name: "FK_DeliveredProjects_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveredJobs");

            migrationBuilder.DropTable(
                name: "DeliveredProjects");

            migrationBuilder.CreateTable(
                name: "DeliveredProducts",
                columns: table => new
                {
                    WorkId = table.Column<int>(type: "int", nullable: false),
                    WorkType = table.Column<int>(type: "int", nullable: false),
                    GitHubUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Verified = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveredProducts", x => new { x.WorkId, x.WorkType });
                    table.ForeignKey(
                        name: "FK_DeliveredProducts_Jobs_WorkId",
                        column: x => x.WorkId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeliveredProducts_Projects_WorkId",
                        column: x => x.WorkId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveredProducts_WorkId",
                table: "DeliveredProducts",
                column: "WorkId",
                unique: true);
        }
    }
}
