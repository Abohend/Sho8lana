using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sho8lana.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ProjectProposal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectsProposal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OfferedPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OfferedDeliverDate = table.Column<DateOnly>(type: "date", nullable: false),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: true),
                    ClientNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FreelancerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectsProposal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectsProposal_AspNetUsers_FreelancerId",
                        column: x => x.FreelancerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ProjectsProposal_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectsProposal_FreelancerId",
                table: "ProjectsProposal",
                column: "FreelancerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectsProposal_ProjectId",
                table: "ProjectsProposal",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectsProposal");
        }
    }
}
