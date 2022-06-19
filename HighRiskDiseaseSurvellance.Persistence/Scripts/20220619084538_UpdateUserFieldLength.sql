START TRANSACTION;

ALTER TABLE `Users` MODIFY COLUMN `DistributorQrCode` longtext CHARACTER SET utf8 COLLATE utf8_general_ci NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20220619084538_UpdateUserFieldLength', '6.0.5');

COMMIT;

