using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace G2rismBeta.API.Migrations
{
    /// <inheritdoc />
    public partial class ModuloReservas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "reservas",
                columns: table => new
                {
                    id_reserva = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_cliente = table.Column<int>(type: "int", nullable: false),
                    id_empleado = table.Column<int>(type: "int", nullable: false),
                    descripcion = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_inicio_viaje = table.Column<DateTime>(type: "DATE", nullable: false),
                    fecha_fin_viaje = table.Column<DateTime>(type: "DATE", nullable: false),
                    numero_pasajeros = table.Column<int>(type: "int", nullable: false),
                    monto_total = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: false),
                    monto_pagado = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: false),
                    saldo_pendiente = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: false),
                    estado = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    observaciones = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_hora = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservas", x => x.id_reserva);
                    table.ForeignKey(
                        name: "fk_reserva_cliente",
                        column: x => x.id_cliente,
                        principalTable: "clientes",
                        principalColumn: "id_cliente",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_reserva_empleado",
                        column: x => x.id_empleado,
                        principalTable: "empleados",
                        principalColumn: "id_empleado",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "idx_reserva_cliente",
                table: "reservas",
                column: "id_cliente");

            migrationBuilder.CreateIndex(
                name: "idx_reserva_cliente_estado_fecha",
                table: "reservas",
                columns: new[] { "id_cliente", "estado", "fecha_inicio_viaje" });

            migrationBuilder.CreateIndex(
                name: "idx_reserva_empleado",
                table: "reservas",
                column: "id_empleado");

            migrationBuilder.CreateIndex(
                name: "idx_reserva_estado",
                table: "reservas",
                column: "estado");

            migrationBuilder.CreateIndex(
                name: "idx_reserva_fecha_creacion",
                table: "reservas",
                column: "fecha_hora");

            migrationBuilder.CreateIndex(
                name: "idx_reserva_fecha_fin",
                table: "reservas",
                column: "fecha_fin_viaje");

            migrationBuilder.CreateIndex(
                name: "idx_reserva_fecha_inicio",
                table: "reservas",
                column: "fecha_inicio_viaje");

            migrationBuilder.CreateIndex(
                name: "idx_reserva_fechas_estado",
                table: "reservas",
                columns: new[] { "fecha_inicio_viaje", "fecha_fin_viaje", "estado" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reservas");
        }
    }
}
