using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TestProj.Migrations
{
    /// <inheritdoc />
    public partial class ImagesAndTickets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MuseumImages",
                columns: new[] { "MuseumImageId", "ImageUrl", "MuseumId" },
                values: new object[,]
                {
                    { 1, "/images/museums/louvre.jpg", 1 },
                    { 2, "/images/museums/british.jpg", 2 },
                    { 3, "/images/museums/met.jpg", 3 },
                    { 4, "/images/museums/vatican.jpg", 4 },
                    { 5, "/images/museums/prado.jpg", 5 }
                });

            migrationBuilder.InsertData(
                table: "TicketTypes",
                columns: new[] { "TicketTypeId", "IsActive", "MuseumId", "Name", "Price" },
                values: new object[,]
                {
                    { 1, true, 1, "Adult", 25m },
                    { 2, true, 1, "Student", 15m },
                    { 3, true, 1, "Child", 10m },
                    { 4, true, 2, "Adult", 20m },
                    { 5, true, 2, "Student", 12m },
                    { 6, true, 2, "Child", 8m },
                    { 7, true, 3, "Adult", 28m },
                    { 8, true, 3, "Student", 18m },
                    { 9, true, 3, "Child", 12m },
                    { 10, true, 4, "Adult", 30m },
                    { 11, true, 4, "Student", 20m },
                    { 12, true, 4, "Child", 15m },
                    { 13, true, 5, "Adult", 22m },
                    { 14, true, 5, "Student", 14m },
                    { 15, true, 5, "Child", 9m },
                    { 16, true, 6, "Adult", 20m },
                    { 17, true, 6, "Student", 12m },
                    { 18, true, 6, "Child", 8m },
                    { 19, true, 7, "Adult", 20m },
                    { 20, true, 7, "Student", 12m },
                    { 21, true, 7, "Child", 8m },
                    { 22, true, 8, "Adult", 20m },
                    { 23, true, 8, "Student", 12m },
                    { 24, true, 8, "Child", 8m },
                    { 25, true, 9, "Adult", 20m },
                    { 26, true, 9, "Student", 12m },
                    { 27, true, 9, "Child", 8m },
                    { 28, true, 10, "Adult", 20m },
                    { 29, true, 10, "Student", 12m },
                    { 30, true, 10, "Child", 8m },
                    { 31, true, 11, "Adult", 20m },
                    { 32, true, 11, "Student", 12m },
                    { 33, true, 11, "Child", 8m },
                    { 34, true, 12, "Adult", 20m },
                    { 35, true, 12, "Student", 12m },
                    { 36, true, 12, "Child", 8m },
                    { 37, true, 13, "Adult", 20m },
                    { 38, true, 13, "Student", 12m },
                    { 39, true, 13, "Child", 8m },
                    { 40, true, 14, "Adult", 20m },
                    { 41, true, 14, "Student", 12m },
                    { 42, true, 14, "Child", 8m },
                    { 43, true, 15, "Adult", 20m },
                    { 44, true, 15, "Student", 12m },
                    { 45, true, 15, "Child", 8m },
                    { 46, true, 16, "Adult", 20m },
                    { 47, true, 16, "Student", 12m },
                    { 48, true, 16, "Child", 8m },
                    { 49, true, 17, "Adult", 20m },
                    { 50, true, 17, "Student", 12m },
                    { 51, true, 17, "Child", 8m },
                    { 52, true, 18, "Adult", 20m },
                    { 53, true, 18, "Student", 12m },
                    { 54, true, 18, "Child", 8m },
                    { 55, true, 19, "Adult", 20m },
                    { 56, true, 19, "Student", 12m },
                    { 57, true, 19, "Child", 8m },
                    { 58, true, 20, "Adult", 20m },
                    { 59, true, 20, "Student", 12m },
                    { 60, true, 20, "Child", 8m },
                    { 61, true, 21, "Adult", 20m },
                    { 62, true, 21, "Student", 12m },
                    { 63, true, 21, "Child", 8m },
                    { 64, true, 22, "Adult", 20m },
                    { 65, true, 22, "Student", 12m },
                    { 66, true, 22, "Child", 8m },
                    { 67, true, 23, "Adult", 20m },
                    { 68, true, 23, "Student", 12m },
                    { 69, true, 23, "Child", 8m },
                    { 70, true, 24, "Adult", 20m },
                    { 71, true, 24, "Student", 12m },
                    { 72, true, 24, "Child", 8m },
                    { 73, true, 25, "Adult", 20m },
                    { 74, true, 25, "Student", 12m },
                    { 75, true, 25, "Child", 8m },
                    { 76, true, 26, "Adult", 20m },
                    { 77, true, 26, "Student", 12m },
                    { 78, true, 26, "Child", 8m },
                    { 79, true, 27, "Adult", 20m },
                    { 80, true, 27, "Student", 12m },
                    { 81, true, 27, "Child", 8m },
                    { 82, true, 28, "Adult", 20m },
                    { 83, true, 28, "Student", 12m },
                    { 84, true, 28, "Child", 8m },
                    { 85, true, 29, "Adult", 20m },
                    { 86, true, 29, "Student", 12m },
                    { 87, true, 29, "Child", 8m },
                    { 88, true, 30, "Adult", 20m },
                    { 89, true, 30, "Student", 12m },
                    { 90, true, 30, "Child", 8m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 90);
        }
    }
}
