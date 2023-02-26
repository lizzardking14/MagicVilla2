using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class AgregarNuevaBaseDeDatos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 2, 25, 20, 12, 4, 36, DateTimeKind.Local).AddTicks(936), new DateTime(2023, 2, 25, 20, 12, 4, 36, DateTimeKind.Local).AddTicks(924) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 2, 25, 20, 12, 4, 36, DateTimeKind.Local).AddTicks(939), new DateTime(2023, 2, 25, 20, 12, 4, 36, DateTimeKind.Local).AddTicks(939) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 2, 25, 20, 2, 6, 801, DateTimeKind.Local).AddTicks(2237), new DateTime(2023, 2, 25, 20, 2, 6, 801, DateTimeKind.Local).AddTicks(2223) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 2, 25, 20, 2, 6, 801, DateTimeKind.Local).AddTicks(2239), new DateTime(2023, 2, 25, 20, 2, 6, 801, DateTimeKind.Local).AddTicks(2239) });
        }
    }
}
