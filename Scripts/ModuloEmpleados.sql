CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;
ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `permisos` (
    `id_permiso` int NOT NULL AUTO_INCREMENT,
    `modulo` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `accion` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `nombre_permiso` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `descripcion` varchar(200) CHARACTER SET utf8mb4 NULL,
    `estado` tinyint(1) NOT NULL,
    CONSTRAINT `PK_permisos` PRIMARY KEY (`id_permiso`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `roles` (
    `id_rol` int NOT NULL AUTO_INCREMENT,
    `nombre` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `descripcion` varchar(200) CHARACTER SET utf8mb4 NULL,
    `nivel_acceso` int NOT NULL,
    `estado` tinyint(1) NOT NULL,
    `fecha_creacion` datetime(6) NOT NULL,
    `fecha_modificacion` datetime(6) NULL,
    CONSTRAINT `PK_roles` PRIMARY KEY (`id_rol`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `roles_permisos` (
    `id_rol` int NOT NULL,
    `id_permiso` int NOT NULL,
    `fecha_asignacion` datetime(6) NOT NULL,
    CONSTRAINT `pk_roles_permisos` PRIMARY KEY (`id_rol`, `id_permiso`),
    CONSTRAINT `fk_rol_permiso_permiso` FOREIGN KEY (`id_permiso`) REFERENCES `permisos` (`id_permiso`) ON DELETE CASCADE,
    CONSTRAINT `fk_rol_permiso_rol` FOREIGN KEY (`id_rol`) REFERENCES `roles` (`id_rol`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `idx_permiso_modulo_accion` ON `permisos` (`modulo`, `accion`);

CREATE UNIQUE INDEX `idx_permiso_nombre_unique` ON `permisos` (`nombre_permiso`);

CREATE INDEX `idx_rol_nivel_acceso` ON `roles` (`nivel_acceso`);

CREATE UNIQUE INDEX `idx_rol_nombre_unique` ON `roles` (`nombre`);

CREATE INDEX `IX_roles_permisos_id_permiso` ON `roles_permisos` (`id_permiso`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20251028133800_InitialCreate_RolesPermisos', '9.0.9');

CREATE TABLE `usuarios` (
    `id_usuario` int NOT NULL AUTO_INCREMENT,
    `username` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `email` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `password_hash` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `tipo_usuario` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `id_referencia` int NULL,
    `ultimo_acceso` datetime(6) NULL,
    `intentos_fallidos` int NOT NULL,
    `bloqueado` tinyint(1) NOT NULL,
    `estado` tinyint(1) NOT NULL,
    `fecha_creacion` datetime(6) NOT NULL,
    `fecha_modificacion` datetime(6) NULL,
    CONSTRAINT `PK_usuarios` PRIMARY KEY (`id_usuario`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `tokens_recuperacion` (
    `id_token` int NOT NULL AUTO_INCREMENT,
    `id_usuario` int NOT NULL,
    `token` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `tipo_token` longtext CHARACTER SET utf8mb4 NOT NULL,
    `fecha_generacion` datetime(6) NOT NULL,
    `fecha_expiracion` datetime(6) NOT NULL,
    `usado` tinyint(1) NOT NULL,
    `fecha_uso` datetime(6) NULL,
    `ip_solicitud` varchar(45) CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_tokens_recuperacion` PRIMARY KEY (`id_token`),
    CONSTRAINT `fk_token_usuario` FOREIGN KEY (`id_usuario`) REFERENCES `usuarios` (`id_usuario`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `usuarios_roles` (
    `id_usuario` int NOT NULL,
    `id_rol` int NOT NULL,
    `fecha_asignacion` datetime(6) NOT NULL,
    `fecha_expiracion` datetime(6) NULL,
    `asignado_por` int NULL,
    CONSTRAINT `pk_usuarios_roles` PRIMARY KEY (`id_usuario`, `id_rol`),
    CONSTRAINT `fk_usuario_rol_rol` FOREIGN KEY (`id_rol`) REFERENCES `roles` (`id_rol`) ON DELETE CASCADE,
    CONSTRAINT `fk_usuario_rol_usuario` FOREIGN KEY (`id_usuario`) REFERENCES `usuarios` (`id_usuario`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `idx_token_expiracion` ON `tokens_recuperacion` (`fecha_expiracion`);

CREATE UNIQUE INDEX `idx_token_unique` ON `tokens_recuperacion` (`token`);

CREATE INDEX `idx_token_usuario` ON `tokens_recuperacion` (`id_usuario`);

CREATE UNIQUE INDEX `idx_usuario_email_unique` ON `usuarios` (`email`);

CREATE INDEX `idx_usuario_estado` ON `usuarios` (`estado`);

CREATE INDEX `idx_usuario_tipo` ON `usuarios` (`tipo_usuario`);

CREATE UNIQUE INDEX `idx_usuario_username_unique` ON `usuarios` (`username`);

CREATE INDEX `idx_usuario_rol_rol` ON `usuarios_roles` (`id_rol`);

CREATE INDEX `idx_usuario_rol_usuario` ON `usuarios_roles` (`id_usuario`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20251031133411_SecondCreateUsuarios', '9.0.9');

CREATE TABLE `categorias_cliente` (
    `id_categoria` int NOT NULL AUTO_INCREMENT,
    `nombre` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `descripcion` varchar(500) CHARACTER SET utf8mb4 NULL,
    `color_hex` varchar(7) CHARACTER SET utf8mb4 NULL,
    `beneficios` varchar(1000) CHARACTER SET utf8mb4 NULL,
    `criterios_clasificacion` varchar(500) CHARACTER SET utf8mb4 NULL,
    `descuento_porcentaje` decimal(65,30) NOT NULL,
    `estado` tinyint(1) NOT NULL,
    CONSTRAINT `PK_categorias_cliente` PRIMARY KEY (`id_categoria`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `clientes` (
    `id_cliente` int NOT NULL AUTO_INCREMENT,
    `id_usuario` int NOT NULL,
    `id_categoria` int NULL,
    `nombre` longtext CHARACTER SET utf8mb4 NOT NULL,
    `apellido` longtext CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_clientes` PRIMARY KEY (`id_cliente`),
    CONSTRAINT `fk_cliente_categoria` FOREIGN KEY (`id_categoria`) REFERENCES `categorias_cliente` (`id_categoria`) ON DELETE RESTRICT,
    CONSTRAINT `fk_cliente_usuario` FOREIGN KEY (`id_usuario`) REFERENCES `usuarios` (`id_usuario`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `preferencias_cliente` (
    `id_preferencia` int NOT NULL AUTO_INCREMENT,
    `id_cliente` int NOT NULL,
    CONSTRAINT `PK_preferencias_cliente` PRIMARY KEY (`id_preferencia`),
    CONSTRAINT `fk_preferencia_cliente` FOREIGN KEY (`id_cliente`) REFERENCES `clientes` (`id_cliente`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `idx_categoria_cliente_descuento` ON `categorias_cliente` (`descuento_porcentaje`);

CREATE INDEX `idx_categoria_cliente_estado` ON `categorias_cliente` (`estado`);

CREATE UNIQUE INDEX `idx_categoria_cliente_nombre_unique` ON `categorias_cliente` (`nombre`);

CREATE INDEX `IX_clientes_id_categoria` ON `clientes` (`id_categoria`);

CREATE INDEX `IX_clientes_id_usuario` ON `clientes` (`id_usuario`);

CREATE UNIQUE INDEX `idx_preferencia_cliente` ON `preferencias_cliente` (`id_cliente`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20251107002209_ModuloCategoriasCliente', '9.0.9');

ALTER TABLE `clientes` RENAME INDEX `IX_clientes_id_categoria` TO `idx_cliente_categoria`;

ALTER TABLE `clientes` MODIFY COLUMN `nombre` varchar(100) CHARACTER SET utf8mb4 NOT NULL;

ALTER TABLE `clientes` MODIFY COLUMN `apellido` varchar(100) CHARACTER SET utf8mb4 NOT NULL;

ALTER TABLE `clientes` ADD `ciudad` varchar(100) CHARACTER SET utf8mb4 NOT NULL DEFAULT '';

ALTER TABLE `clientes` ADD `correo_electronico` varchar(150) CHARACTER SET utf8mb4 NOT NULL DEFAULT '';

ALTER TABLE `clientes` ADD `direccion` varchar(200) CHARACTER SET utf8mb4 NULL;

ALTER TABLE `clientes` ADD `documento_identidad` varchar(50) CHARACTER SET utf8mb4 NOT NULL DEFAULT '';

ALTER TABLE `clientes` ADD `estado` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `clientes` ADD `fecha_nacimiento` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00';

ALTER TABLE `clientes` ADD `fecha_registro` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00';

ALTER TABLE `clientes` ADD `pais` varchar(100) CHARACTER SET utf8mb4 NOT NULL DEFAULT '';

ALTER TABLE `clientes` ADD `telefono` varchar(20) CHARACTER SET utf8mb4 NOT NULL DEFAULT '';

ALTER TABLE `clientes` ADD `tipo_documento` varchar(10) CHARACTER SET utf8mb4 NOT NULL DEFAULT '';

CREATE INDEX `idx_cliente_apellido_nombre` ON `clientes` (`apellido`, `nombre`);

CREATE INDEX `idx_cliente_ciudad` ON `clientes` (`ciudad`);

CREATE UNIQUE INDEX `idx_cliente_documento_unique` ON `clientes` (`documento_identidad`);

CREATE INDEX `idx_cliente_email` ON `clientes` (`correo_electronico`);

CREATE INDEX `idx_cliente_estado` ON `clientes` (`estado`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20251107123658_ModuloCliente', '9.0.9');

ALTER TABLE `preferencias_cliente` ADD `fecha_actualizacion` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00';

ALTER TABLE `preferencias_cliente` ADD `intereses` json NULL;

ALTER TABLE `preferencias_cliente` ADD `preferencias_alimentacion` varchar(100) CHARACTER SET utf8mb4 NULL;

ALTER TABLE `preferencias_cliente` ADD `presupuesto_promedio` decimal(10,2) NULL;

ALTER TABLE `preferencias_cliente` ADD `tipo_alojamiento` varchar(50) CHARACTER SET utf8mb4 NULL;

ALTER TABLE `preferencias_cliente` ADD `tipo_destino` varchar(50) CHARACTER SET utf8mb4 NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20251109175531_ModuloPreferenciasCliente', '9.0.9');

CREATE TABLE `empleados` (
    `id_empleado` int NOT NULL AUTO_INCREMENT,
    `id_usuario` int NOT NULL,
    `id_jefe` int NULL,
    `nombre` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `apellido` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `documento_identidad` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
    `tipo_documento` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
    `fecha_nacimiento` DATE NOT NULL,
    `correo_electronico` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `telefono` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
    `cargo` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `fecha_ingreso` DATE NOT NULL,
    `salario` DECIMAL(10,2) NOT NULL,
    `estado` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_empleados` PRIMARY KEY (`id_empleado`),
    CONSTRAINT `fk_empleado_jefe` FOREIGN KEY (`id_jefe`) REFERENCES `empleados` (`id_empleado`) ON DELETE RESTRICT,
    CONSTRAINT `fk_empleado_usuario` FOREIGN KEY (`id_usuario`) REFERENCES `usuarios` (`id_usuario`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE INDEX `idx_empleado_apellido_nombre` ON `empleados` (`apellido`, `nombre`);

CREATE INDEX `idx_empleado_cargo` ON `empleados` (`cargo`);

CREATE UNIQUE INDEX `idx_empleado_documento_unique` ON `empleados` (`documento_identidad`);

CREATE INDEX `idx_empleado_email` ON `empleados` (`correo_electronico`);

CREATE INDEX `idx_empleado_estado` ON `empleados` (`estado`);

CREATE INDEX `idx_empleado_jefe` ON `empleados` (`id_jefe`);

CREATE INDEX `IX_empleados_id_usuario` ON `empleados` (`id_usuario`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20251110042441_ModuloEmpleados', '9.0.9');

COMMIT;

