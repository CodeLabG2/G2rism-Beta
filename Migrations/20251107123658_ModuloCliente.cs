using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace G2rismBeta.API.Migrations
{
    /// <inheritdoc />
    public partial class ModuloCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_clientes_id_categoria",
                table: "clientes",
                newName: "idx_cliente_categoria");

            migrationBuilder.AlterColumn<string>(
                name: "nombre",
                table: "clientes",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "apellido",
                table: "clientes",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ciudad",
                table: "clientes",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "correo_electronico",
                table: "clientes",
                type: "varchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "direccion",
                table: "clientes",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "documento_identidad",
                table: "clientes",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "estado",
                table: "clientes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "fecha_nacimiento",
                table: "clientes",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "fecha_registro",
                table: "clientes",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "pais",
                table: "clientes",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "telefono",
                table: "clientes",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "tipo_documento",
                table: "clientes",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "idx_cliente_apellido_nombre",
                table: "clientes",
                columns: new[] { "apellido", "nombre" });

            migrationBuilder.CreateIndex(
                name: "idx_cliente_ciudad",
                table: "clientes",
                column: "ciudad");

            migrationBuilder.CreateIndex(
                name: "idx_cliente_documento_unique",
                table: "clientes",
                column: "documento_identidad",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_cliente_email",
                table: "clientes",
                column: "correo_electronico");

            migrationBuilder.CreateIndex(
                name: "idx_cliente_estado",
                table: "clientes",
                column: "estado");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_cliente_apellido_nombre",
                table: "clientes");

            migrationBuilder.DropIndex(
                name: "idx_cliente_ciudad",
                table: "clientes");

            migrationBuilder.DropIndex(
                name: "idx_cliente_documento_unique",
                table: "clientes");

            migrationBuilder.DropIndex(
                name: "idx_cliente_email",
                table: "clientes");

            migrationBuilder.DropIndex(
                name: "idx_cliente_estado",
                table: "clientes");

            migrationBuilder.DropColumn(
                name: "ciudad",
                table: "clientes");

            migrationBuilder.DropColumn(
                name: "correo_electronico",
                table: "clientes");

            migrationBuilder.DropColumn(
                name: "direccion",
                table: "clientes");

            migrationBuilder.DropColumn(
                name: "documento_identidad",
                table: "clientes");

            migrationBuilder.DropColumn(
                name: "estado",
                table: "clientes");

            migrationBuilder.DropColumn(
                name: "fecha_nacimiento",
                table: "clientes");

            migrationBuilder.DropColumn(
                name: "fecha_registro",
                table: "clientes");

            migrationBuilder.DropColumn(
                name: "pais",
                table: "clientes");

            migrationBuilder.DropColumn(
                name: "telefono",
                table: "clientes");

            migrationBuilder.DropColumn(
                name: "tipo_documento",
                table: "clientes");

            migrationBuilder.RenameIndex(
                name: "idx_cliente_categoria",
                table: "clientes",
                newName: "IX_clientes_id_categoria");

            migrationBuilder.AlterColumn<string>(
                name: "nombre",
                table: "clientes",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "apellido",
                table: "clientes",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
