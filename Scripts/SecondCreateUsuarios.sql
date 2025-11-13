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

COMMIT;

