CREATE TABLE `dcsm`.`user_role` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `role` VARCHAR(45) NOT NULL,
  `date_assign` DATETIME NOT NULL,
  `assign_by` INT NULL,
  PRIMARY KEY (`id`),
  INDEX `assign_by_fk_idx` (`assign_by` ASC) VISIBLE,
  CONSTRAINT `assign_by_fk`
    FOREIGN KEY (`assign_by`)
    REFERENCES `dcsm`.`user` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);

ALTER TABLE `dcsm`.`user_role` 
DROP FOREIGN KEY `assign_by_fk`;
ALTER TABLE `dcsm`.`user_role` 
DROP COLUMN `assign_by`,
DROP COLUMN `date_assign`,
DROP INDEX `assign_by_fk_idx` ;
;

INSERT INTO `dcsm`.`user_role` (`id`, `role`) VALUES ('1', 'SYSTEM');
INSERT INTO `dcsm`.`user_role` (`id`, `role`) VALUES ('2', 'admin');
INSERT INTO `dcsm`.`user_role` (`id`, `role`) VALUES ('3', 'regular');
