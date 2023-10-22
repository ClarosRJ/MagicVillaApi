using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVillaApi.Migrations
{
    /// <inheritdoc />
    public partial class addCamposDeTableVilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizacion", "FechaCreacion", "ImageUrl", "MetrosCuadrados", "Nombre", "Ocupantes", "Tarifa" },
                values: new object[,]
                {
                    { 1, "", "Detalle de la Villa...", new DateTime(2023, 10, 21, 22, 15, 41, 528, DateTimeKind.Local).AddTicks(7669), new DateTime(2023, 10, 21, 22, 15, 41, 528, DateTimeKind.Local).AddTicks(7650), "", 50, "Villa Real", 5, 200.0 },
                    { 2, "", "New Detalle de la Villa...", new DateTime(2023, 10, 21, 22, 15, 41, 528, DateTimeKind.Local).AddTicks(7674), new DateTime(2023, 10, 21, 22, 15, 41, 528, DateTimeKind.Local).AddTicks(7673), "", 40, "New Villa Real", 4, 400.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
