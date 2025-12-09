using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace G2rismBeta.API.Migrations
{
    /// <inheritdoc />
    public partial class AgregarReservasVuelos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "reservas_vuelos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_reserva = table.Column<int>(type: "int", nullable: false),
                    id_vuelo = table.Column<int>(type: "int", nullable: false),
                    numero_pasajeros = table.Column<int>(type: "int", nullable: false),
                    clase = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    asientos_asignados = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    precio_por_pasajero = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: false),
                    subtotal = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: false),
                    equipaje_incluido = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    equipaje_extra = table.Column<int>(type: "int", nullable: true),
                    costo_equipaje_extra = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: false),
                    fecha_agregado = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservas_vuelos", x => x.id);
                    table.ForeignKey(
                        name: "fk_reserva_vuelo_reserva",
                        column: x => x.id_reserva,
                        principalTable: "reservas",
                        principalColumn: "id_reserva",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_reserva_vuelo_vuelo",
                        column: x => x.id_vuelo,
                        principalTable: "vuelos",
                        principalColumn: "id_vuelo",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "idx_reserva_vuelo_clase",
                table: "reservas_vuelos",
                column: "clase");

            migrationBuilder.CreateIndex(
                name: "idx_reserva_vuelo_reserva",
                table: "reservas_vuelos",
                column: "id_reserva");

            migrationBuilder.CreateIndex(
                name: "idx_reserva_vuelo_vuelo",
                table: "reservas_vuelos",
                column: "id_vuelo");

            migrationBuilder.CreateIndex(
                name: "idx_reserva_vuelo_vuelo_reserva",
                table: "reservas_vuelos",
                columns: new[] { "id_vuelo", "id_reserva" });

            migrationBuilder.CreateIndex(
                name: "idx_reservavuelo_reserva",
                table: "reservas_vuelos",
                column: "id_reserva");

            migrationBuilder.CreateIndex(
                name: "idx_reservavuelo_vuelo",
                table: "reservas_vuelos",
                column: "id_vuelo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reservas_vuelos");
        }
    }
}
