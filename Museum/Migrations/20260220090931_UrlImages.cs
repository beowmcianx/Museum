using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Museum.Migrations
{
    /// <inheritdoc />
    public partial class UrlImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://upload.wikimedia.org/wikipedia/commons/a/af/Louvre_Museum_Wikimedia_Commons.jpg");

            migrationBuilder.UpdateData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 2,
                column: "ImageUrl",
                value: "https://upload.wikimedia.org/wikipedia/commons/a/a3/British_Museum_from_NE_2.JPG");

            migrationBuilder.UpdateData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 3,
                column: "ImageUrl",
                value: "https://upload.wikimedia.org/wikipedia/commons/7/7c/The_Metropolitan_Museum_of_Art_%28The_Met%29_Logo.svg");

            migrationBuilder.UpdateData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 4,
                column: "ImageUrl",
                value: "https://upload.wikimedia.org/wikipedia/commons/6/6f/Vatican_Museums_Entrance.jpg");

            migrationBuilder.UpdateData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 5,
                column: "ImageUrl",
                value: "https://upload.wikimedia.org/wikipedia/commons/4/4f/Museo_del_Prado_2016_%28cropped%29.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 1,
                column: "ImageUrl",
                value: "/images/museums/louvre.jpg");

            migrationBuilder.UpdateData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 2,
                column: "ImageUrl",
                value: "/images/museums/british.jpg");

            migrationBuilder.UpdateData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 3,
                column: "ImageUrl",
                value: "/images/museums/met.jpg");

            migrationBuilder.UpdateData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 4,
                column: "ImageUrl",
                value: "/images/museums/vatican.jpg");

            migrationBuilder.UpdateData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 5,
                column: "ImageUrl",
                value: "/images/museums/prado.jpg");
        }
    }
}
