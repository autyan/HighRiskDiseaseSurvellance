START TRANSACTION;

CREATE TABLE `OfficeUsers` (
    `Id` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
    `Name` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
    `PasswordHash` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
    `Salt` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
    `CreateTime` datetime(6) NOT NULL,
    `ModifiedTime` datetime(6) NULL,
    CONSTRAINT `PK_OfficeUsers` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8 COLLATE=utf8_general_ci;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20220605072825_AddOfficeUser', '6.0.5');

COMMIT;

