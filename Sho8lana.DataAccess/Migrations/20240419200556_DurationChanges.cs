using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sho8lana.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class DurationChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpectedDuration_Hours",
                table: "Projects",
                newName: "ExpectedDuration_Months");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpectedDuration_Months",
                table: "Projects",
                newName: "ExpectedDuration_Hours");
        }
    }
}
