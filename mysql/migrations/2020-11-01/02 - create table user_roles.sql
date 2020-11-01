CREATE TABLE `dcsm`.`user_roles` (
  `user_id` INT NOT NULL,
  `role_id` INT NOT NULL,
  `assigned_by` INT NOT NULL,
  `date_assigned` DATETIME NOT NULL,
  PRIMARY KEY (`user_id`),
  INDEX `assigned_by_idx` (`assigned_by` ASC) VISIBLE,
  INDEX `role_id_fk_idx` (`role_id` ASC) VISIBLE,
  CONSTRAINT `assigned_by_fk`
    FOREIGN KEY (`assigned_by`)
    REFERENCES `dcsm`.`user` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `user_id_fk2`
    FOREIGN KEY (`user_id`)
    REFERENCES `dcsm`.`user` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `role_id_fk`
    FOREIGN KEY (`role_id`)
    REFERENCES `dcsm`.`user_role` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);


INSERT INTO `dcsm`.`user_roles` (`user_id`, `role_id`, `assigned_by`, `date_assigned`) VALUES ('2', '1', '2', '2020-11-01 16:16:48');

ALTER TABLE `dcsm`.`user_roles` 
DROP PRIMARY KEY,
ADD PRIMARY KEY (`user_id`, `role_id`);
;
