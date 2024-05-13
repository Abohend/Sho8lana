using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace src.Migrations
{
    /// <inheritdoc />
    public partial class UnTieJobAndFreelancer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_AspNetUsers_FreelancerId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_FreelancerId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "FreelancerId",
                table: "Jobs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FreelancerId",
                table: "Jobs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_FreelancerId",
                table: "Jobs",
                column: "FreelancerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_AspNetUsers_FreelancerId",
                table: "Jobs",
                column: "FreelancerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
