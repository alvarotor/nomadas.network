using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nomadas.Network.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeatherForecast",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    TemperatureC = table.Column<int>(nullable: false),
                    Summary = table.Column<string>(nullable: false),
                    RandomString = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherForecast", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "WeatherForecast",
                columns: new[] { "Id", "Date", "RandomString", "Summary", "TemperatureC" },
                values: new object[,]
                {
                    { 1, new DateTime(2020, 1, 30, 16, 16, 49, 241, DateTimeKind.Local).AddTicks(5335), "TMAZ0JB72H", "Freezing", 43 },
                    { 2, new DateTime(2020, 1, 31, 16, 16, 49, 247, DateTimeKind.Local).AddTicks(7543), "AUIFTJQS6J", "Scorching", -16 },
                    { 3, new DateTime(2020, 2, 1, 16, 16, 49, 247, DateTimeKind.Local).AddTicks(7860), "CLDKZ33UE8", "Mild", 45 },
                    { 4, new DateTime(2020, 2, 2, 16, 16, 49, 247, DateTimeKind.Local).AddTicks(7897), "D8AKIQ7MYX", "Cool", 13 },
                    { 5, new DateTime(2020, 2, 3, 16, 16, 49, 247, DateTimeKind.Local).AddTicks(7922), "IQIF8OB9VG", "Scorching", 51 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeatherForecast");
        }
    }
}
