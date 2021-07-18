-- --------------------------------------------------------
-- Хост:                         127.0.0.1
-- Версия сервера:               8.0.25 - MySQL Community Server - GPL
-- Операционная система:         Linux
-- HeidiSQL Версия:              11.3.0.6295
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Дамп структуры базы данных dcsm
CREATE DATABASE IF NOT EXISTS `dcsm` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `dcsm`;

-- Дамп структуры для таблица dcsm.chat
CREATE TABLE IF NOT EXISTS `chat` (
  `id` int NOT NULL AUTO_INCREMENT,
  `title` varchar(255) NOT NULL,
  `image_medium` varchar(1024) DEFAULT NULL,
  `image_small` varchar(1024) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Дамп данных таблицы dcsm.chat: ~0 rows (приблизительно)
/*!40000 ALTER TABLE `chat` DISABLE KEYS */;
/*!40000 ALTER TABLE `chat` ENABLE KEYS */;

-- Дамп структуры для таблица dcsm.chat_members
CREATE TABLE IF NOT EXISTS `chat_members` (
  `chat_id` int NOT NULL,
  `user_id` int NOT NULL,
  `joined_at` datetime DEFAULT NULL,
  `added_by` int NOT NULL,
  PRIMARY KEY (`chat_id`,`user_id`),
  KEY `fk_user_id_idx` (`user_id`),
  CONSTRAINT `fk_added_by` FOREIGN KEY (`user_id`) REFERENCES `user` (`id`),
  CONSTRAINT `fk_chat_id` FOREIGN KEY (`chat_id`) REFERENCES `chat` (`id`),
  CONSTRAINT `fk_user_id` FOREIGN KEY (`user_id`) REFERENCES `user` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Дамп данных таблицы dcsm.chat_members: ~0 rows (приблизительно)
/*!40000 ALTER TABLE `chat_members` DISABLE KEYS */;
/*!40000 ALTER TABLE `chat_members` ENABLE KEYS */;

-- Дамп структуры для процедура dcsm.p_add_user_to_chat
DELIMITER //
CREATE PROCEDURE `p_add_user_to_chat`(
	IN new_user_id INT,
    IN added_by INT,
    IN chat_id INT
)
BEGIN
	IF added_by = 2 THEN    
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Can not add user from system account';
	ELSE
		BEGIN
			DECLARE count_rows INT;
			SET count_rows = (
				SELECT COUNT(*)
				FROM `chat_members`
				WHERE `chat_members`.`user_id` = added_by
				AND `chat_members`.`chat_id` = chat_id
			);
			
			IF count_rows = 0 THEN
				SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'added_by is not a member of chat';
			ELSE
				INSERT INTO `chat_members` (`chat_id`, `user_id`, `joined_at`, `added_by`) 
				VALUES (chat_id, new_user_id, NOW(), added_by);
			END IF;
        END;
    END IF;
END//
DELIMITER ;

-- Дамп структуры для процедура dcsm.p_add_user_to_chat_by_name
DELIMITER //
CREATE PROCEDURE `p_add_user_to_chat_by_name`(
	IN p_username VARCHAR(255),
    IN p_added_by INT,
    IN p_chat_id INT
)
BEGIN
	DECLARE new_user_id INT;

	IF p_added_by = 2 THEN    
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Can not add user from system account';
	ELSE
		BEGIN
			DECLARE count_rows INT;
			SET count_rows = (
				SELECT COUNT(*)
				FROM `chat_members`
				WHERE `chat_members`.`user_id` = p_added_by
				AND `chat_members`.`chat_id` = p_chat_id
			);
			
			IF count_rows = 0 THEN
				SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'added_by is not a member of chat';
			ELSE
				SET new_user_id = (SELECT `id` FROM `user` WHERE `username` = p_username);
				
				IF new_user_id is NULL OR new_user_id = 0 THEN
					SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'user not found';
				ELSE
					INSERT INTO `chat_members` (`chat_id`, `user_id`, `joined_at`, `added_by`) 
					VALUES (p_chat_id, new_user_id, NOW(), p_added_by);
				END IF;
			END IF;
        END;
    END IF;
END//
DELIMITER ;

-- Дамп структуры для процедура dcsm.p_change_password
DELIMITER //
CREATE PROCEDURE `p_change_password`(
	IN `p_user_id` INT,
    IN `p_current_password` VARCHAR(256),
    IN `p_new_password` VARCHAR(256)
)
BEGIN
	DECLARE salt_length INT;
    DECLARE salt VARCHAR(64);
    DECLARE salted_hash VARCHAR(256);
    
    CALL p_valid_password_user_id(p_user_id, p_current_password, @current_password_valid);
	
	IF @current_password_valid = 0 THEN
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'invalid password';
	ELSE
		
		SET salt_length = (SELECT FLOOR(RAND() * (64 - 32 + 1) + 32));
		SET salt = (SELECT SUBSTRING(SHA1(RAND()), 1, salt_length));
		SET salted_hash = (SELECT SHA2(CONCAT(salt, p_new_password), 256));
        
        UPDATE `user`
        SET `salt` = salt,
			`password_hash` = salted_hash
        WHERE `id` = p_user_id;
    END IF;
END//
DELIMITER ;

-- Дамп структуры для процедура dcsm.p_create_chat
DELIMITER //
CREATE PROCEDURE `p_create_chat`(
  IN title VARCHAR(255), 
  IN user_id INT
)
BEGIN DECLARE row_id INT;
	INSERT INTO `chat`(`title`) VALUES (title);
	SET row_id = (
		SELECT `id` 
		FROM `chat` 
		ORDER BY `id` DESC 
		LIMIT 1
	  );
      
	INSERT INTO `chat_members` (
	  `chat_id`, `user_id`, `added_by`, `joined_at`
	) 
	VALUES (row_id, user_id, 2, NOW());
END//
DELIMITER ;

-- Дамп структуры для процедура dcsm.p_create_user
DELIMITER //
CREATE PROCEDURE `p_create_user`(
	IN `p_username` VARCHAR(255),
    IN `p_name` VARCHAR(255),
    IN `p_surname` VARCHAR(255),
    IN `p_email` VARCHAR(255),
    IN `p_password` VARCHAR(256)
)
BEGIN
	DECLARE count_rows INT;
	DECLARE salt_length INT;
    DECLARE salt VARCHAR(64);
    DECLARE salted_hash VARCHAR(256);
    DECLARE user_id INT;
    
	SET count_rows = (
		SELECT COUNT(*)
		FROM `user`
		WHERE `user`.`email` = p_email
		OR `user`.`username` = p_username
	);

	IF count_rows > 0 THEN
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'User already registered';
	ELSE
		SET salt_length = (SELECT FLOOR(RAND() * (64 - 32 + 1) + 32));
		SET salt = (SELECT SUBSTRING(SHA1(RAND()), 1, salt_length));
		SET salted_hash = (SELECT SHA2(CONCAT(salt, p_password), 256));
    
		INSERT INTO `user` (`username`, `name`, `surname`, `email`, `salt`, `password_hash`)
			VALUES (p_username, p_name, p_surname, p_email, salt, salted_hash);
            
		SET user_id = (SELECT `id` FROM `user` WHERE `user`.`username` = p_username);
            
		INSERT INTO `user_roles` (`user_id`, `role_id`, `assigned_by`, `date_assigned`)
			VALUES (user_id, 3, 2, NOW());
	END IF;
END//
DELIMITER ;

-- Дамп структуры для процедура dcsm.p_is_user_authorized
DELIMITER //
CREATE PROCEDURE `p_is_user_authorized`(
IN user_id INTEGER,
IN token VARCHAR(260),
OUT authorized TINYINT
)
BEGIN
	SET authorized = (
		SELECT COUNT(*) 
        FROM `session_view`
        WHERE `session_view`.`user_id` = user_id
        AND `session_view`.`token` = token
        AND `available` = 1
    );
END//
DELIMITER ;

-- Дамп структуры для процедура dcsm.p_valid_password_email
DELIMITER //
CREATE PROCEDURE `p_valid_password_email`(
    IN `p_email` VARCHAR(255),
    IN `p_password` VARCHAR(256),
    OUT `p_out_valid` TINYINT
)
BEGIN
	SET p_out_valid = (
		SELECT COUNT(*)
		FROM `user`
		WHERE `email` = p_email
		AND PASSWORD_HASH = SHA2(CONCAT(`salt`, p_password), 256));
END//
DELIMITER ;

-- Дамп структуры для процедура dcsm.p_valid_password_username
DELIMITER //
CREATE PROCEDURE `p_valid_password_username`(
    IN `p_username` VARCHAR(255),
    IN `p_password` VARCHAR(256),
    OUT `p_out_valid` TINYINT
)
BEGIN
	SET p_out_valid = (
		SELECT COUNT(*)
		FROM `user`
		WHERE `username` = p_username
		AND PASSWORD_HASH = SHA2(CONCAT(`salt`, p_password), 256));
END//
DELIMITER ;

-- Дамп структуры для процедура dcsm.p_valid_password_user_id
DELIMITER //
CREATE PROCEDURE `p_valid_password_user_id`(
    IN `p_user_id` INT,
    IN `p_password` VARCHAR(256),
    OUT `p_out_valid` TINYINT
)
BEGIN
	SET p_out_valid = (
		SELECT COUNT(*)
		FROM `user`
		WHERE `id` = p_user_id
		AND PASSWORD_HASH = SHA2(CONCAT(`salt`, p_password), 256));
END//
DELIMITER ;

-- Дамп структуры для таблица dcsm.services
CREATE TABLE IF NOT EXISTS `services` (
  `id` int NOT NULL AUTO_INCREMENT,
  `title` varchar(255) NOT NULL,
  `description` varchar(255) NOT NULL,
  `address` varchar(45) NOT NULL,
  `settings_port_id` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `settings_port_fk_idx` (`settings_port_id`),
  CONSTRAINT `settings_port_fk` FOREIGN KEY (`settings_port_id`) REFERENCES `settings` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Дамп данных таблицы dcsm.services: ~4 rows (приблизительно)
/*!40000 ALTER TABLE `services` DISABLE KEYS */;
INSERT INTO `services` (`id`, `title`, `description`, `address`, `settings_port_id`) VALUES
	(1, 'Registration serivce', 'Сервис регистрации', '192.168.40.76', 4),
	(2, 'Auth service', 'Сервис авторизации', '192.168.40.76', 5),
	(3, 'Chat service', 'Сервис информации о чатах', '192.168.40.76', 2),
	(4, 'Messenger service', 'Мессенджер - сервис', '192.168.40.43', 3);
/*!40000 ALTER TABLE `services` ENABLE KEYS */;

-- Дамп структуры для таблица dcsm.session
CREATE TABLE IF NOT EXISTS `session` (
  `session_id` int NOT NULL AUTO_INCREMENT,
  `user_id` int NOT NULL,
  `device_name` varchar(255) NOT NULL,
  `token` varchar(260) NOT NULL,
  `sign_out` tinyint NOT NULL DEFAULT '0',
  PRIMARY KEY (`session_id`),
  KEY `id_idx` (`user_id`),
  CONSTRAINT `user_id_fk` FOREIGN KEY (`user_id`) REFERENCES `user` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Дамп данных таблицы dcsm.session: ~0 rows (приблизительно)
/*!40000 ALTER TABLE `session` DISABLE KEYS */;
/*!40000 ALTER TABLE `session` ENABLE KEYS */;

-- Дамп структуры для таблица dcsm.settings
CREATE TABLE IF NOT EXISTS `settings` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(128) NOT NULL,
  `value` varchar(512) NOT NULL,
  `description` varchar(512) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Дамп данных таблицы dcsm.settings: ~8 rows (приблизительно)
/*!40000 ALTER TABLE `settings` DISABLE KEYS */;
INSERT INTO `settings` (`id`, `name`, `value`, `description`) VALUES
	(1, 'es_address', 'http://192.168.40.76:9200', 'Elasic search address'),
	(2, 'service_chat_info_port', '23578', 'Port for chat info service'),
	(3, 'service_messenger_port', '23579', 'Port for messenger server'),
	(4, 'service_registration_port', '23580', 'Port for registration service'),
	(5, 'service_authorization_port', '23581', 'Port for authorization service'),
	(8, 'rabbit_mq_address', '192.168.40.76', 'Rabbit MQ address'),
	(9, 'rabbit_mq_user', 'services', 'Rabbit MQ username'),
	(10, 'rabbit_mq_password', 'l9MqLHC6ca', 'Rabbit MQ password');
/*!40000 ALTER TABLE `settings` ENABLE KEYS */;

-- Дамп структуры для таблица dcsm.user
CREATE TABLE IF NOT EXISTS `user` (
  `id` int NOT NULL AUTO_INCREMENT,
  `username` varchar(255) NOT NULL,
  `name` varchar(255) NOT NULL,
  `surname` varchar(255) NOT NULL,
  `email` varchar(255) DEFAULT NULL,
  `password_hash` varchar(256) NOT NULL,
  `image_large` varchar(255) DEFAULT NULL,
  `image_medium` varchar(255) DEFAULT NULL,
  `image_small` varchar(255) DEFAULT NULL,
  `salt` varchar(64) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Дамп данных таблицы dcsm.user: ~0 rows (приблизительно)
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` (`id`, `username`, `name`, `surname`, `email`, `password_hash`, `image_large`, `image_medium`, `image_small`, `salt`) VALUES
	(2, 'SYSTEM', 'SYSTEM', 'SYSTEM', NULL, 'SYSTEM', NULL, NULL, NULL, '');
/*!40000 ALTER TABLE `user` ENABLE KEYS */;

-- Дамп структуры для таблица dcsm.user_role
CREATE TABLE IF NOT EXISTS `user_role` (
  `id` int NOT NULL AUTO_INCREMENT,
  `role` varchar(45) NOT NULL,
  `description` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Дамп данных таблицы dcsm.user_role: ~0 rows (приблизительно)
/*!40000 ALTER TABLE `user_role` DISABLE KEYS */;
INSERT INTO `user_role` (`id`, `role`, `description`) VALUES
	(1, 'SYSTEM', 'Система'),
	(2, 'admin', 'Администратор'),
	(3, 'regular', 'Пользователь');
/*!40000 ALTER TABLE `user_role` ENABLE KEYS */;

-- Дамп структуры для таблица dcsm.user_roles
CREATE TABLE IF NOT EXISTS `user_roles` (
  `user_id` int NOT NULL,
  `role_id` int NOT NULL,
  `assigned_by` int NOT NULL,
  `date_assigned` datetime NOT NULL,
  PRIMARY KEY (`user_id`),
  KEY `assigned_by_idx` (`assigned_by`),
  KEY `role_id_fk_idx` (`role_id`),
  CONSTRAINT `assigned_by_fk` FOREIGN KEY (`assigned_by`) REFERENCES `user` (`id`),
  CONSTRAINT `role_id_fk` FOREIGN KEY (`role_id`) REFERENCES `user_role` (`id`),
  CONSTRAINT `user_id_fk2` FOREIGN KEY (`user_id`) REFERENCES `user` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Дамп данных таблицы dcsm.user_roles: ~0 rows (приблизительно)
/*!40000 ALTER TABLE `user_roles` DISABLE KEYS */;
/*!40000 ALTER TABLE `user_roles` ENABLE KEYS */;

-- Дамп структуры для таблица dcsm.v_chat_members
CREATE TABLE IF NOT EXISTS `v_chat_members` (
  `id` int NOT NULL,
  `joined_at` datetime DEFAULT NULL,
  `chat_id` int NOT NULL,
  `user_id` int NOT NULL,
  `username` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `surname` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `image_small` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `image_medium` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `image_large` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `invited_by_username` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `invited_by_name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `invited_by_surname` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Дамп данных таблицы dcsm.v_chat_members: 0 rows
/*!40000 ALTER TABLE `v_chat_members` DISABLE KEYS */;
/*!40000 ALTER TABLE `v_chat_members` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
