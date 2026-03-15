using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TestProj.Migrations
{
    /// <inheritdoc />
    public partial class SeededDefaultMuseumsImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://t4.ftcdn.net/jpg/03/02/03/03/240_F_302030306_VLGXUtPa0QId7O6Zqz9AF6RSql6uIdVd.jpg");

            migrationBuilder.UpdateData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 2,
                column: "ImageUrl",
                value: "https://t3.ftcdn.net/jpg/00/95/44/26/240_F_95442619_GrhmvQcSput2G9lrbG8QRzX96H3MAvlG.jpg");

            migrationBuilder.UpdateData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 3,
                column: "ImageUrl",
                value: "https://t3.ftcdn.net/jpg/06/66/26/00/240_F_666260007_eCaT4Pk5BP9zWzvY5wPQRjRdLLlkLkVe.jpg");

            migrationBuilder.UpdateData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 4,
                column: "ImageUrl",
                value: "https://t3.ftcdn.net/jpg/05/07/06/78/240_F_507067856_oeG1oQO9FyZhA7j0e0IpTHwsS3ZVWtln.jpg");

            migrationBuilder.UpdateData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 5,
                column: "ImageUrl",
                value: "https://t4.ftcdn.net/jpg/00/83/91/39/240_F_83913965_MdPt8k5xeVa4sIx0zuajUTGGtpqQijkv.jpg");

            migrationBuilder.InsertData(
                table: "MuseumImages",
                columns: new[] { "MuseumImageId", "ImageUrl", "MuseumId" },
                values: new object[,]
                {
                    { 6, "https://t3.ftcdn.net/jpg/18/33/81/44/240_F_1833814465_cDXO5nqwcDp4W9EMiY3eRVkL5zhhQlzJ.jpg", 6 },
                    { 7, "https://t3.ftcdn.net/jpg/02/67/14/66/240_F_267146668_0k2G3hX2Vf9YJ7bA8nXgR0r0nE5K3F8K.jpg", 7 },
                    { 8, "https://t4.ftcdn.net/jpg/01/84/22/97/240_F_184229782_yN0kqzP6n3d6XnL3qQqZpM3u6EJr3K8Q.jpg", 8 },
                    { 9, "https://t3.ftcdn.net/jpg/03/21/44/56/240_F_321445673_jJrF1d8Jm5sF7vM1bT0Xc6yL8q9p6n2.jpg", 9 },
                    { 10, "https://t4.ftcdn.net/jpg/02/94/71/31/240_F_294713180_Zy2VqzE1b1TQn4bS3c0lJ5Fh1Tn8xA0.jpg", 10 },
                    { 11, "https://t3.ftcdn.net/jpg/02/15/32/77/240_F_215327715_qYdE5YjL5r5E9pG3z6H2zY5E1X7p4mK.jpg", 11 },
                    { 12, "https://t4.ftcdn.net/jpg/03/12/64/09/240_F_312640987_T6n4gZ9Y5Yk3Yh2F5X5pY0z1C7d8x9.jpg", 12 },
                    { 13, "https://t3.ftcdn.net/jpg/01/97/23/88/240_F_197238884_kJ0P8Y7V8q7X2H2P5m2N4L1c5y3b8k.jpg", 13 },
                    { 14, "https://t4.ftcdn.net/jpg/02/53/11/90/240_F_253119063_L2o2K5y2M8z3L8c8R3P2F6d2B1y8z.jpg", 14 },
                    { 15, "https://t3.ftcdn.net/jpg/04/20/77/91/240_F_420779188_m6r8X5Y3H4L3B2Y4m5p7Z9Q1A4Y2x.jpg", 15 },
                    { 16, "https://t4.ftcdn.net/jpg/02/18/74/53/240_F_218745357_x6P3k3F3G2F8m2P6q5J4T8Y5z1n2.jpg", 16 },
                    { 17, "https://t3.ftcdn.net/jpg/03/30/77/45/240_F_330774560_Xc1y6P6X3x9H1P5m2Q4F8z5Z3y1.jpg", 17 },
                    { 18, "https://t4.ftcdn.net/jpg/02/88/61/55/240_F_288615596_gY1M1Z7b3X6F5Z9p8T2P7L3Q6F3.jpg", 18 },
                    { 19, "https://t3.ftcdn.net/jpg/03/64/70/02/240_F_364700229_bX1F4m2X4Q6n5T3p6L8Z3P4L7.jpg", 19 },
                    { 20, "https://t4.ftcdn.net/jpg/03/45/98/55/240_F_345985574_X3p5X3M7F2K3P7X4L3m6F5Z1.jpg", 20 },
                    { 21, "https://t3.ftcdn.net/jpg/02/76/44/11/240_F_276441156_T1y7Y5M3F8X4P9M4Z6X1p3Z.jpg", 21 },
                    { 22, "https://t4.ftcdn.net/jpg/02/33/88/90/240_F_233889006_K5Y3Z8Q4F7M2Z5L7F6Q3T2.jpg", 22 },
                    { 23, "https://t3.ftcdn.net/jpg/03/10/75/66/240_F_310756652_L1T4M6P9Z4F2L5P7Y6K3.jpg", 23 },
                    { 24, "https://t4.ftcdn.net/jpg/02/64/88/23/240_F_264882381_H4P7M5Z6F3L8Q3P7X5.jpg", 24 },
                    { 25, "https://t3.ftcdn.net/jpg/03/91/00/14/240_F_391001462_T3P8Z7F4M5X3L8Y1.jpg", 25 },
                    { 26, "https://t4.ftcdn.net/jpg/02/55/63/12/240_F_255631245_M5X4L6T2Z8P5.jpg", 26 },
                    { 27, "https://t3.ftcdn.net/jpg/02/99/55/18/240_F_299551862_Y5M6Z8X4P3F1.jpg", 27 },
                    { 28, "https://t4.ftcdn.net/jpg/02/71/33/66/240_F_271336631_Z4P6L2F9M3.jpg", 28 },
                    { 29, "https://t3.ftcdn.net/jpg/03/40/19/87/240_F_340198709_Q4M8F5Z2P6.jpg", 29 },
                    { 30, "https://t4.ftcdn.net/jpg/02/90/77/44/240_F_290774463_M6X2P8L5F3.jpg", 30 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 30);

            migrationBuilder.UpdateData(
                table: "MuseumImages",
                keyColumn: "MuseumImageId",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://upload.wikimedia.org/wikipedia/commons/a/aa/Louvre_Museum_Wikimedia_Commons.jpg");

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
                value: "https://upload.wikimedia.org/wikipedia/commons/0/0c/Metropolitan_Museum_of_Art_entrance_NYC.jpg");

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
    }
}
