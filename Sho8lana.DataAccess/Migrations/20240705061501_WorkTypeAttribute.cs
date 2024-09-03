using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sho8lana.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class WorkTypeAttribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DeliveredProducts",
                table: "DeliveredProducts");

            migrationBuilder.AddColumn<int>(
                name: "WorkType",
                table: "DeliveredProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeliveredProducts",
                table: "DeliveredProducts",
                columns: new[] { "WorkId", "WorkType" });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveredProducts_WorkId",
                table: "DeliveredProducts",
                column: "WorkId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DeliveredProducts",
                table: "DeliveredProducts");

            migrationBuilder.DropIndex(
                name: "IX_DeliveredProducts_WorkId",
                table: "DeliveredProducts");

            migrationBuilder.DropColumn(
                name: "WorkType",
                table: "DeliveredProducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeliveredProducts",
                table: "DeliveredProducts",
                column: "WorkId");
        }
    }
}
