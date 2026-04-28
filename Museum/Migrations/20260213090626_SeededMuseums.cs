using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Museum.Migrations
{
    /// <inheritdoc />
    public partial class SeededMuseums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Museums",
                columns: new[] { "MuseumId", "Address", "City", "Description", "IsActive", "Name", "Type", "WorkingHours" },
                values: new object[,]
                {
                    { 1, "Rue de Rivoli", "Paris", "World's largest art museum.", true, "Louvre Museum", "Art", "09:00-18:00" },
                    { 2, "Great Russell St", "London", "Museum dedicated to human history and culture.", true, "British Museum", "History", "10:00-17:30" },
                    { 3, "1000 5th Ave", "New York", "Largest art museum in the US.", true, "Metropolitan Museum of Art", "Art", "10:00-17:00" },
                    { 4, "Viale Vaticano", "Vatican City", "Christian and art museums.", true, "Vatican Museums", "Art", "09:00-16:00" },
                    { 5, "Calle de Ruiz de Alarcón", "Madrid", "Spanish national art museum.", true, "Prado Museum", "Art", "10:00-20:00" },
                    { 6, "Palace Square", "Saint Petersburg", "One of the largest museums in the world.", true, "State Hermitage Museum", "Art", "10:30-18:00" },
                    { 7, "Trafalgar Square", "London", "Collection of European paintings.", true, "National Gallery", "Art", "10:00-18:00" },
                    { 8, "Museumstraat 1", "Amsterdam", "Dutch national museum.", true, "Rijksmuseum", "Art", "09:00-17:00" },
                    { 9, "Piazzale degli Uffizi", "Florence", "Famous Italian art museum.", true, "Uffizi Gallery", "Art", "08:15-18:30" },
                    { 10, "Dionysiou Areopagitou 15", "Athens", "Archaeological museum focused on the Acropolis.", true, "Acropolis Museum", "Archaeology", "08:00-20:00" },
                    { 11, "Tahrir Square", "Cairo", "Ancient Egyptian antiquities.", true, "Egyptian Museum", "History", "09:00-17:00" },
                    { 12, "600 Independence Ave SW", "Washington", "Aviation and space artifacts.", true, "Smithsonian National Air and Space Museum", "Science", "10:00-17:30" },
                    { 13, "East Chang'an Avenue", "Beijing", "Chinese art and history.", true, "National Museum of China", "History", "09:00-17:00" },
                    { 14, "13-9 Uenokoen", "Tokyo", "Japanese art and antiquities.", true, "Tokyo National Museum", "Art", "09:30-17:00" },
                    { 15, "137 Seobinggo-ro", "Seoul", "Korean history and art.", true, "National Museum of Korea", "History", "10:00-18:00" },
                    { 16, "111 S Michigan Ave", "Chicago", "Famous art museum in Chicago.", true, "Art Institute of Chicago", "Art", "11:00-17:00" },
                    { 17, "11 W 53rd St", "New York", "Modern and contemporary art.", true, "Museum of Modern Art", "Modern Art", "10:30-17:30" },
                    { 18, "Museumplein 6", "Amsterdam", "Works of Vincent van Gogh.", true, "Van Gogh Museum", "Art", "09:00-18:00" },
                    { 19, "Abandoibarra Etorbidea 2", "Bilbao", "Contemporary art museum.", true, "Guggenheim Museum", "Modern Art", "10:00-19:00" },
                    { 20, "945 Magazine St", "New Orleans", "World War II history.", true, "National WWII Museum", "War", "09:00-17:00" },
                    { 21, "100 Laurier St", "Gatineau", "Canadian history and culture.", true, "Canadian Museum of History", "History", "09:00-17:00" },
                    { 22, "Av. Paseo de la Reforma", "Mexico City", "Mexican archaeology and anthropology.", true, "Museo Nacional de Antropología", "Anthropology", "09:00-19:00" },
                    { 23, "Bodestraße 1-3", "Berlin", "Classical antiquities museum.", true, "Pergamon Museum", "Archaeology", "10:00-18:00" },
                    { 24, "221 Zhishan Rd", "Taipei", "Chinese imperial artifacts.", true, "National Palace Museum", "History", "09:00-17:00" },
                    { 25, "Praça Mauá 1", "Rio de Janeiro", "Science museum focused on sustainability.", true, "Museum of Tomorrow", "Science", "10:00-18:00" },
                    { 26, "55 Cable St", "Wellington", "Museum of New Zealand.", true, "Te Papa Tongarewa", "History", "10:00-18:00" },
                    { 27, "Calle Alfonso XII 68", "Madrid", "Anthropology museum.", true, "National Museum of Anthropology", "Anthropology", "09:30-15:00" },
                    { 28, "500 Harris St", "Sydney", "Science and design museum.", true, "Powerhouse Museum", "Science", "10:00-17:00" },
                    { 29, "Northern Parkway Rd", "Johannesburg", "History of apartheid in South Africa.", true, "Apartheid Museum", "History", "09:00-17:00" },
                    { 30, "Corniche", "Doha", "Islamic art collection.", true, "Museum of Islamic Art", "Art", "09:00-19:00" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 30);
        }
    }
}
