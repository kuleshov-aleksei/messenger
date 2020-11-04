ALTER TABLE `dcsm`.`user_role` 
ADD COLUMN `description` VARCHAR(45) NULL AFTER `role`;

UPDATE `dcsm`.`user_role` SET `description`='Система' WHERE  `id`=1;
SELECT `id`, `role`, `description` FROM `dcsm`.`user_role` WHERE  `id`=1;
UPDATE `dcsm`.`user_role` SET `description`='Администратор' WHERE  `id`=2;
SELECT `id`, `role`, `description` FROM `dcsm`.`user_role` WHERE  `id`=2;
UPDATE `dcsm`.`user_role` SET `description`='Пользователь' WHERE  `id`=3;
SELECT `id`, `role`, `description` FROM `dcsm`.`user_role` WHERE  `id`=3;