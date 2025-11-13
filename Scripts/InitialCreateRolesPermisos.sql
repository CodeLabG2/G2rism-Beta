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

COMMIT;

