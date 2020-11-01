CREATE TABLE `dcsm`.`services` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `title` VARCHAR(255) NOT NULL,
  `description` VARCHAR(255) NOT NULL,
  `address` VARCHAR(45) NOT NULL,
  `settings_port_id` INT NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `settings_port_fk_idx` (`settings_port_id` ASC) VISIBLE,
  CONSTRAINT `settings_port_fk`
    FOREIGN KEY (`settings_port_id`)
    REFERENCES `dcsm`.`settings` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);

INSERT INTO `services` (`id`, `title`, `description`, `address`, `settings_port_id`) VALUES (1, 'Registration serivce', 'Сервис регистрации', '192.168.40.76', 4);
INSERT INTO `services` (`id`, `title`, `description`, `address`, `settings_port_id`) VALUES (2, 'Auth service', 'Сервис авторизации', '192.168.40.76', 5);
INSERT INTO `services` (`id`, `title`, `description`, `address`, `settings_port_id`) VALUES (3, 'Chat service', 'Сервис информации о чатах', '192.168.40.76', 2);
INSERT INTO `services` (`id`, `title`, `description`, `address`, `settings_port_id`) VALUES (4, 'Messenger service', 'Мессенджер - сервис', '192.168.40.43', 3);
INSERT INTO `services` (`id`, `title`, `description`, `address`, `settings_port_id`) VALUES (5, 'Orchestrator service', 'Оркестратор', '192.168.40.43', 6);
