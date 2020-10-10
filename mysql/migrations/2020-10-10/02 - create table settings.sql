CREATE TABLE `dcsm`.`setting` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(128) NOT NULL,
  `value` VARCHAR(512) NOT NULL,
  `description` VARCHAR(512) NULL,
  PRIMARY KEY (`id`));

INSERT INTO `dcsm`.`setting` (`name`, `value`, `description`) VALUES ('es_address', 'http://192.168.40.76', 'Elasic search address');
INSERT INTO `dcsm`.`setting` (`name`, `value`, `description`) VALUES ('service_chat_info_port', '23578', 'Port for chat info service');
INSERT INTO `dcsm`.`setting` (`name`, `value`, `description`) VALUES ('service_messenger_port', '23579', 'Port for messenger server');
