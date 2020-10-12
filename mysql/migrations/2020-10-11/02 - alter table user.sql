ALTER TABLE `dcsm`.`user` 
CHANGE COLUMN `password_hash` `password_hash` VARCHAR(256) NOT NULL ;

ALTER TABLE `dcsm`.`user` 
ADD COLUMN `salt` VARCHAR(64) NOT NULL AFTER `image_small`;
