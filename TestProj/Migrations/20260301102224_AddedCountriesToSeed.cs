using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestProj.Migrations
{
    /// <inheritdoc />
    public partial class AddedCountriesToSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Museums",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 1,
                column: "Country",
                value: "France");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 2,
                column: "Country",
                value: "United Kingdom");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 3,
                column: "Country",
                value: "USA");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 4,
                column: "Country",
                value: "Vatican City");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 5,
                column: "Country",
                value: "Spain");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 6,
                column: "Country",
                value: "Russia");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 7,
                column: "Country",
                value: "United Kingdom");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 8,
                column: "Country",
                value: "Netherlands");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 9,
                column: "Country",
                value: "Italy");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 10,
                column: "Country",
                value: "Greece");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 11,
                column: "Country",
                value: "Egypt");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 12,
                column: "Country",
                value: "USA");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 13,
                column: "Country",
                value: "China");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 14,
                column: "Country",
                value: "Japan");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 15,
                column: "Country",
                value: "South Korea");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 16,
                column: "Country",
                value: "USA");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 17,
                column: "Country",
                value: "USA");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 18,
                column: "Country",
                value: "Netherlands");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 19,
                column: "Country",
                value: "Spain");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 20,
                column: "Country",
                value: "USA");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 21,
                column: "Country",
                value: "Canada");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 22,
                column: "Country",
                value: "Mexico");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 23,
                column: "Country",
                value: "Germany");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 24,
                column: "Country",
                value: "Taiwan");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 25,
                column: "Country",
                value: "Brazil");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 26,
                column: "Country",
                value: "New Zealand");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 27,
                column: "Country",
                value: "Spain");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 28,
                column: "Country",
                value: "Australia");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 29,
                column: "Country",
                value: "South Africa");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 30,
                column: "Country",
                value: "Qatar");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Museums");
        }
    }
}
