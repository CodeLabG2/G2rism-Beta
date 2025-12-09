using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace G2rismBeta.API.Migrations
{
    /// <inheritdoc />
    public partial class ModuloFinanciero : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reservas_servicios_reservas_id_reserva",
                table: "reservas_servicios");

            migrationBuilder.DropForeignKey(
                name: "FK_reservas_servicios_servicios_adicionales_id_servicio",
                table: "reservas_servicios");

            migrationBuilder.CreateTable(
                name: "facturas",
                columns: table => new
                {
                    id_factura = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_reserva = table.Column<int>(type: "int", nullable: false),
                    numero_factura = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_emision = table.Column<DateTime>(type: "DATE", nullable: false),
                    fecha_vencimiento = table.Column<DateTime>(type: "DATE", nullable: true),
                    resolucion_dian = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cufe_cude = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tipo_factura = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    estado = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    subtotal = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: false),
                    impuestos = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: false),
                    porcentaje_iva = table.Column<decimal>(type: "DECIMAL(5,2)", nullable: false),
                    descuentos = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: false),
                    total = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: false),
                    observaciones = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_creacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ReservaIdReserva = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_facturas", x => x.id_factura);
                    table.ForeignKey(
                        name: "FK_facturas_reservas_ReservaIdReserva",
                        column: x => x.ReservaIdReserva,
                        principalTable: "reservas",
                        principalColumn: "id_reserva");
                    table.ForeignKey(
                        name: "fk_factura_reserva",
                        column: x => x.id_reserva,
                        principalTable: "reservas",
                        principalColumn: "id_reserva",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "formas_de_pago",
                columns: table => new
                {
                    id_forma_pago = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    metodo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    descripcion = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    requiere_verificacion = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    activo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_formas_de_pago", x => x.id_forma_pago);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "pagos",
                columns: table => new
                {
                    id_pago = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_factura = table.Column<int>(type: "int", nullable: false),
                    id_forma_pago = table.Column<int>(type: "int", nullable: false),
                    monto = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: false),
                    fecha_pago = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    referencia_transaccion = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    comprobante_pago = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    estado = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    observaciones = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_creacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pagos", x => x.id_pago);
                    table.ForeignKey(
                        name: "fk_pago_factura",
                        column: x => x.id_factura,
                        principalTable: "facturas",
                        principalColumn: "id_factura",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_pago_forma_pago",
                        column: x => x.id_forma_pago,
                        principalTable: "formas_de_pago",
                        principalColumn: "id_forma_pago",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "idx_reserva_servicio_fecha",
                table: "reservas_servicios",
                column: "fecha_servicio");

            migrationBuilder.CreateIndex(
                name: "idx_reserva_servicio_reserva",
                table: "reservas_servicios",
                column: "id_reserva");

            migrationBuilder.CreateIndex(
                name: "idx_reserva_servicio_servicio",
                table: "reservas_servicios",
                column: "id_servicio");

            migrationBuilder.CreateIndex(
                name: "idx_reserva_servicio_servicio_reserva",
                table: "reservas_servicios",
                columns: new[] { "id_servicio", "id_reserva" });

            migrationBuilder.CreateIndex(
                name: "idx_factura_estado",
                table: "facturas",
                column: "estado");

            migrationBuilder.CreateIndex(
                name: "idx_factura_estado_vencimiento",
                table: "facturas",
                columns: new[] { "estado", "fecha_vencimiento" });

            migrationBuilder.CreateIndex(
                name: "idx_factura_fecha_emision",
                table: "facturas",
                column: "fecha_emision");

            migrationBuilder.CreateIndex(
                name: "idx_factura_fecha_vencimiento",
                table: "facturas",
                column: "fecha_vencimiento");

            migrationBuilder.CreateIndex(
                name: "idx_factura_numero_unique",
                table: "facturas",
                column: "numero_factura",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_factura_reserva",
                table: "facturas",
                column: "id_reserva");

            migrationBuilder.CreateIndex(
                name: "IX_facturas_ReservaIdReserva",
                table: "facturas",
                column: "ReservaIdReserva");

            migrationBuilder.CreateIndex(
                name: "idx_forma_pago_activo",
                table: "formas_de_pago",
                column: "activo");

            migrationBuilder.CreateIndex(
                name: "idx_forma_pago_metodo_unique",
                table: "formas_de_pago",
                column: "metodo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_pago_estado",
                table: "pagos",
                column: "estado");

            migrationBuilder.CreateIndex(
                name: "idx_pago_factura",
                table: "pagos",
                column: "id_factura");

            migrationBuilder.CreateIndex(
                name: "idx_pago_factura_estado_fecha",
                table: "pagos",
                columns: new[] { "id_factura", "estado", "fecha_pago" });

            migrationBuilder.CreateIndex(
                name: "idx_pago_fecha_pago",
                table: "pagos",
                column: "fecha_pago");

            migrationBuilder.CreateIndex(
                name: "idx_pago_forma_pago",
                table: "pagos",
                column: "id_forma_pago");

            migrationBuilder.AddForeignKey(
                name: "fk_reserva_servicio_reserva",
                table: "reservas_servicios",
                column: "id_reserva",
                principalTable: "reservas",
                principalColumn: "id_reserva",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_reserva_servicio_servicio",
                table: "reservas_servicios",
                column: "id_servicio",
                principalTable: "servicios_adicionales",
                principalColumn: "id_servicio",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_reserva_servicio_reserva",
                table: "reservas_servicios");

            migrationBuilder.DropForeignKey(
                name: "fk_reserva_servicio_servicio",
                table: "reservas_servicios");

            migrationBuilder.DropTable(
                name: "pagos");

            migrationBuilder.DropTable(
                name: "facturas");

            migrationBuilder.DropTable(
                name: "formas_de_pago");

            migrationBuilder.DropIndex(
                name: "idx_reserva_servicio_fecha",
                table: "reservas_servicios");

            migrationBuilder.DropIndex(
                name: "idx_reserva_servicio_reserva",
                table: "reservas_servicios");

            migrationBuilder.DropIndex(
                name: "idx_reserva_servicio_servicio",
                table: "reservas_servicios");

            migrationBuilder.DropIndex(
                name: "idx_reserva_servicio_servicio_reserva",
                table: "reservas_servicios");

            migrationBuilder.AddForeignKey(
                name: "FK_reservas_servicios_reservas_id_reserva",
                table: "reservas_servicios",
                column: "id_reserva",
                principalTable: "reservas",
                principalColumn: "id_reserva",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_reservas_servicios_servicios_adicionales_id_servicio",
                table: "reservas_servicios",
                column: "id_servicio",
                principalTable: "servicios_adicionales",
                principalColumn: "id_servicio",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
