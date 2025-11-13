using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace G2rismBeta.API.Migrations
{
    /// <inheritdoc />
    public partial class ModuloPreferenciasCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "fecha_actualizacion",
                table: "preferencias_cliente",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "intereses",
                table: "preferencias_cliente",
                type: "json",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "preferencias_alimentacion",
                table: "preferencias_cliente",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "presupuesto_promedio",
                table: "preferencias_cliente",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tipo_alojamiento",
                table: "preferencias_cliente",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "tipo_destino",
                table: "preferencias_cliente",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fecha_actualizacion",
                table: "preferencias_cliente");

            migrationBuilder.DropColumn(
                name: "intereses",
                table: "preferencias_cliente");

            migrationBuilder.DropColumn(
                name: "preferencias_alimentacion",
                table: "preferencias_cliente");

            migrationBuilder.DropColumn(
                name: "presupuesto_promedio",
                table: "preferencias_cliente");

            migrationBuilder.DropColumn(
                name: "tipo_alojamiento",
                table: "preferencias_cliente");

            migrationBuilder.DropColumn(
                name: "tipo_destino",
                table: "preferencias_cliente");
        }
    }
}
