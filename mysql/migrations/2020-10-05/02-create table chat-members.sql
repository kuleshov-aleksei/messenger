CREATE TABLE `dcsm`.`chat_members` (
  `chat_id` INT NOT NULL,
  `user_id` INT NOT NULL,
  `joined_at` DATETIME NULL,
  `join_link` VARCHAR(45) NULL,
  PRIMARY KEY (`chat_id`, `user_id`),
  INDEX `fk_user_id_idx` (`user_id` ASC) VISIBLE,
  CONSTRAINT `fk_chat_id`
    FOREIGN KEY (`chat_id`)
    REFERENCES `dcsm`.`chat` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_user_id`
    FOREIGN KEY (`user_id`)
    REFERENCES `dcsm`.`user` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);
