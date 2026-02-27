using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestProj.Migrations
{
    /// <inheritdoc />
    public partial class SeededImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://upload.wikimedia.org/wikipedia/commons/a/aa/Louvre_Museum_Wikimedia_Commons.jpg");

            migrationBuilder.UpdateData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 3,
                column: "ImageUrl",
                value: "https://upload.wikimedia.org/wikipedia/commons/0/0c/Metropolitan_Museum_of_Art_entrance_NYC.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://api-www.louvre.fr/sites/default/files/2021-01/cour-napoleon-et-pyramide_1.jpg");

            migrationBuilder.UpdateData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 3,
                column: "ImageUrl",
                value: "https://upload.wikimedia.org/wikipedia/commons/7/7c/The_Metropolitan_Museum_of_Art_%28The_Met%29_Logo.svg");
        }
    }
}
