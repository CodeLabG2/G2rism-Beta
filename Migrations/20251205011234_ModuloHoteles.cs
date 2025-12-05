using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace G2rismBeta.API.Migrations
{
    /// <inheritdoc />
    public partial class ModuloHoteles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "hoteles",
                columns: table => new
                {
                    id_hotel = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_proveedor = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ciudad = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    pais = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    direccion = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    contacto = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    descripcion = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    categoria = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    estrellas = table.Column<int>(type: "int", nullable: true),
                    precio_por_noche = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    capacidad_por_habitacion = table.Column<int>(type: "int", nullable: true),
                    numero_habitaciones = table.Column<int>(type: "int", nullable: true),
                    tiene_wifi = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    tiene_piscina = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    tiene_restaurante = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    tiene_gimnasio = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    tiene_parqueadero = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    politicas_cancelacion = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    check_in_hora = table.Column<TimeSpan>(type: "time", nullable: true),
                    check_out_hora = table.Column<TimeSpan>(type: "time", nullable: true),
                    fotos = table.Column<string>(type: "json", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    servicios_incluidos = table.Column<string>(type: "json", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    estado = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hoteles", x => x.id_hotel);
                    table.ForeignKey(
                        name: "fk_hotel_proveedor",
                        column: x => x.id_proveedor,
                        principalTable: "proveedores",
                        principalColumn: "id_proveedor",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "idx_hotel_categoria",
                table: "hoteles",
                column: "categoria");

            migrationBuilder.CreateIndex(
                name: "idx_hotel_ciudad",
                table: "hoteles",
                column: "ciudad");

            migrationBuilder.CreateIndex(
                name: "idx_hotel_estado",
                table: "hoteles",
                column: "estado");

            migrationBuilder.CreateIndex(
                name: "idx_hotel_estrellas",
                table: "hoteles",
                column: "estrellas");

            migrationBuilder.CreateIndex(
                name: "idx_hotel_nombre_ciudad_unique",
                table: "hoteles",
                columns: new[] { "nombre", "ciudad" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_hotel_pais",
                table: "hoteles",
                column: "pais");

            migrationBuilder.CreateIndex(
                name: "idx_hotel_precio",
                table: "hoteles",
                column: "precio_por_noche");

            migrationBuilder.CreateIndex(
                name: "idx_hotel_proveedor",
                table: "hoteles",
                column: "id_proveedor");

            migrationBuilder.CreateIndex(
                name: "idx_hotel_servicios_premium",
                table: "hoteles",
                columns: new[] { "tiene_piscina", "tiene_gimnasio", "tiene_restaurante" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "hoteles");
        }
    }
}
