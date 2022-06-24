START TRANSACTION;

ALTER TABLE `Users` ADD `HasSyncWeChatUserProfile` tinyint(1) NOT NULL DEFAULT FALSE;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20220624163111_UserAddSyncWeChatProfile', '6.0.5');

COMMIT;

