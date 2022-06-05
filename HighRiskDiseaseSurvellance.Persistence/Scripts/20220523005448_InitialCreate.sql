CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8;

CREATE TABLE `Orders` (
    `Id` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
    `OrderNumber` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
    `OrderPayStatus` int NOT NULL,
    `UserInfo_WeChatOpenId` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
    `UserInfo_NickName` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
    `UserInfo_PhoneNumber` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
    `UserId` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
    `Price` decimal(65,30) NOT NULL,
    `CreateTime` datetime(6) NOT NULL,
    `ModifiedTime` datetime(6) NULL,
    CONSTRAINT `PK_Orders` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8 COLLATE=utf8_general_ci;

CREATE TABLE `Records` (
    `Id` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
    `UserInfo_WeChatOpenId` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
    `UserInfo_NickName` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
    `UserInfo_PhoneNumber` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
    `SurveillanceContent` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
    `SurveillanceTypeName` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
    `OrderId` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
    `UserId` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
    `Status` int NOT NULL,
    `CreateTime` datetime(6) NOT NULL,
    `ModifiedTime` datetime(6) NULL,
    CONSTRAINT `PK_Records` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8 COLLATE=utf8_general_ci;

CREATE TABLE `Users` (
    `Id` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
    `NickName` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
    `AvatarUrl` varchar(2048) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
    `PhoneNumber` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
    `WeChatOpenId` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
    `DistributorId` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
    `IsDistributor` tinyint(1) NOT NULL,
    `DistributorQrCode` varchar(1024) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
    `CreateTime` datetime(6) NOT NULL,
    `ModifiedTime` datetime(6) NULL,
    CONSTRAINT `PK_Users` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8 COLLATE=utf8_general_ci;

CREATE INDEX `IX_Orders_UserId` ON `Orders` (`UserId`);

CREATE INDEX `IX_Records_UserId` ON `Records` (`UserId`);

CREATE INDEX `IX_Users_PhoneNumber` ON `Users` (`PhoneNumber`);

CREATE INDEX `IX_Users_WeChatOpenId` ON `Users` (`WeChatOpenId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20220523005448_InitialCreate', '6.0.5');

COMMIT;

