using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Sho8lana.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class seedCategoriesAndSkills : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "ImagePath", "Name" },
                values: new object[,]
                {
                    { 1, "category/motionGraphic.png", "Motion Graphics" },
                    { 2, "category/video-edit.png", "Video Editing" },
                    { 3, "category/appDevelopment.png", "Software Development" },
                    { 4, "category/translate.png", "Translating" },
                    { 5, "category/digitalMarketing.png", "Digital Marketing" }
                });

            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "SQL" },
                    { 2, "C#" },
                    { 3, "Python" },
                    { 4, "Java" },
                    { 5, "Arabic" },
                    { 6, "3dMax" },
                    { 7, "Photoshop" },
                    { 8, "English" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 8);
        }
    }
}
