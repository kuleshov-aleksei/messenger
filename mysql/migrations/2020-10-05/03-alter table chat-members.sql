ALTER TABLE `dcsm`.`chat_members` 
ADD COLUMN `added_by` INT NOT NULL AFTER `join_link`;
ALTER TABLE `dcsm`.`chat_members` 
ADD CONSTRAINT `fk_added_by`
  FOREIGN KEY (`user_id`)
  REFERENCES `dcsm`.`user` (`id`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;
