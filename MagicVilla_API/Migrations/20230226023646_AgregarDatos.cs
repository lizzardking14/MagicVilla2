using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class AgregarDatos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 2, 25, 20, 36, 46, 393, DateTimeKind.Local).AddTicks(8183), new DateTime(2023, 2, 25, 20, 36, 46, 393, DateTimeKind.Local).AddTicks(8170) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 2, 25, 20, 36, 46, 393, DateTimeKind.Local).AddTicks(8185), new DateTime(2023, 2, 25, 20, 36, 46, 393, DateTimeKind.Local).AddTicks(8185) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 2, 25, 20, 30, 43, 462, DateTimeKind.Local).AddTicks(9857), new DateTime(2023, 2, 25, 20, 30, 43, 462, DateTimeKind.Local).AddTicks(9844) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 2, 25, 20, 30, 43, 462, DateTimeKind.Local).AddTicks(9859), new DateTime(2023, 2, 25, 20, 30, 43, 462, DateTimeKind.Local).AddTicks(9859) });
        }
    }
}
