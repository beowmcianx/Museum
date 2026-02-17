using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestProj.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedWorkingHours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkingHours",
                table: "Museums");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ClosingTime",
                table: "Museums",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "OpeningTime",
                table: "Museums",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 1,
                columns: new[] { "ClosingTime", "OpeningTime" },
                values: new object[] { new TimeSpan(0, 18, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 2,
                columns: new[] { "ClosingTime", "OpeningTime" },
                values: new object[] { new TimeSpan(0, 17, 30, 0, 0), new TimeSpan(0, 10, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 3,
                columns: new[] { "ClosingTime", "OpeningTime" },
                values: new object[] { new TimeSpan(0, 17, 0, 0, 0), new TimeSpan(0, 10, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 4,
                columns: new[] { "ClosingTime", "OpeningTime" },
                values: new object[] { new TimeSpan(0, 16, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 5,
                columns: new[] { "ClosingTime", "OpeningTime" },
                values: new object[] { new TimeSpan(0, 20, 0, 0, 0), new TimeSpan(0, 10, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 6,
                columns: new[] { "ClosingTime", "OpeningTime" },
                values: new object[] { new TimeSpan(0, 18, 0, 0, 0), new TimeSpan(0, 10, 30, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 7,
                columns: new[] { "ClosingTime", "OpeningTime" },
                values: new object[] { new TimeSpan(0, 18, 0, 0, 0), new TimeSpan(0, 10, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 8,
                columns: new[] { "ClosingTime", "OpeningTime" },
                values: new object[] { new TimeSpan(0, 17, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 9,
                columns: new[] { "ClosingTime", "OpeningTime" },
                values: new object[] { new TimeSpan(0, 18, 30, 0, 0), new TimeSpan(0, 8, 15, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 10,
                columns: new[] { "ClosingTime", "OpeningTime" },
                values: new object[] { new TimeSpan(0, 20, 0, 0, 0), new TimeSpan(0, 8, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 11,
                columns: new[] { "ClosingTime", "OpeningTime" },
                values: new object[] { new TimeSpan(0, 17, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 12,
                columns: new[] { "ClosingTime", "OpeningTime" },
                values: new object[] { new TimeSpan(0, 17, 30, 0, 0), new TimeSpan(0, 10, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 13,
                columns: new[] { "ClosingTime", "OpeningTime" },
                values: new object[] { new TimeSpan(0, 17, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 14,
                columns: new[] { "ClosingTime", "OpeningTime" },
                values: new object[] { new TimeSpan(0, 17, 0, 0, 0), new TimeSpan(0, 9, 30, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 15,
                columns: new[] { "ClosingTime", "OpeningTime" },
                values: new object[] { new TimeSpan(0, 18, 0, 0, 0), new TimeSpan(0, 10, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 16,
                columns: new[] { "ClosingTime", "OpeningTime" },
                values: new object[] { new TimeSpan(0, 17, 0, 0, 0), new TimeSpan(0, 11, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 17,
                columns: new[] { "ClosingTime", "OpeningTime" },
                values: new object[] { new TimeSpan(0, 17, 30, 0, 0), new TimeSpan(0, 10, 30, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 18,
                columns: new[] { "ClosingTime", "OpeningTime" },
                values: new object[] { new TimeSpan(0, 18, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 19,
                columns: new[] { "ClosingTime", "OpeningTime" },
                values: new object[] { new TimeSpan(0, 19, 0, 0, 0), new TimeSpan(0, 10, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 20,
                columns: new[] { "ClosingTime", "OpeningTime" },
                values: new object[] { new TimeSpan(0, 17, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 21,
                columns: new[] { "ClosingTime", "OpeningTime" },
                values: new object[] { new TimeSpan(0, 17, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 22,
                columns: new[] { "ClosingTime", "OpeningTime" },
                values: new object[] { new TimeSpan(0, 19, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 23,
                columns: new[] { "ClosingTime", "OpeningTime" },
                values: new object[] { new TimeSpan(0, 18, 0, 0, 0), new TimeSpan(0, 10, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 24,
                columns: new[] { "ClosingTime", "OpeningTime" },
                values: new object[] { new TimeSpan(0, 17, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 25,
                columns: new[] { "ClosingTime", "OpeningTime" },
                values: new object[] { new TimeSpan(0, 18, 0, 0, 0), new TimeSpan(0, 10, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 26,
                columns: new[] { "ClosingTime", "OpeningTime" },
                values: new object[] { new TimeSpan(0, 18, 0, 0, 0), new TimeSpan(0, 10, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 27,
                columns: new[] { "ClosingTime", "OpeningTime" },
                values: new object[] { new TimeSpan(0, 15, 0, 0, 0), new TimeSpan(0, 9, 30, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 28,
                columns: new[] { "ClosingTime", "OpeningTime" },
                values: new object[] { new TimeSpan(0, 17, 0, 0, 0), new TimeSpan(0, 10, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 29,
                columns: new[] { "ClosingTime", "OpeningTime" },
                values: new object[] { new TimeSpan(0, 17, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 30,
                columns: new[] { "ClosingTime", "OpeningTime" },
                values: new object[] { new TimeSpan(0, 19, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClosingTime",
                table: "Museums");

            migrationBuilder.DropColumn(
                name: "OpeningTime",
                table: "Museums");

            migrationBuilder.AddColumn<string>(
                name: "WorkingHours",
                table: "Museums",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 1,
                column: "WorkingHours",
                value: "09:00-18:00");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 2,
                column: "WorkingHours",
                value: "10:00-17:30");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 3,
                column: "WorkingHours",
                value: "10:00-17:00");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 4,
                column: "WorkingHours",
                value: "09:00-16:00");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 5,
                column: "WorkingHours",
                value: "10:00-20:00");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 6,
                column: "WorkingHours",
                value: "10:30-18:00");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 7,
                column: "WorkingHours",
                value: "10:00-18:00");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 8,
                column: "WorkingHours",
                value: "09:00-17:00");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 9,
                column: "WorkingHours",
                value: "08:15-18:30");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 10,
                column: "WorkingHours",
                value: "08:00-20:00");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 11,
                column: "WorkingHours",
                value: "09:00-17:00");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 12,
                column: "WorkingHours",
                value: "10:00-17:30");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 13,
                column: "WorkingHours",
                value: "09:00-17:00");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 14,
                column: "WorkingHours",
                value: "09:30-17:00");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 15,
                column: "WorkingHours",
                value: "10:00-18:00");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 16,
                column: "WorkingHours",
                value: "11:00-17:00");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 17,
                column: "WorkingHours",
                value: "10:30-17:30");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 18,
                column: "WorkingHours",
                value: "09:00-18:00");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 19,
                column: "WorkingHours",
                value: "10:00-19:00");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 20,
                column: "WorkingHours",
                value: "09:00-17:00");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 21,
                column: "WorkingHours",
                value: "09:00-17:00");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 22,
                column: "WorkingHours",
                value: "09:00-19:00");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 23,
                column: "WorkingHours",
                value: "10:00-18:00");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 24,
                column: "WorkingHours",
                value: "09:00-17:00");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 25,
                column: "WorkingHours",
                value: "10:00-18:00");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 26,
                column: "WorkingHours",
                value: "10:00-18:00");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 27,
                column: "WorkingHours",
                value: "09:30-15:00");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 28,
                column: "WorkingHours",
                value: "10:00-17:00");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 29,
                column: "WorkingHours",
                value: "09:00-17:00");

            migrationBuilder.UpdateData(
                table: "Museums",
                keyColumn: "MuseumId",
                keyValue: 30,
                column: "WorkingHours",
                value: "09:00-19:00");
        }
    }
}
