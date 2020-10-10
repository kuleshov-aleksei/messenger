ALTER TABLE `dcsm`.`user` 
ADD COLUMN `image_large` VARCHAR(255) NULL AFTER `password_hash`,
ADD COLUMN `image_medium` VARCHAR(255) NULL AFTER `image_large`,
ADD COLUMN `image_small` VARCHAR(255) NULL AFTER `image_medium`;
