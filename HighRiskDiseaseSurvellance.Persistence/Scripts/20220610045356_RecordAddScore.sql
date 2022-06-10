START TRANSACTION;

ALTER TABLE `Records` ADD `Score` decimal(65,30) NOT NULL DEFAULT 0.0;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20220610045356_RecordAddScore', '6.0.5');

COMMIT;

