-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               8.0.28 - MySQL Community Server - GPL
-- Server OS:                    Linux
-- HeidiSQL Version:             12.0.0.6468
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Dumping database structure for dcsm
CREATE DATABASE IF NOT EXISTS `dcsm` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `dcsm`;

-- Dumping structure for table dcsm.chat
CREATE TABLE IF NOT EXISTS `chat` (
  `id` int NOT NULL AUTO_INCREMENT,
  `title` varchar(255) NOT NULL,
  `image_medium` varchar(1024) DEFAULT NULL,
  `image_small` varchar(1024) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table dcsm.chat: ~5 rows (approximately)
DELETE FROM `chat`;
INSERT INTO `chat` (`id`, `title`, `image_medium`, `image_small`) VALUES
	(1, 'test_caht', NULL, NULL),
	(2, '01ПГ-м', 'https://sun1-22.userapi.com/s/v1/ig2/5AG-rV4HqP5q8bLGuLvBMlpwjTBQVwSeYCZNLPEZ8gf59WOMz3bgYP0HCe5KhuCzvioRsZYEsxxXC8PhOFSpkEli.jpg?size=200x0&quality=96&crop=0,10,380,380&ava=1', 'https://sun1-22.userapi.com/s/v1/ig2/5AG-rV4HqP5q8bLGuLvBMlpwjTBQVwSeYCZNLPEZ8gf59WOMz3bgYP0HCe5KhuCzvioRsZYEsxxXC8PhOFSpkEli.jpg?size=200x0&quality=96&crop=0,10,380,380&ava=1'),
	(3, 'Соколов Даниил', 'https://mir-s3-cdn-cf.behance.net/project_modules/disp/96be2232163929.567197ac6fb64.png', 'https://mir-s3-cdn-cf.behance.net/project_modules/disp/96be2232163929.567197ac6fb64.png'),
	(4, 'Флуд', 'https://pbs.twimg.com/profile_images/1370055089260392450/T35dhXpe_400x400.jpg', 'https://pbs.twimg.com/profile_images/1370055089260392450/T35dhXpe_400x400.jpg'),
	(5, 'Беляева Мария', 'https://cdn.pixabay.com/photo/2016/11/18/23/38/child-1837375__340.png', 'https://cdn.pixabay.com/photo/2016/11/18/23/38/child-1837375__340.png'),
	(6, 'Чат о важном', 'https://upload.wikimedia.org/wikipedia/commons/f/f7/Nuvola_apps_important.svg', 'https://upload.wikimedia.org/wikipedia/commons/f/f7/Nuvola_apps_important.svg'),
	(7, '16.04.2022', NULL, NULL),
	(8, 'another_chat', NULL, NULL),
	(17, 'Back To The Future (1985)', 'https://davidocchino.com/portfolio/logos/back-to-the-future-part-4-logo-by-david-occhino-design.jpg', NULL),
	(18, 'Blade Runner (1982)', 'https://render.fineartamerica.com/images/images-profile-flow/400/images/artworkimages/mediumlarge/2/rachel-blade-runner-1982-kultur-arts-studios.jpg', NULL),
	(19, 'Matrix (1999)', 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSkX7OldzSMH1u2Si1BNpCtMwyG-jOeAES4ZA&usqp=CAU', NULL);

-- Dumping structure for table dcsm.chat_members
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

-- Dumping data for table dcsm.chat_members: ~9 rows (approximately)
DELETE FROM `chat_members`;
INSERT INTO `chat_members` (`chat_id`, `user_id`, `joined_at`, `added_by`) VALUES
	(1, 7, '2022-02-17 20:49:17', 2),
	(2, 7, '2022-03-25 21:17:55', 8),
	(2, 8, '2022-03-25 21:13:39', 2),
	(3, 8, '2022-03-25 21:14:06', 2),
	(4, 4, '2022-03-25 21:18:12', 8),
	(4, 6, '2022-03-25 21:18:21', 8),
	(4, 7, '2022-03-25 21:18:06', 8),
	(4, 8, '2022-03-25 21:14:16', 2),
	(5, 8, '2022-03-25 21:14:28', 2),
	(6, 8, '2022-03-25 21:14:36', 2),
	(7, 6, '2022-04-16 13:23:16', 9),
	(7, 9, '2022-04-16 13:05:33', 2),
	(8, 9, '2022-04-16 15:07:53', 2),
	(17, 5, '2022-04-16 19:08:46', 2),
	(17, 292, '2022-04-16 19:08:47', 5),
	(17, 293, '2022-04-16 19:08:48', 5),
	(17, 294, '2022-04-16 19:08:48', 5),
	(17, 295, '2022-04-16 19:08:48', 5),
	(17, 296, '2022-04-16 19:08:48', 5),
	(17, 297, '2022-04-16 19:08:48', 5),
	(17, 298, '2022-04-16 19:08:49', 5),
	(17, 299, '2022-04-16 19:08:49', 5),
	(17, 300, '2022-04-16 19:08:49', 5),
	(17, 301, '2022-04-16 19:08:49', 5),
	(17, 302, '2022-04-16 19:08:50', 5),
	(17, 303, '2022-04-16 19:08:50', 5),
	(17, 304, '2022-04-16 19:08:50', 5),
	(17, 305, '2022-04-16 19:08:50', 5),
	(17, 306, '2022-04-16 19:08:51', 5),
	(17, 307, '2022-04-16 19:08:51', 5),
	(17, 308, '2022-04-16 19:08:51', 5),
	(17, 309, '2022-04-16 19:08:51', 5),
	(17, 310, '2022-04-16 19:08:52', 5),
	(17, 311, '2022-04-16 19:08:52', 5),
	(17, 312, '2022-04-16 19:08:52', 5),
	(17, 313, '2022-04-16 19:08:52', 5),
	(17, 314, '2022-04-16 19:08:52', 5),
	(17, 315, '2022-04-16 19:08:53', 5),
	(17, 316, '2022-04-16 19:08:53', 5),
	(17, 317, '2022-04-16 19:08:53', 5),
	(17, 318, '2022-04-16 19:08:53', 5),
	(17, 319, '2022-04-16 19:08:53', 5),
	(17, 320, '2022-04-16 19:08:54', 5),
	(17, 321, '2022-04-16 19:08:54', 5),
	(17, 322, '2022-04-16 19:08:54', 5),
	(17, 323, '2022-04-16 19:08:54', 5),
	(17, 324, '2022-04-16 19:08:55', 5),
	(17, 325, '2022-04-16 19:08:55', 5),
	(17, 326, '2022-04-16 19:08:55', 5),
	(17, 327, '2022-04-16 19:08:55', 5),
	(17, 328, '2022-04-16 19:08:55', 5),
	(17, 329, '2022-04-16 19:08:56', 5),
	(17, 330, '2022-04-16 19:08:56', 5),
	(18, 5, '2022-04-16 19:11:57', 2),
	(18, 331, '2022-04-16 19:11:58', 5),
	(18, 332, '2022-04-16 19:11:58', 5),
	(18, 333, '2022-04-16 19:11:58', 5),
	(18, 334, '2022-04-16 19:11:58', 5),
	(18, 335, '2022-04-16 19:11:58', 5),
	(18, 336, '2022-04-16 19:11:59', 5),
	(18, 337, '2022-04-16 19:11:59', 5),
	(18, 338, '2022-04-16 19:11:59', 5),
	(18, 339, '2022-04-16 19:11:59', 5),
	(18, 340, '2022-04-16 19:12:00', 5),
	(18, 341, '2022-04-16 19:12:00', 5),
	(18, 342, '2022-04-16 19:12:00', 5),
	(18, 343, '2022-04-16 19:12:00', 5),
	(18, 344, '2022-04-16 19:12:00', 5),
	(18, 345, '2022-04-16 19:12:01', 5),
	(18, 346, '2022-04-16 19:12:01', 5),
	(18, 347, '2022-04-16 19:12:01', 5),
	(18, 348, '2022-04-16 19:12:01', 5),
	(18, 349, '2022-04-16 19:12:01', 5),
	(18, 350, '2022-04-16 19:12:02', 5),
	(18, 351, '2022-04-16 19:12:02', 5),
	(18, 352, '2022-04-16 19:12:02', 5),
	(18, 353, '2022-04-16 19:12:02', 5),
	(18, 354, '2022-04-16 19:12:02', 5),
	(18, 355, '2022-04-16 19:12:03', 5),
	(18, 356, '2022-04-16 19:12:03', 5),
	(18, 357, '2022-04-16 19:12:03', 5),
	(18, 358, '2022-04-16 19:12:03', 5),
	(18, 359, '2022-04-16 19:12:04', 5),
	(18, 360, '2022-04-16 19:12:04', 5),
	(18, 361, '2022-04-16 19:12:04', 5),
	(18, 362, '2022-04-16 19:12:04', 5),
	(18, 363, '2022-04-16 19:12:04', 5),
	(19, 5, '2022-04-16 19:14:01', 2),
	(19, 364, '2022-04-16 19:14:02', 5),
	(19, 365, '2022-04-16 19:14:02', 5),
	(19, 366, '2022-04-16 19:14:02', 5),
	(19, 367, '2022-04-16 19:14:02', 5),
	(19, 368, '2022-04-16 19:14:02', 5),
	(19, 369, '2022-04-16 19:14:03', 5),
	(19, 370, '2022-04-16 19:14:03', 5),
	(19, 371, '2022-04-16 19:14:03', 5),
	(19, 372, '2022-04-16 19:14:03', 5),
	(19, 373, '2022-04-16 19:14:03', 5),
	(19, 374, '2022-04-16 19:14:04', 5),
	(19, 375, '2022-04-16 19:14:04', 5),
	(19, 376, '2022-04-16 19:14:04', 5),
	(19, 377, '2022-04-16 19:14:04', 5),
	(19, 378, '2022-04-16 19:14:04', 5),
	(19, 379, '2022-04-16 19:14:05', 5),
	(19, 380, '2022-04-16 19:14:05', 5),
	(19, 381, '2022-04-16 19:14:05', 5),
	(19, 382, '2022-04-16 19:14:05', 5),
	(19, 383, '2022-04-16 19:14:05', 5),
	(19, 384, '2022-04-16 19:14:06', 5),
	(19, 385, '2022-04-16 19:14:06', 5),
	(19, 386, '2022-04-16 19:14:06', 5),
	(19, 387, '2022-04-16 19:14:06', 5),
	(19, 388, '2022-04-16 19:14:06', 5),
	(19, 389, '2022-04-16 19:14:07', 5),
	(19, 390, '2022-04-16 19:14:07', 5),
	(19, 391, '2022-04-16 19:14:07', 5);

-- Dumping structure for procedure dcsm.p_add_user_to_chat
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

-- Dumping structure for procedure dcsm.p_add_user_to_chat_by_name
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

-- Dumping structure for procedure dcsm.p_change_password
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

-- Dumping structure for procedure dcsm.p_create_chat
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

-- Dumping structure for procedure dcsm.p_create_user
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

-- Dumping structure for procedure dcsm.p_is_user_authorized
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

-- Dumping structure for procedure dcsm.p_valid_password_email
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

-- Dumping structure for procedure dcsm.p_valid_password_username
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

-- Dumping structure for procedure dcsm.p_valid_password_user_id
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

-- Dumping structure for table dcsm.services
CREATE TABLE IF NOT EXISTS `services` (
  `id` int NOT NULL AUTO_INCREMENT,
  `title` varchar(255) NOT NULL,
  `description` varchar(255) NOT NULL,
  `address` varchar(45) NOT NULL,
  `settings_port_id` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `settings_port_fk_idx` (`settings_port_id`),
  CONSTRAINT `settings_port_fk` FOREIGN KEY (`settings_port_id`) REFERENCES `settings` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table dcsm.services: ~6 rows (approximately)
DELETE FROM `services`;
INSERT INTO `services` (`id`, `title`, `description`, `address`, `settings_port_id`) VALUES
	(1, 'Registration serivce', 'Сервис регистрации', 'mes-registration-service', 4),
	(2, 'Auth service', 'Сервис авторизации', 'mes-auth-service', 5),
	(3, 'Chat service', 'Сервис информации о чатах', 'mes-chat-info-service', 2),
	(4, 'Messenger service', 'Мессенджер - сервис', 'mes-messenger-service', 3),
	(5, 'User service', 'Сервис информации о пользователях', 'mes-user-service', 11),
	(6, 'Orchestrator', 'Оркестратор', 'mes-orchestrator-service', 12),
	(7, 'Historical message service', 'Сервис исторических сообщений', 'historical-messages-service', 15),
	(8, 'Instant Messages Service', 'Сервис мгновенных сообщений', 'instant-messages-service', 16),
	(9, 'Message Service', 'Сервис сохранения сообщений в ES', 'message-service', 17),
	(10, 'Subcribing service', 'Сервис нотификаций', 'subcribing-service', 18);

-- Dumping structure for table dcsm.session
CREATE TABLE IF NOT EXISTS `session` (
  `session_id` int NOT NULL AUTO_INCREMENT,
  `user_id` int NOT NULL,
  `device_name` varchar(255) NOT NULL,
  `token` varchar(260) NOT NULL,
  `sign_out` tinyint NOT NULL DEFAULT '0',
  PRIMARY KEY (`session_id`),
  KEY `id_idx` (`user_id`),
  CONSTRAINT `user_id_fk` FOREIGN KEY (`user_id`) REFERENCES `user` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=396 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table dcsm.session: ~4 rows (approximately)
DELETE FROM `session`;
INSERT INTO `session` (`session_id`, `user_id`, `device_name`, `token`, `sign_out`) VALUES
	(1, 5, 'insomnia', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI1Iiwicm9sZSI6InJlZ3VsYXIiLCJuYmYiOjE2MjY2MjI0NTAsImV4cCI6MTYyOTIxNDQ1MCwiaWF0IjoxNjI2NjIyNDUwLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.j5zWtHm2NsmS7MccLkbwpKwR3OLtlWOmgrL_B7WbNjQ', 0),
	(2, 5, 'insomnia', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI1Iiwicm9sZSI6ImFkbWluIiwibmJmIjoxNjI2NjIyNDk1LCJleHAiOjE2MjkyMTQ0OTUsImlhdCI6MTYyNjYyMjQ5NSwiaXNzIjoiQXV0aFNlcnZlciIsImF1ZCI6IldlYkNsaWVudCJ9.9g4zjLuk40dBDpf9PsuxvDdWDRspESlNmXmkXyF06Yc', 0),
	(3, 6, 'insomnia', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI2Iiwicm9sZSI6InJlZ3VsYXIiLCJuYmYiOjE2MjY2MjI3MjYsImV4cCI6MTYyOTIxNDcyNiwiaWF0IjoxNjI2NjIyNzI2LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.I8UpWOAFW2k0yQi5139RRC-F9HUU-rR59jPuhI9T5KA', 0),
	(4, 7, 'chrome 98.0.4758 Windows 10', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI3Iiwicm9sZSI6InJlZ3VsYXIiLCJuYmYiOjE2NDUxMjAxNDgsImV4cCI6MTY0NzcxMjE0OCwiaWF0IjoxNjQ1MTIwMTQ4LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.iK-HPeWwLHTqVwCcZyuOyn5WnczlYUz1BtxBdlohi70', 0),
	(5, 8, 'chrome 99.0.4844 Windows 10', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiI4IiwibmJmIjoxNjQ4MjMzNjY4LCJleHAiOjE2NTA4MjU2NjgsImlhdCI6MTY0ODIzMzY2OCwiaXNzIjoiQXV0aFNlcnZlciIsImF1ZCI6IldlYkNsaWVudCJ9.k-fopsaj9-R2meVnc2h4YZuLTvqH5BMwJmjY1UX7Ddk', 0),
	(6, 9, 'chrome 100.0.4896 Windows 10', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiI5IiwibmJmIjoxNjUwMTAyNDE3LCJleHAiOjE2NTI2OTQ0MTcsImlhdCI6MTY1MDEwMjQxNywiaXNzIjoiQXV0aFNlcnZlciIsImF1ZCI6IldlYkNsaWVudCJ9.N8Re8THmtoi1t06SiVUYaJUT1FbaNQPopdX76zYStu4', 0),
	(7, 9, 'chrome 100.0.4896 Windows 10', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiI5IiwibmJmIjoxNjUwMTAzMjU2LCJleHAiOjE2NTI2OTUyNTYsImlhdCI6MTY1MDEwMzI1NiwiaXNzIjoiQXV0aFNlcnZlciIsImF1ZCI6IldlYkNsaWVudCJ9.n5ojsfdL2MzOt158I_3meDm57rq2Fw0rZkVu4KOVlpk', 0),
	(8, 9, 'chrome 100.0.4896 Windows 10', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiI5IiwibmJmIjoxNjUwMTAzOTY4LCJleHAiOjE2NTI2OTU5NjgsImlhdCI6MTY1MDEwMzk2OCwiaXNzIjoiQXV0aFNlcnZlciIsImF1ZCI6IldlYkNsaWVudCJ9.LoTYg_1TRnvv3_JlnG72PybJ7Z1W4YaGrlbpq_ItcwM', 0),
	(9, 9, 'chrome 100.0.4896 Windows 10', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiI5IiwibmJmIjoxNjUwMTEyMDg2LCJleHAiOjE2NTI3MDQwODYsImlhdCI6MTY1MDExMjA4NiwiaXNzIjoiQXV0aFNlcnZlciIsImF1ZCI6IldlYkNsaWVudCJ9.4fmP3I1hUYiVcCAsndh56evVmnzrpSrlGa_bAYLhUEA', 0),
	(10, 6, 'insomnia', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiI2IiwibmJmIjoxNjUwMTA0MTI2LCJleHAiOjE2NTI2OTYxMjYsImlhdCI6MTY1MDEwNDEyNiwiaXNzIjoiQXV0aFNlcnZlciIsImF1ZCI6IldlYkNsaWVudCJ9.YCcOdMFUaIKbFni5ySOdEQ9I_Bd7IX7Tgrz445g0y88', 0),
	(11, 5, 'chrome 100.0.4896 Windows 10', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoiYWRtaW4iLCJ1c2VyX2lkIjoiNSIsIm5iZiI6MTY1MDExMzc0MSwiZXhwIjoxNjUyNzA1NzQxLCJpYXQiOjE2NTAxMTM3NDEsImlzcyI6IkF1dGhTZXJ2ZXIiLCJhdWQiOiJXZWJDbGllbnQifQ.tL2uw0-PStqQyHYkp4zLXOhfRRcUl6pEG1Oy7jbtk9Y', 0),
	(12, 5, 'chrome 100.0.4896 Windows 10', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoiYWRtaW4iLCJ1c2VyX2lkIjoiNSIsIm5iZiI6MTY1MDExNDUzMCwiZXhwIjoxNjUyNzA2NTMwLCJpYXQiOjE2NTAxMTQ1MzAsImlzcyI6IkF1dGhTZXJ2ZXIiLCJhdWQiOiJXZWJDbGllbnQifQ.CVy-3FSl9sI3GLI9ojQ5e2nQNe1rsjg5qvv02_i5o7Y', 0),
	(13, 5, 'chrome 100.0.4896 Windows 10', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoiYWRtaW4iLCJ1c2VyX2lkIjoiNSIsIm5iZiI6MTY1MDExNTM3NiwiZXhwIjoxNjUyNzA3Mzc2LCJpYXQiOjE2NTAxMTUzNzYsImlzcyI6IkF1dGhTZXJ2ZXIiLCJhdWQiOiJXZWJDbGllbnQifQ.YuGc8rnNFgnTYtC_-n1WSW4XbGNL1E-aFzw_o5XlYu4', 0),
	(14, 10, 'chrome 100.0.4896 Windows 10', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIxMCIsIm5iZiI6MTY1MDEyMTk3NiwiZXhwIjoxNjUyNzEzOTc2LCJpYXQiOjE2NTAxMjE5NzYsImlzcyI6IkF1dGhTZXJ2ZXIiLCJhdWQiOiJXZWJDbGllbnQifQ.2rUvVcrKdS8USSF53sJaS2LcriLejHQGofLwNaGo-iw', 0),
	(15, 5, 'postman', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoiYWRtaW4iLCJ1c2VyX2lkIjoiNSIsIm5iZiI6MTY1MDEyMjE3NiwiZXhwIjoxNjUyNzE0MTc2LCJpYXQiOjE2NTAxMjIxNzYsImlzcyI6IkF1dGhTZXJ2ZXIiLCJhdWQiOiJXZWJDbGllbnQifQ.BThXLbtG7EvoOQjLU3sB-nyMIXNGDG8y4ajIMZ8F_iM', 0),
	(295, 292, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIyOTIiLCJuYmYiOjE2NTAxMjUzMjcsImV4cCI6MTY1MjcxNzMyNywiaWF0IjoxNjUwMTI1MzI3LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.ikniPggqoco58pQg2n_TfLiWirPR5LRqGCJ72GWNrn0', 0),
	(296, 293, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIyOTMiLCJuYmYiOjE2NTAxMjUzMjcsImV4cCI6MTY1MjcxNzMyNywiaWF0IjoxNjUwMTI1MzI3LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.rBRPHCbPTUPM8rdlRqe3TpJyj6HEZJWfS1uXrz6k6M4', 0),
	(297, 294, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIyOTQiLCJuYmYiOjE2NTAxMjUzMjgsImV4cCI6MTY1MjcxNzMyOCwiaWF0IjoxNjUwMTI1MzI4LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.PlZtC_PfwdMsUs5NDJ6abZCZEEBfoOyKTyzKLGhONhQ', 0),
	(298, 295, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIyOTUiLCJuYmYiOjE2NTAxMjUzMjgsImV4cCI6MTY1MjcxNzMyOCwiaWF0IjoxNjUwMTI1MzI4LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.WhBgtEFZ1sU4L0M8zR-MpPPvw8gxBXwP1ITmR7PF6j4', 0),
	(299, 296, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIyOTYiLCJuYmYiOjE2NTAxMjUzMjgsImV4cCI6MTY1MjcxNzMyOCwiaWF0IjoxNjUwMTI1MzI4LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.X3Jr7keKhbgAqvZM5zN2BRitt4-pLavCpaBsDH5j9Yk', 0),
	(300, 297, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIyOTciLCJuYmYiOjE2NTAxMjUzMjgsImV4cCI6MTY1MjcxNzMyOCwiaWF0IjoxNjUwMTI1MzI4LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.MtgdVZd_1Z6DjOz49YKX5rYIn934daCsVg9_e-dqiBU', 0),
	(301, 298, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIyOTgiLCJuYmYiOjE2NTAxMjUzMjksImV4cCI6MTY1MjcxNzMyOSwiaWF0IjoxNjUwMTI1MzI5LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.8SsoZtoHcCuJfmJyp9brlRZ1xSh4OgwPNOyTZ-sT83g', 0),
	(302, 299, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIyOTkiLCJuYmYiOjE2NTAxMjUzMjksImV4cCI6MTY1MjcxNzMyOSwiaWF0IjoxNjUwMTI1MzI5LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.XMPfAqXCIpcQPEJ8Lt_EiHqlbTbH8gDTxL1rw1b3Vu8', 0),
	(303, 300, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMDAiLCJuYmYiOjE2NTAxMjUzMjksImV4cCI6MTY1MjcxNzMyOSwiaWF0IjoxNjUwMTI1MzI5LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.P8EUNYKMr_IU9UnHRkt7tL5sPbO2wgFmYgPaJW4_G2w', 0),
	(304, 301, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMDEiLCJuYmYiOjE2NTAxMjUzMjksImV4cCI6MTY1MjcxNzMyOSwiaWF0IjoxNjUwMTI1MzI5LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.IbGPvjZE_tb0W8I-MT_v9wrk5_IHk-Mr5_zAMMek2k0', 0),
	(305, 302, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMDIiLCJuYmYiOjE2NTAxMjUzMzAsImV4cCI6MTY1MjcxNzMzMCwiaWF0IjoxNjUwMTI1MzMwLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.41lEEARR7qRtwenCkqOkQ37cmArRLzr6e7Ge9FnZkSs', 0),
	(306, 303, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMDMiLCJuYmYiOjE2NTAxMjUzMzAsImV4cCI6MTY1MjcxNzMzMCwiaWF0IjoxNjUwMTI1MzMwLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.CIkdzllIFPXyh1AT0aVzDeA6t_5U4cWwrSPUk_CyBpY', 0),
	(307, 304, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMDQiLCJuYmYiOjE2NTAxMjUzMzAsImV4cCI6MTY1MjcxNzMzMCwiaWF0IjoxNjUwMTI1MzMwLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.61t4QdDMYn8eCwTvpyhscdf29PD001a9pD8K2mJ-3pQ', 0),
	(308, 305, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMDUiLCJuYmYiOjE2NTAxMjUzMzAsImV4cCI6MTY1MjcxNzMzMCwiaWF0IjoxNjUwMTI1MzMwLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.VCHvE7LXg8PROG3gVjFLH7vOn1J8hQutkmVWF3dhrCk', 0),
	(309, 306, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMDYiLCJuYmYiOjE2NTAxMjUzMzEsImV4cCI6MTY1MjcxNzMzMSwiaWF0IjoxNjUwMTI1MzMxLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.K2aryeHYobuwCGUVeBDIZHTLg2y9fSxJ95j32kJbQHA', 0),
	(310, 307, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMDciLCJuYmYiOjE2NTAxMjUzMzEsImV4cCI6MTY1MjcxNzMzMSwiaWF0IjoxNjUwMTI1MzMxLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.Frgl_2W5KF8UcX-WNN_AMGXg8CAjKMT6b6oIuWJkcrw', 0),
	(311, 308, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMDgiLCJuYmYiOjE2NTAxMjUzMzEsImV4cCI6MTY1MjcxNzMzMSwiaWF0IjoxNjUwMTI1MzMxLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.b1tpZxgrwG5cpFJpo5822DS4IVE8lqIz2258AiZEMpw', 0),
	(312, 309, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMDkiLCJuYmYiOjE2NTAxMjUzMzEsImV4cCI6MTY1MjcxNzMzMSwiaWF0IjoxNjUwMTI1MzMxLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.kERPmZY0EjDdWHlUFAYOvz40tG53HQWw0yvfK9nEnM8', 0),
	(313, 310, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMTAiLCJuYmYiOjE2NTAxMjUzMzIsImV4cCI6MTY1MjcxNzMzMiwiaWF0IjoxNjUwMTI1MzMyLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.ADTntYqzNFmLPLn4zkXCllsNPWJnWenZR3xN9dmBvt4', 0),
	(314, 311, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMTEiLCJuYmYiOjE2NTAxMjUzMzIsImV4cCI6MTY1MjcxNzMzMiwiaWF0IjoxNjUwMTI1MzMyLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.1w1jrE4X1UugKhnCiEWGb9hHI3QMxxMP-bYUdVUZaAQ', 0),
	(315, 312, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMTIiLCJuYmYiOjE2NTAxMjUzMzIsImV4cCI6MTY1MjcxNzMzMiwiaWF0IjoxNjUwMTI1MzMyLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.sYjUSFXhpwQDEUS0n67mu7Y9zpbd4VuGkJYYIiggu_E', 0),
	(316, 313, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMTMiLCJuYmYiOjE2NTAxMjUzMzIsImV4cCI6MTY1MjcxNzMzMiwiaWF0IjoxNjUwMTI1MzMyLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.UYe-xujYfHlV_LAtm2jZmwQydUtbOZnKKJ8_4PsHe9o', 0),
	(317, 314, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMTQiLCJuYmYiOjE2NTAxMjUzMzIsImV4cCI6MTY1MjcxNzMzMiwiaWF0IjoxNjUwMTI1MzMyLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0._XCPKcPT_8b-ivZiCl9F7EGstg4u7SzaR-_iQjIKwSI', 0),
	(318, 315, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMTUiLCJuYmYiOjE2NTAxMjUzMzMsImV4cCI6MTY1MjcxNzMzMywiaWF0IjoxNjUwMTI1MzMzLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.AKt-2pNc-EDydPhFlhQ6ubWcjW-tV4qbLBUVDkMKUKk', 0),
	(319, 316, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMTYiLCJuYmYiOjE2NTAxMjUzMzMsImV4cCI6MTY1MjcxNzMzMywiaWF0IjoxNjUwMTI1MzMzLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.sTQmA74fzLQrYkjYpxrQX5jZx9HvLeIg7s3ZdEkb9rU', 0),
	(320, 317, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMTciLCJuYmYiOjE2NTAxMjUzMzMsImV4cCI6MTY1MjcxNzMzMywiaWF0IjoxNjUwMTI1MzMzLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.7GLsPcXOz6bVltVQgSqTwBIE_xqbMEwsb1QyUYSLP0I', 0),
	(321, 318, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMTgiLCJuYmYiOjE2NTAxMjUzMzMsImV4cCI6MTY1MjcxNzMzMywiaWF0IjoxNjUwMTI1MzMzLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.pZtRCSfilfafMiDMjVnjQNjtMazI5uGrmj-pIfwjt08', 0),
	(322, 319, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMTkiLCJuYmYiOjE2NTAxMjUzMzMsImV4cCI6MTY1MjcxNzMzMywiaWF0IjoxNjUwMTI1MzMzLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.AyOLhrm2_LuGlECXv7IzZ5NPmVy66C0-kJWNEOf9Wnk', 0),
	(323, 320, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMjAiLCJuYmYiOjE2NTAxMjUzMzQsImV4cCI6MTY1MjcxNzMzNCwiaWF0IjoxNjUwMTI1MzM0LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.1z-TrJ96ldT8N7Wp89tN7F0SbmcfJH1B6EOt_OtP7Zs', 0),
	(324, 321, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMjEiLCJuYmYiOjE2NTAxMjUzMzQsImV4cCI6MTY1MjcxNzMzNCwiaWF0IjoxNjUwMTI1MzM0LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.pKH4eD_moFKncvLaA-JoDqs3p1ePYPMCrvWQw5mcr_o', 0),
	(325, 322, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMjIiLCJuYmYiOjE2NTAxMjUzMzQsImV4cCI6MTY1MjcxNzMzNCwiaWF0IjoxNjUwMTI1MzM0LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.VcoGlC9aR6wJtplAW1VGuFmwrGnj-IdCtS0Gp8ZLi_A', 0),
	(326, 323, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMjMiLCJuYmYiOjE2NTAxMjUzMzQsImV4cCI6MTY1MjcxNzMzNCwiaWF0IjoxNjUwMTI1MzM0LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.7zZeXJgum50UOYVQISg9GUuWhiDIEQ9ITc4Vz8mwPW0', 0),
	(327, 324, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMjQiLCJuYmYiOjE2NTAxMjUzMzUsImV4cCI6MTY1MjcxNzMzNSwiaWF0IjoxNjUwMTI1MzM1LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0._wLcPQf33-ScqWg-IUUfarhG2ImkXuMv0GWvApI31Rg', 0),
	(328, 325, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMjUiLCJuYmYiOjE2NTAxMjUzMzUsImV4cCI6MTY1MjcxNzMzNSwiaWF0IjoxNjUwMTI1MzM1LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.ysmGKQtj64Y51O1vqiqeBuDipLr6W_GoPUnz8iT4Y98', 0),
	(329, 326, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMjYiLCJuYmYiOjE2NTAxMjUzMzUsImV4cCI6MTY1MjcxNzMzNSwiaWF0IjoxNjUwMTI1MzM1LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.GSeYi_ya83a8GkzgNvL0PemLvz7njE32TwfH8_Bzcw4', 0),
	(330, 327, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMjciLCJuYmYiOjE2NTAxMjUzMzUsImV4cCI6MTY1MjcxNzMzNSwiaWF0IjoxNjUwMTI1MzM1LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.rFIRIBkiOBjgSPc82WPusk-QY5yIP8yOM6ZGyflesKA', 0),
	(331, 328, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMjgiLCJuYmYiOjE2NTAxMjUzMzUsImV4cCI6MTY1MjcxNzMzNSwiaWF0IjoxNjUwMTI1MzM1LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.75ZOhfDJK3b1l6cT_fKRBgBTzF5NkaJmbyHcChuMdyI', 0),
	(332, 329, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMjkiLCJuYmYiOjE2NTAxMjUzMzYsImV4cCI6MTY1MjcxNzMzNiwiaWF0IjoxNjUwMTI1MzM2LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.ybMmck6YaK4kTwR66D9Wl6ZB4L6fZRZ1S6_LKYTYw4s', 0),
	(333, 330, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMzAiLCJuYmYiOjE2NTAxMjUzMzYsImV4cCI6MTY1MjcxNzMzNiwiaWF0IjoxNjUwMTI1MzM2LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.rfcT7268XH5sUkdlZdm_mzzbmjZnpDDGi181M_4zyxg', 0),
	(334, 331, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMzEiLCJuYmYiOjE2NTAxMjU1MTgsImV4cCI6MTY1MjcxNzUxOCwiaWF0IjoxNjUwMTI1NTE4LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.dUfOAs8GiHWJn5tE-Umo7DC1Sf8i7CTXzuiS8OXTVOY', 0),
	(335, 332, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMzIiLCJuYmYiOjE2NTAxMjU1MTgsImV4cCI6MTY1MjcxNzUxOCwiaWF0IjoxNjUwMTI1NTE4LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.xLaP7AD2AVgwxoiR-OA_x_RWqBwFGvxR6cZuNH0KFvU', 0),
	(336, 333, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMzMiLCJuYmYiOjE2NTAxMjU1MTgsImV4cCI6MTY1MjcxNzUxOCwiaWF0IjoxNjUwMTI1NTE4LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.3cO1FYEZLPkUAxdvfA955_2TsN79E2exrm6WDAHFNRU', 0),
	(337, 334, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMzQiLCJuYmYiOjE2NTAxMjU1MTgsImV4cCI6MTY1MjcxNzUxOCwiaWF0IjoxNjUwMTI1NTE4LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.aoqOdi3suTSytwcIMV-hX9BnIc4KLIoTngrEAtF6zdI', 0),
	(338, 335, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMzUiLCJuYmYiOjE2NTAxMjU1MTgsImV4cCI6MTY1MjcxNzUxOCwiaWF0IjoxNjUwMTI1NTE4LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.qjxBjoCEURs5CK4E1V53lftUWxk3FCh-tpoXCARpM2I', 0),
	(339, 336, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMzYiLCJuYmYiOjE2NTAxMjU1MTksImV4cCI6MTY1MjcxNzUxOSwiaWF0IjoxNjUwMTI1NTE5LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.uFRiYRtr_P6Yt5WH68AJsSVZqml9T0Pjh80eVDTk6hc', 0),
	(340, 337, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMzciLCJuYmYiOjE2NTAxMjU1MTksImV4cCI6MTY1MjcxNzUxOSwiaWF0IjoxNjUwMTI1NTE5LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0._K_KfyFj_9hdJbbNBViLIYsEjw7vlHlyU1EwsvfpzQg', 0),
	(341, 338, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMzgiLCJuYmYiOjE2NTAxMjU1MTksImV4cCI6MTY1MjcxNzUxOSwiaWF0IjoxNjUwMTI1NTE5LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.Gn3HfGpSMwUR_wpIMS4-A4tpPK1j9nZQTJHH5RysEo8', 0),
	(342, 339, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzMzkiLCJuYmYiOjE2NTAxMjU1MTksImV4cCI6MTY1MjcxNzUxOSwiaWF0IjoxNjUwMTI1NTE5LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.ksrQXCJLkZE3qFmqSADE8E3E2QcCTxAEuLfKQL8pV7w', 0),
	(343, 340, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNDAiLCJuYmYiOjE2NTAxMjU1MjAsImV4cCI6MTY1MjcxNzUyMCwiaWF0IjoxNjUwMTI1NTIwLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.2DVrvB_xfkwHgIj1YOZEa1IMX6OchTYS9aZ0DWbqnUk', 0),
	(344, 341, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNDEiLCJuYmYiOjE2NTAxMjU1MjAsImV4cCI6MTY1MjcxNzUyMCwiaWF0IjoxNjUwMTI1NTIwLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.IWccKxsORd6j4ktTMEBM7yUT9k0ZyhzhrYysjbbfYWg', 0),
	(345, 342, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNDIiLCJuYmYiOjE2NTAxMjU1MjAsImV4cCI6MTY1MjcxNzUyMCwiaWF0IjoxNjUwMTI1NTIwLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.yXig4w9lBeufBwqKR3WHDKKkvr2Ofb0in_3mb-ZUYMI', 0),
	(346, 343, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNDMiLCJuYmYiOjE2NTAxMjU1MjAsImV4cCI6MTY1MjcxNzUyMCwiaWF0IjoxNjUwMTI1NTIwLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.VMS34JWjvnyhOPCS4hbmioBduro__gH87IsSOUFDgVc', 0),
	(347, 344, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNDQiLCJuYmYiOjE2NTAxMjU1MjAsImV4cCI6MTY1MjcxNzUyMCwiaWF0IjoxNjUwMTI1NTIwLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.ybt4tNo3Awr14UL4CODd70wCBDccFBBl1u56odzuguc', 0),
	(348, 345, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNDUiLCJuYmYiOjE2NTAxMjU1MjEsImV4cCI6MTY1MjcxNzUyMSwiaWF0IjoxNjUwMTI1NTIxLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.2DKPMg-iUF_9r3NlMgU_aZdAHvJ_C9uhP_SfXPj1J9g', 0),
	(349, 346, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNDYiLCJuYmYiOjE2NTAxMjU1MjEsImV4cCI6MTY1MjcxNzUyMSwiaWF0IjoxNjUwMTI1NTIxLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.-1q0UA0wfJFZM3o5Rg0gXNa9FIOhJ0p8Oo_CFPnaIDw', 0),
	(350, 347, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNDciLCJuYmYiOjE2NTAxMjU1MjEsImV4cCI6MTY1MjcxNzUyMSwiaWF0IjoxNjUwMTI1NTIxLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.y1B7IoU-RmlQ_6cqV0NhITBwSnrIxPy118lUZ9RvLcA', 0),
	(351, 348, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNDgiLCJuYmYiOjE2NTAxMjU1MjEsImV4cCI6MTY1MjcxNzUyMSwiaWF0IjoxNjUwMTI1NTIxLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.woTv_sBzFy90AsDBc9ii6f1yfb7CM2k4O_Q1gsIF7gA', 0),
	(352, 349, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNDkiLCJuYmYiOjE2NTAxMjU1MjEsImV4cCI6MTY1MjcxNzUyMSwiaWF0IjoxNjUwMTI1NTIxLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.aWxYDp1E8poPK_WoTyw0gzRzVorxj7wyHw6rVq4AJXU', 0),
	(353, 350, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNTAiLCJuYmYiOjE2NTAxMjU1MjIsImV4cCI6MTY1MjcxNzUyMiwiaWF0IjoxNjUwMTI1NTIyLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.psAn45HoN6hl29b3ZaMcnOlDNXKyPIJl5Z8MLKH55V0', 0),
	(354, 351, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNTEiLCJuYmYiOjE2NTAxMjU1MjIsImV4cCI6MTY1MjcxNzUyMiwiaWF0IjoxNjUwMTI1NTIyLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.0R702aGt0ldoeckaer-rxsXmVzG1Bop14__bqcN9Lac', 0),
	(355, 352, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNTIiLCJuYmYiOjE2NTAxMjU1MjIsImV4cCI6MTY1MjcxNzUyMiwiaWF0IjoxNjUwMTI1NTIyLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.sFiDn3kAKFXxFQES8Ffc1i9-a2FYULJmbYA6nR0yqak', 0),
	(356, 353, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNTMiLCJuYmYiOjE2NTAxMjU1MjIsImV4cCI6MTY1MjcxNzUyMiwiaWF0IjoxNjUwMTI1NTIyLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.6EpYgVTrGN3S8BRxRh_SyVTr4xaqXAVbPQ8PuWOKznU', 0),
	(357, 354, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNTQiLCJuYmYiOjE2NTAxMjU1MjIsImV4cCI6MTY1MjcxNzUyMiwiaWF0IjoxNjUwMTI1NTIyLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.7rBmU9mS095_qtkzXv17Lu2Ak4A1bAJ6rGAKlqhh5Is', 0),
	(358, 355, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNTUiLCJuYmYiOjE2NTAxMjU1MjMsImV4cCI6MTY1MjcxNzUyMywiaWF0IjoxNjUwMTI1NTIzLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.qUwu4D0lQfXoC9-jM3O2Hdwww4JJsKIXU2cjsit7KvY', 0),
	(359, 356, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNTYiLCJuYmYiOjE2NTAxMjU1MjMsImV4cCI6MTY1MjcxNzUyMywiaWF0IjoxNjUwMTI1NTIzLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.-y_AQpa21JLmrU4tmn6rrIsepYI8sBwglRHkgKkcmfo', 0),
	(360, 357, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNTciLCJuYmYiOjE2NTAxMjU1MjMsImV4cCI6MTY1MjcxNzUyMywiaWF0IjoxNjUwMTI1NTIzLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.b1gGOTU0EPypKnPTQ-YW1BB6wHvKU_FlwOjYy-nC9DA', 0),
	(361, 358, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNTgiLCJuYmYiOjE2NTAxMjU1MjMsImV4cCI6MTY1MjcxNzUyMywiaWF0IjoxNjUwMTI1NTIzLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.LkDUfRkbPDDcFqWFDtvFL1RfyE4dhWHdSgtX-Znbw8U', 0),
	(362, 359, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNTkiLCJuYmYiOjE2NTAxMjU1MjQsImV4cCI6MTY1MjcxNzUyNCwiaWF0IjoxNjUwMTI1NTI0LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.1qwb6mEAZ6d3wIZtp50cMxFRZfisXqzjQSC3lYM3uhc', 0),
	(363, 360, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNjAiLCJuYmYiOjE2NTAxMjU1MjQsImV4cCI6MTY1MjcxNzUyNCwiaWF0IjoxNjUwMTI1NTI0LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.wNNyBIPfGsWPKCFDtDnrPETkK-ytkQe5vGgiCSGjBGg', 0),
	(364, 361, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNjEiLCJuYmYiOjE2NTAxMjU1MjQsImV4cCI6MTY1MjcxNzUyNCwiaWF0IjoxNjUwMTI1NTI0LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.a1HggVpGxU3WjApf3VnREmuQ8Zt5qNCGwoG-WcoD2a8', 0),
	(365, 362, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNjIiLCJuYmYiOjE2NTAxMjU1MjQsImV4cCI6MTY1MjcxNzUyNCwiaWF0IjoxNjUwMTI1NTI0LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.IIlgB_-mCnnp1fBuQVvGqDj4bo1d5Rhnl-POnNsx8_8', 0),
	(366, 363, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNjMiLCJuYmYiOjE2NTAxMjU1MjQsImV4cCI6MTY1MjcxNzUyNCwiaWF0IjoxNjUwMTI1NTI0LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.f6LVkYCGLhmrrQ4UCqrHKXHKCky-hyPWAkjAVJNUVJc', 0),
	(367, 364, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNjQiLCJuYmYiOjE2NTAxMjU2NDIsImV4cCI6MTY1MjcxNzY0MiwiaWF0IjoxNjUwMTI1NjQyLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.7R5WqSvmzcfNKipAKVqVNI4NfzK2FwFn9VtZiFVkqqQ', 0),
	(368, 365, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNjUiLCJuYmYiOjE2NTAxMjU2NDIsImV4cCI6MTY1MjcxNzY0MiwiaWF0IjoxNjUwMTI1NjQyLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.ZqV3iNmzAzkA7NvQ6bXfeedVah48gS-TenfM-aa8YjM', 0),
	(369, 366, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNjYiLCJuYmYiOjE2NTAxMjU2NDIsImV4cCI6MTY1MjcxNzY0MiwiaWF0IjoxNjUwMTI1NjQyLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.UuIfzPUNRufb8azGmijFIhlPHGYPMgkN59q0p-ZB1G0', 0),
	(370, 367, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNjciLCJuYmYiOjE2NTAxMjU2NDIsImV4cCI6MTY1MjcxNzY0MiwiaWF0IjoxNjUwMTI1NjQyLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.7kh2e2unAlvI77qDh-A9_zlbsnNhqDUSJBLNTZL0kK8', 0),
	(371, 368, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNjgiLCJuYmYiOjE2NTAxMjU2NDIsImV4cCI6MTY1MjcxNzY0MiwiaWF0IjoxNjUwMTI1NjQyLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.5lPQtL2kuhtY1bkV0IGwMWE4ec_ER20fM2dHRx88-b4', 0),
	(372, 369, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNjkiLCJuYmYiOjE2NTAxMjU2NDMsImV4cCI6MTY1MjcxNzY0MywiaWF0IjoxNjUwMTI1NjQzLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.kKJe3rDKGGRW2KS5jJjLcm28BPTyGSK79YcXQVzgAY0', 0),
	(373, 370, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNzAiLCJuYmYiOjE2NTAxMjU2NDMsImV4cCI6MTY1MjcxNzY0MywiaWF0IjoxNjUwMTI1NjQzLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.Modb_-oQNJ8d9Rpv_zSNyd_uDlVxxKi8gEGHddi5fBk', 0),
	(374, 371, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNzEiLCJuYmYiOjE2NTAxMjU2NDMsImV4cCI6MTY1MjcxNzY0MywiaWF0IjoxNjUwMTI1NjQzLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.JYN4bY4xbWHXA2rvcg9NajwCZOVRv-bUyhVyDuEa_YA', 0),
	(375, 372, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNzIiLCJuYmYiOjE2NTAxMjU2NDMsImV4cCI6MTY1MjcxNzY0MywiaWF0IjoxNjUwMTI1NjQzLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.KFDmGJSLqrg5jzh69qpp7FWPIcrZ0BGCGo0Xry5pHd0', 0),
	(376, 373, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNzMiLCJuYmYiOjE2NTAxMjU2NDMsImV4cCI6MTY1MjcxNzY0MywiaWF0IjoxNjUwMTI1NjQzLCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.t3opxan_hQR5RtVSjkU_4ivYr2BwFWYB8BSQh7vRtpU', 0),
	(377, 374, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNzQiLCJuYmYiOjE2NTAxMjU2NDQsImV4cCI6MTY1MjcxNzY0NCwiaWF0IjoxNjUwMTI1NjQ0LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.qywEMwfzUiu2V0ahAi8MbercyOooVo0c6dWWhCN4biE', 0),
	(378, 375, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNzUiLCJuYmYiOjE2NTAxMjU2NDQsImV4cCI6MTY1MjcxNzY0NCwiaWF0IjoxNjUwMTI1NjQ0LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.dJrVYryeYAa50LfTQI2xNLvsPoHAnlkA1rJ_8BOX3uU', 0),
	(379, 376, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNzYiLCJuYmYiOjE2NTAxMjU2NDQsImV4cCI6MTY1MjcxNzY0NCwiaWF0IjoxNjUwMTI1NjQ0LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.Fbw5g9wqWwuYHH6XExM-mfBxKwRS62KZtGc8tmeCuvE', 0),
	(380, 377, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNzciLCJuYmYiOjE2NTAxMjU2NDQsImV4cCI6MTY1MjcxNzY0NCwiaWF0IjoxNjUwMTI1NjQ0LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.wSBKPCaatHMpNZbkCLwY5Cu54T47o0fJ1GTd80RTJp4', 0),
	(381, 378, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNzgiLCJuYmYiOjE2NTAxMjU2NDQsImV4cCI6MTY1MjcxNzY0NCwiaWF0IjoxNjUwMTI1NjQ0LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.98rn5v8z9WCl67u1ZdmYXqY8L8N2PaMIhv_D3WPBDSA', 0),
	(382, 379, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzNzkiLCJuYmYiOjE2NTAxMjU2NDUsImV4cCI6MTY1MjcxNzY0NSwiaWF0IjoxNjUwMTI1NjQ1LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.NCxJcQoEiGSD_ZosoUUJBqMKSqsvtT2p3JO5UsIPAPE', 0),
	(383, 380, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzODAiLCJuYmYiOjE2NTAxMjU2NDUsImV4cCI6MTY1MjcxNzY0NSwiaWF0IjoxNjUwMTI1NjQ1LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.N8pOOYZUfgiBsJGkBnQ6Bp8lKdaHAcjrEhTLTNHV2fs', 0),
	(384, 381, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzODEiLCJuYmYiOjE2NTAxMjU2NDUsImV4cCI6MTY1MjcxNzY0NSwiaWF0IjoxNjUwMTI1NjQ1LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.-zTKCGlSaxun2vHV4U14rJx4mU8WH4TuofvX1DGMHWg', 0),
	(385, 382, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzODIiLCJuYmYiOjE2NTAxMjU2NDUsImV4cCI6MTY1MjcxNzY0NSwiaWF0IjoxNjUwMTI1NjQ1LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.W1oeMYUk9lbIL0zxQO0SKhfiXrF3effLWdj1TD8zcqQ', 0),
	(386, 383, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzODMiLCJuYmYiOjE2NTAxMjU2NDUsImV4cCI6MTY1MjcxNzY0NSwiaWF0IjoxNjUwMTI1NjQ1LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.DwM5cGcEBtcObTWg3nqzlj8S2NtxGsjg5WbBfbY-eeY', 0),
	(387, 384, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzODQiLCJuYmYiOjE2NTAxMjU2NDYsImV4cCI6MTY1MjcxNzY0NiwiaWF0IjoxNjUwMTI1NjQ2LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.BEkdMJSsllEjX8AUYI6t9tKxPs2giwsaBpJmavq0gQo', 0),
	(388, 385, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzODUiLCJuYmYiOjE2NTAxMjU2NDYsImV4cCI6MTY1MjcxNzY0NiwiaWF0IjoxNjUwMTI1NjQ2LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.C3tB16Z-Xl8rmpQLWKGsfhjhAgiz5_I9Z-738_QnkLQ', 0),
	(389, 386, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzODYiLCJuYmYiOjE2NTAxMjU2NDYsImV4cCI6MTY1MjcxNzY0NiwiaWF0IjoxNjUwMTI1NjQ2LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.KxUf0fQmiptB8r8mSc7j2ucwIVYkJ1uFnIppdCqPruw', 0),
	(390, 387, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzODciLCJuYmYiOjE2NTAxMjU2NDYsImV4cCI6MTY1MjcxNzY0NiwiaWF0IjoxNjUwMTI1NjQ2LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.YDErk8YHGiFioX7BrCcxheNwzCWl9BE8Gp09z7bTRMs', 0),
	(391, 388, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzODgiLCJuYmYiOjE2NTAxMjU2NDYsImV4cCI6MTY1MjcxNzY0NiwiaWF0IjoxNjUwMTI1NjQ2LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.MQfPR93XU76rOfJpJ2cWNHTAZb4zEXqwQE6wgRCV2ao', 0),
	(392, 389, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzODkiLCJuYmYiOjE2NTAxMjU2NDcsImV4cCI6MTY1MjcxNzY0NywiaWF0IjoxNjUwMTI1NjQ3LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.AGeo4HKQfdBHYYxaINDlWnQwi33yeV3nYC5WWaDEeUg', 0),
	(393, 390, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzOTAiLCJuYmYiOjE2NTAxMjU2NDcsImV4cCI6MTY1MjcxNzY0NywiaWF0IjoxNjUwMTI1NjQ3LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.e0h2hRtlRpg_Lhli-If9-KtDnMcBD0BjHqBE05KJyCQ', 0),
	(394, 391, 'ChatFiller', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoicmVndWxhciIsInVzZXJfaWQiOiIzOTEiLCJuYmYiOjE2NTAxMjU2NDcsImV4cCI6MTY1MjcxNzY0NywiaWF0IjoxNjUwMTI1NjQ3LCJpc3MiOiJBdXRoU2VydmVyIiwiYXVkIjoiV2ViQ2xpZW50In0.YMTo5H9t7LKsv750Pc9wdk8RWfIwj5q00Ztgyuv9CD0', 0),
	(395, 5, 'chrome 100.0.4896 Windows 10', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoiYWRtaW4iLCJ1c2VyX2lkIjoiNSIsIm5iZiI6MTY1MDEyNzMwOSwiZXhwIjoxNjUyNzE5MzA5LCJpYXQiOjE2NTAxMjczMDksImlzcyI6IkF1dGhTZXJ2ZXIiLCJhdWQiOiJXZWJDbGllbnQifQ.0Glk711_SyOPs11T59MR58NgCFg6enkZg80FCn2UgSo', 0);

-- Dumping structure for table dcsm.settings
CREATE TABLE IF NOT EXISTS `settings` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(128) NOT NULL,
  `value` varchar(512) NOT NULL,
  `description` varchar(512) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table dcsm.settings: ~10 rows (approximately)
DELETE FROM `settings`;
INSERT INTO `settings` (`id`, `name`, `value`, `description`) VALUES
	(1, 'es_address', 'http://192.168.40.43:9200', 'Elasic search address'),
	(2, 'service_chat_info_port', '23578', 'Port for chat info service'),
	(3, 'service_messenger_port', '23579', 'Port for messenger server'),
	(4, 'service_registration_port', '23580', 'Port for registration service'),
	(5, 'service_authorization_port', '23581', 'Port for authorization service'),
	(8, 'rabbit_mq_address', '192.168.40.43', 'Rabbit MQ address'),
	(9, 'rabbit_mq_user', 'services', 'Rabbit MQ username'),
	(10, 'rabbit_mq_password', 'l9MqLHC6ca', 'Rabbit MQ password'),
	(11, 'service_userinfo_port', '23582', 'Port for user info service'),
	(12, 'service_orchestrator_port', '23583', 'Port for orchestrator service'),
	(13, 'redis_address', 'redis', 'Redis address'),
	(14, 'redis_port', '6379', 'Redis port'),
	(15, 'service_historical_messages_port', '80', 'Port for historical messages service (Orchestrator only)'),
	(16, 'instant-messages-service-port', '80', 'Port for Instant messages service (Orchestrator only)'),
	(17, 'message-service-port', '80', 'Port for message service (Orchestrator only)'),
	(18, 'subcribing-service-port', '80', 'Port for subcribing service (Orchestrator only)');

-- Dumping structure for table dcsm.user
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
) ENGINE=InnoDB AUTO_INCREMENT=392 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table dcsm.user: ~5 rows (approximately)
DELETE FROM `user`;
INSERT INTO `user` (`id`, `username`, `name`, `surname`, `email`, `password_hash`, `image_large`, `image_medium`, `image_small`, `salt`) VALUES
	(2, 'SYSTEM', 'SYSTEM', 'SYSTEM', NULL, 'SYSTEM', NULL, NULL, NULL, ''),
	(4, 'test', 'from', 'docker', 'docker@example.com', 'f9d45d4094cb7a0ca3e74038b84d7e353e4128d9de0b3955f897d4f71469f59f', NULL, NULL, NULL, 'c0bf3f4e047932d52bb610ad235b5b1d40295047'),
	(5, 'igor', 'И', 'Горь', 'i_gor@example.com', 'ed35a7e1411424a71a2b72717e901798812648055e4f863af4d32a9547f3d018', NULL, NULL, NULL, '307953995f0c129c99d9d866a22ca7dc9008f253'),
	(6, 'vladislav', 'Vlad и', 'Слав', 'vlad_i_slav@example.com', '376a9ad41e0a4fafe2fa39dcb9cf04f65c3eefdaac572c837f3b836355361038', NULL, NULL, NULL, 'd37b70d5d3c61fe292cc1ea1fe3800a018964b0a'),
	(7, 'user', 'Violet ', 'Nielsen', 'murebigi@mailinator.com', '0cdfbe44d40142ec91138bf3f1dbeefe8b71e88d7c40e97a703a6cf1799a852c', NULL, NULL, NULL, 'e6aa3f58c13a852fc985985ad75cbcc2b4d79744'),
	(8, 'admin', 'Иван', 'Иванов', 'ivan@mail.ru', '', '', '', '', '09cb67c812c441f821f40fccfc72e40fa10ac564'),
	(9, 'At nostrum cupidatat', 'Salvador', 'Kirk', 'lece@mailinator.com', 'd55b0c1608a5fab15cfdf36d7ad1803e890d7471f7d437f71b7e5933e7dd9330', NULL, NULL, NULL, '1def4849e5052a8df9ec33304984610fc5f9ded3'),
	(10, 'test_01', 'Тестовый', 'Пользователь', 'test@mail.ru', 'ae772efd5f5e0565f17d903904251d7399c672e8981fcf4fe2f07b5f7c040e5e', NULL, NULL, NULL, '6c5f4a4d217844f547c567bb7829ef4728003'),
	(289, 'generated_noTyhdRIfg', 'Pilot', ' ', 'generated_noTyhdRIfg@encamy.com', 'd25f085532620581903462d62bb6950997996f0391c40fb79d49a51e21694366', NULL, NULL, NULL, '949049004cf62fa36848deaf8b5eb5e52cbc237'),
	(290, 'generated_qDBhQSxrDr', 'Man', ' ', 'generated_qDBhQSxrDr@encamy.com', '3c705f2757b16a5e0358375222a7ed616a9510ca67af599342978cff663a1b23', NULL, NULL, NULL, '5f269e5406c01388caba933239e598863d91'),
	(291, 'generated_prRHswoAgL', 'The One', ' ', 'generated_prRHswoAgL@encamy.com', '446bb57c07db2a16b9878abd179cb727072bafcff5f83a5d11996d58a2f2d79a', NULL, NULL, NULL, '18adda54c972e12da9a01d9fe4822f93133b99'),
	(292, 'generated_gLvSsSDcAZ', 'Marty', ' ', 'generated_gLvSsSDcAZ@encamy.com', '05d7fe683ac60ef2de501b25ea3e66897da1da5543103c5f74b731b019fe7708', 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT5x47XMO527_x5On3gX5kAf3NGAsEfHbStyA&usqp=CAU', 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT5x47XMO527_x5On3gX5kAf3NGAsEfHbStyA&usqp=CAU', 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT5x47XMO527_x5On3gX5kAf3NGAsEfHbStyA&usqp=CAU', '1dec776eb1b6630f5d7e972bbba8d3c3faec1a00'),
	(293, 'generated_cdSqoOCzRy', 'Doc', ' ', 'generated_cdSqoOCzRy@encamy.com', '2a36afaba63d26cf662ff84a81245c7a74d6512b2d7595be6d538b7e37a6ee3c', 'https://c.tenor.com/xXXrBidPuPEAAAAC/back-to-the-future-doc-brown.gif', 'https://c.tenor.com/xXXrBidPuPEAAAAC/back-to-the-future-doc-brown.gif', 'https://c.tenor.com/xXXrBidPuPEAAAAC/back-to-the-future-doc-brown.gif', 'c5090eff018a2826cdf36b4a71a117e9de448bb6'),
	(294, 'generated_rxwAFtugVM', 'Jennifer', ' ', 'generated_rxwAFtugVM@encamy.com', 'eaf5f44e09c768b5dc964d9332b30a2fa7fdf99c253d92d0826f8658168dc0ce', NULL, NULL, NULL, '4b337812766b5f76d81120d7e9cd4a79086c8b1c'),
	(295, 'generated_QiEOAobvYb', 'Strickland', ' ', 'generated_QiEOAobvYb@encamy.com', '2313b21fd73e7bc90a66f76b49737907e606de52143a221cf20c3c6e2e0a8b7d', NULL, NULL, NULL, '979e65dacebac532d416179e38d864d7600513b5'),
	(296, 'generated_SPNAcfmuqG', 'Audition Judge', ' ', 'generated_SPNAcfmuqG@encamy.com', 'fde440e4380f215123c5cd6c083cdcf7157f6c906b7421ab483dbae9e5940eb0', NULL, NULL, NULL, '02d951f9d991e40f53793a821db1ab726952abd4'),
	(297, 'generated_RPCgFjFLBz', 'Election Van', ' ', 'generated_RPCgFjFLBz@encamy.com', '4631c7ea09a628eede388566b39a71c2177621e6651e4effb9ad790cd9b9f13f', NULL, NULL, NULL, '70fa0be468f892f0ffc81ccd5e76c1019d4f0eaa'),
	(298, 'generated_TuFDqkBbRC', 'Woman', ' ', 'generated_TuFDqkBbRC@encamy.com', '6ee37d5c7d5b094fde7dfc6cd117dfcd084a1197eb152b24100dc5ef1b4d5aff', NULL, NULL, NULL, 'a8363220bd27f9dd7aed4a7d54630b3c9c50a9c5'),
	(299, 'generated_LYisOCTcAz', 'Biff', ' ', 'generated_LYisOCTcAz@encamy.com', 'a08a497e170e6427e2d2d535c7860c737ef7daeb4f3296eb54fcd070967d45f2', NULL, NULL, NULL, 'dce63af4e389b61d3f30e9c30d30a37b2a3102d0'),
	(300, 'generated_TNjKCHuqDJ', 'George', ' ', 'generated_TNjKCHuqDJ@encamy.com', 'c7626822466e9faee7f9c863111542edf092d26449c5e8ca9a15d7db21b48653', NULL, NULL, NULL, '9f5adf5453db7a9d7ed37b3c8f2f1c95ad714763'),
	(301, 'generated_aYeRalDCZd', 'David', ' ', 'generated_aYeRalDCZd@encamy.com', '23113daae645dcd5c80cb9f39db38e28abf7ca0fb2762c5dbd53876f6bda2ca7', NULL, NULL, NULL, '7c813608487423888fd06fabc248dfdea6c1f'),
	(302, 'generated_VPknRqpize', 'Lorraine', ' ', 'generated_VPknRqpize@encamy.com', 'b2ef423927e61ad9041caedeeba126edf3ab07bb2ecbfe71f01a4dfb6707b343', NULL, NULL, NULL, '584a936091c607f538ab511d2fa2026071fcee4d'),
	(303, 'generated_COPYjmbyAI', 'Linda', ' ', 'generated_COPYjmbyAI@encamy.com', 'ca22567486442c3284065d6f5ba6c45c5365f3e9850abebd865e244989fbb776', NULL, NULL, NULL, '162fc541536b99b1f9840175c81a17201e46485f'),
	(304, 'generated_uNNiyjueYF', 'Loraine', ' ', 'generated_uNNiyjueYF@encamy.com', 'b1da054237ce5a7ddfd05fe462283df1dbadb5d4c29b632a3315d91af9741eff', NULL, NULL, NULL, '5aa608aef44f2b1d6d83b2310d32ea89fe4a9ad4'),
	(305, 'generated_KtYggUlESx', 'Libyan', ' ', 'generated_KtYggUlESx@encamy.com', 'f546dc7f7882e53b4940efc1c43888504254cdfc3a6db137944703fdea48e2ea', NULL, NULL, NULL, '350470453fe3ef578d323b69a13b67a85c9fd1f6'),
	(306, 'generated_ovuCAHYyvx', 'Mother', ' ', 'generated_ovuCAHYyvx@encamy.com', 'c6375110599004cc1d5174e9bd61a9a18902b02115e5e6d0d3702d1bcbb62311', NULL, NULL, NULL, 'b78cbd842d9b1517254ccb20022b71ee7835d074'),
	(307, 'generated_ycEWWceTXb', 'Father', ' ', 'generated_ycEWWceTXb@encamy.com', 'a09d823e3dfe4e806b2fab27cd11a51641f73249337bb2f53b3c5adec43882fa', NULL, NULL, NULL, 'a616123f38138097ff5b1989c9cda7eeb05558d'),
	(308, 'generated_xTRaqdcnrW', 'Son', ' ', 'generated_xTRaqdcnrW@encamy.com', '953ff4f463dfaf927e6bb8ff0fbc4399c36c894cf41fbe2d2d9b6bbbf383f4bf', NULL, NULL, NULL, '27b96ca55eaeb105c19507842a70b260316a3ec9'),
	(309, 'generated_HLmuloZTEO', 'Mother ', ' ', 'generated_HLmuloZTEO@encamy.com', '49a51338bf3a21aeb681e01d471e0e81568a22e9e3ac9e47dd8ef7d019fda063', NULL, NULL, NULL, 'c8e021dcb4bbe855b3f4f6833dbb2b2f2de1c4a7'),
	(310, 'generated_KqnrgeUOWF', 'Passenger', ' ', 'generated_KqnrgeUOWF@encamy.com', '2e0941b2f15daf776aab77b1701b8b69e7ed07c60d1e2d258edbf7bad2df6a83', NULL, NULL, NULL, '87ef7f689372aaa3775179d213196a359cd1df22'),
	(311, 'generated_FfMoJlLXbH', 'Lou', ' ', 'generated_FfMoJlLXbH@encamy.com', 'a1b4fb0a1e78aedb167078e4dd9e0cf7bdeb15323d5930845acd16a6a65a08c7', NULL, NULL, NULL, '1962874076a07e3b5df942e4b0f8acf84898ebf5'),
	(312, 'generated_AtIQKeuEfn', 'Skinhead', ' ', 'generated_AtIQKeuEfn@encamy.com', '08fbc103b65e1e79b790165a91c8052797d880e8aa5c2fa3a665a6c4222b2c11', NULL, NULL, NULL, '9fe58fda16441e9d991e93b8cb4631659114ed'),
	(313, 'generated_QuvNqzmXLO', 'Goldie', ' ', 'generated_QuvNqzmXLO@encamy.com', 'd6972093d6bf1872c9452910c6b91f5a7ba4b08dc441d3dbc9cfe02d4b3f71a2', NULL, NULL, NULL, '6c1e39759a1350677415ff4c800d369d662fbf4b'),
	(314, 'generated_edkAZIWrKp', 'Sam', ' ', 'generated_edkAZIWrKp@encamy.com', 'e66b24f91fcfc2e423da722a0cac98a659819ff9086da76919e267ce1e5b6be6', NULL, NULL, NULL, 'b04cb4d31d91674d11f9a2b91cccfa9aca26e5b9'),
	(315, 'generated_mzfHdgkvyh', 'Stella', ' ', 'generated_mzfHdgkvyh@encamy.com', '847a859b48b84b69374918e53fa33e85b2879a90f7927850e3f0deac279e9072', NULL, NULL, NULL, '143df83e21a11aae42deef301194e221df1e13ac'),
	(316, 'generated_ixLCdcrtzG', 'Milton', ' ', 'generated_ixLCdcrtzG@encamy.com', 'fb4969c2dd634736c9ff7f297456fe522313f76f7fb491af05323e2fc75042ba', NULL, NULL, NULL, 'e50730a5338bdc25d7b4b4c881b925f1e51c425f'),
	(317, 'generated_xDqfpgKvzW', 'TV Doc', ' ', 'generated_xDqfpgKvzW@encamy.com', '1741a52ac5b435a498ab82a99c463a818e18b500c222ee508908466a309a4d05', NULL, NULL, NULL, 'e392ae28b8cfbd432c326e17bdb8b612842bfab7'),
	(318, 'generated_HASfEJbPxo', 'Kid', ' ', 'generated_HASfEJbPxo@encamy.com', 'b7f687133194d57c5da53f27d2dce60d8087ad164718ff81686a27c3c88bcb1b', NULL, NULL, NULL, '1de7ec313df34c9b3516b4a39814d6c9'),
	(319, 'generated_KTzOziQwDs', 'Girl', ' ', 'generated_KTzOziQwDs@encamy.com', '34ab196c30ce10d62870259885955e203b6e746195c857b037809932942b960c', NULL, NULL, NULL, '614a46a39f9780cb1d2dcecf74811edebcc36272'),
	(320, 'generated_KTMArSobsX', 'Boy', ' ', 'generated_KTMArSobsX@encamy.com', 'adeba6fdf9229a948bf00f6d0ddb2d0a29983cd709b1b65c2520418502e5a50e', NULL, NULL, NULL, '46799fc1d379af5e86a8e97349212a647f3c1d17'),
	(321, 'generated_xibxHjgurN', 'Girlfriend ', ' ', 'generated_xibxHjgurN@encamy.com', 'afe6dec6122c4152cc9fd56ee1d81570d4fc86dc0718def7800e31c91286bee0', NULL, NULL, NULL, 'e73f734cbccb5f2d44e79141f8e4a21fa37deb63'),
	(322, 'generated_koZPoFMzmK', 'Radio', ' ', 'generated_koZPoFMzmK@encamy.com', 'fba2f08c29670d6489aba4e0beb330138a382fb878b37d55232a524faba7af40', NULL, NULL, NULL, '1f52ea8aa48456f4d9297f85a899266d0'),
	(323, 'generated_QQNiaDCqXx', 'Cop', ' ', 'generated_QQNiaDCqXx@encamy.com', '388b589ef9273d71ae8760613039f4e38b54d5e9ab225db296dd8b25f4d0395e', NULL, NULL, NULL, '8b6875f5ab9f14f2c607f06afd37189c52c264c5'),
	(324, 'generated_lmveLXFXtj', 'Marvin Barry', ' ', 'generated_lmveLXFXtj@encamy.com', '4fa17935cdaf4dd72d125caad2dc3c59a6e448d09eebacd7da7b628eb1f72835', NULL, NULL, NULL, '9b7a81cdba27f6cf57e5e192809c3dd94a904a84'),
	(325, 'generated_dnzDQtohDV', 'Starlighter', ' ', 'generated_dnzDQtohDV@encamy.com', '5581dbe589eaef14cdc1db41ac8452438c56c72487a92a117764aeaf69d1910b', NULL, NULL, NULL, '825563b1ccb5fd7466e8781f053b9e818354bb56'),
	(326, 'generated_rjDQkfujPJ', 'Girlfriend', ' ', 'generated_rjDQkfujPJ@encamy.com', '0c1e37312a517e2d2351c3186e78db153dfb1363b0c8b0be4b9fc96c9486f953', NULL, NULL, NULL, 'b056ab75fc7ac7abc040fdc374aecb786d578516'),
	(327, 'generated_QTRRooXLJV', 'Boyfriend', ' ', 'generated_QTRRooXLJV@encamy.com', '855e0b87a62d564fc6db121adab7b991eff3375078880f38f033f8ffde2c01de', NULL, NULL, NULL, '83c1647980137d2987e30969b123f8c2f02e0d'),
	(328, 'generated_uIBELHZIAP', 'Obnoxious Kid', ' ', 'generated_uIBELHZIAP@encamy.com', '6a2fa6730e49354d63e19aa02be02fc32af6ae61b7b3cf064b51b497117e4d9c', NULL, NULL, NULL, 'e9050ac080025be556a9fda331a9b8c481619dac'),
	(329, 'generated_weRPPXVbnk', 'Red', ' ', 'generated_weRPPXVbnk@encamy.com', 'd3b786a5d6a94445e5e9e636de9243cd9ed97382c1ea555a658088976bde4a1f', NULL, NULL, NULL, 'be6332290e388f5de76ca39e80c0946deb2b5091'),
	(330, 'generated_WVxbXMSpzi', 'Lynda', ' ', 'generated_WVxbXMSpzi@encamy.com', '1d05fe3e34dfd180d5fb2bc0e17219205d3156df133f6e51206d0d48645f32a8', NULL, NULL, NULL, 'a300f22a18ad5b8ce9c627202db19ea8b1bd88fa'),
	(331, 'generated_KCSDLHUGZb', 'Intercom', ' ', 'generated_KCSDLHUGZb@encamy.com', '2b45f831fae10b9e10f943c790b67d8153386265dc574b162bc69b90ccc9af3a', NULL, NULL, NULL, '63c277ec9fc5e04b0ee813bfeee3fb484'),
	(332, 'generated_ZeyKwOPbKt', 'Holden', ' ', 'generated_ZeyKwOPbKt@encamy.com', '02cdb8c371e3a1916ff2221b120aae25e6cfd12443651fc4b4d674af6747fd53', NULL, NULL, NULL, '2fa405cac488c8bc5bd17fbac7264dc608108448'),
	(333, 'generated_tsLrncLHvF', 'Leon', ' ', 'generated_tsLrncLHvF@encamy.com', 'b7442919cdd804efbf7f34abc6095c301b6ca99411c2587ba1c224b39598fce2', NULL, NULL, NULL, 'd8cc203df6d0fc86a9647ddea2f3e3407'),
	(334, 'generated_oceobLbgBZ', 'Overhead blimp', ' ', 'generated_oceobLbgBZ@encamy.com', '84b3e56eef2b9c9253e42ae5c47ee7a672511121bca8b272c76d13dd59d58735', NULL, NULL, NULL, 'a98dfc0016f319a124bdcab031af15b1991e5fae'),
	(335, 'generated_MYyOMYNWsE', 'Deckard ', ' ', 'generated_MYyOMYNWsE@encamy.com', '4267b9247d75aefd0cfd4f8a10852eb4e462645fc6fd68461374d78c9f264017', NULL, NULL, NULL, '2017dc768d8fc93175a801b6f44aa8c363d336d0'),
	(336, 'generated_GMJHluDLBB', 'Deckard', ' ', 'generated_GMJHluDLBB@encamy.com', '659af7d5562fac047b54162d822189842f9afbb20c5fe5a936e51edcb6d03704', NULL, NULL, NULL, '798197c4fd4814c8890914ce1a52acb57228d8a1'),
	(337, 'generated_aesiOHIVGe', 'Gaff', ' ', 'generated_aesiOHIVGe@encamy.com', '81a132cf16730c58577dd0785616b28cf5b57c4f7e1cffddadb344d4c422c3b4', NULL, NULL, NULL, 'b1aa1ab5b146e302ce012354d792a2e122afbcb4'),
	(338, 'generated_FnhDdwASVN', 'Chinese dude', ' ', 'generated_FnhDdwASVN@encamy.com', '87dc78f5d616c859ef3769d831b7c817041502231bbca78f092a3d602db09ca9', NULL, NULL, NULL, '7fb27594c04426694fe4728d3514a191fd6d0779'),
	(339, 'generated_YtKpRSVNJz', 'Spinner', ' ', 'generated_YtKpRSVNJz@encamy.com', 'b0da6fb7ff2972eaad0888e531bd2dd24ae7572857acc5a92e700a3e09ac8865', NULL, NULL, NULL, '89d74161d4e7884982c339b1e018d67e70'),
	(340, 'generated_OfepdwTcKA', 'Bryant', ' ', 'generated_OfepdwTcKA@encamy.com', '67087b33bd7cc89d8efd54ebcaa96bed8e8566250a9302552f6da3a5a7d7d614', NULL, NULL, NULL, '6d0d0f81ec53df3fef82ad3846f79c3ca898'),
	(341, 'generated_hDWygDFIjV', 'Decakrd', ' ', 'generated_hDWygDFIjV@encamy.com', '4e954cb1e77597f97ae6371907acaa7d6d0aa4c7edfd3c7076f1730df554c2a2', NULL, NULL, NULL, 'bcf6f79ffa74d40e5d83eab9341eed884c'),
	(342, 'generated_dByWJucoTQ', 'Leon ', ' ', 'generated_dByWJucoTQ@encamy.com', 'e75831c357c3599ea032477aba07edbdd6c57f2bfdfde410047ebe6f0f860f41', NULL, NULL, NULL, '6ae94e5c87a351f3364fbf110df06cd73dd65012'),
	(343, 'generated_OPZlURSTDy', 'Holden ', ' ', 'generated_OPZlURSTDy@encamy.com', 'b3f1a24d912685e738490398d998b69685d3654f7f267d266583de64f14cb0f8', NULL, NULL, NULL, '62fdb13eca578ad71a2f9fc7513d7b680e129644'),
	(344, 'generated_kQsLzghqal', 'Rachael', ' ', 'generated_kQsLzghqal@encamy.com', '54b652ee35e8a1667e0b5bf5102e9e8fa9c90e76536f784e5f2c30eeb83c7b87', NULL, NULL, NULL, 'ed492293d3f6a97191eec23439a418b195a86f'),
	(345, 'generated_PQDlFfoRAj', 'Tyrell', ' ', 'generated_PQDlFfoRAj@encamy.com', 'c9e0242841ba3968041919fbcdfb5bafbb6da70addb642395783fb2586d36a7f', NULL, NULL, NULL, 'ada8cf7357a55f4eacc95b6e3685ddc0e4c27'),
	(346, 'generated_QgpsclmTnF', 'Roy', ' ', 'generated_QgpsclmTnF@encamy.com', 'd57d21adcb8991c5c5f7460e5a7848fc8e90a9d018d80f34ec0a74800bbba8fd', NULL, NULL, NULL, 'a464adef0a0b5cc700ddb013ead93cc35be0de89'),
	(347, 'generated_HRkFYPpmUZ', 'Chew', ' ', 'generated_HRkFYPpmUZ@encamy.com', '4b5d6c9f521702bd4f81cd1a6731ddbdd2c4d49e7a55713f3f78e6374d5a3ef5', NULL, NULL, NULL, 'f413b046482831dc193f9b4a84e7f772646ec030'),
	(348, 'generated_acHdVQVNSk', 'Elevator', ' ', 'generated_acHdVQVNSk@encamy.com', '9eae1d8407c6921ef778aff222ce33fb9407b76d5358245a34dd212b7f6196c4', NULL, NULL, NULL, '0a2910981ed982df2859ffa1f10ea00318b2010'),
	(349, 'generated_KCXXDfYuDH', 'Pris', ' ', 'generated_KCXXDfYuDH@encamy.com', '858f9f9c803f33014cf8be10b8d5cd4d6ea2bb745d313e8c073e0b3ccbe15793', NULL, NULL, NULL, '209b8e71c31f52e87871c4220b9b8cd7d549d0de'),
	(350, 'generated_zynsMZQYKy', 'Sebastian', ' ', 'generated_zynsMZQYKy@encamy.com', '5db737eaeb75a09c626c242cc52519b692cc1c6cf65b17cea7d52078d2afe0a5', NULL, NULL, NULL, '93049b4387fc1f8f7699c4700c7f536deed08ebf'),
	(351, 'generated_RcSKoClIoN', 'Toys', ' ', 'generated_RcSKoClIoN@encamy.com', '9cc7d62e195bd9774816b33093d5c1424f1e0b31adc6b85beab3a8b0b68e882c', NULL, NULL, NULL, 'b2210abc9e99ce7380cd5385951aca2f29ec303c'),
	(352, 'generated_tSByIIEhPE', 'Toy 1', ' ', 'generated_tSByIIEhPE@encamy.com', 'f094d06432b6d7ca25707b7274e95b024b5f86871aab19ee5709e23b9fc7ebb1', NULL, NULL, NULL, '9a45d5dc9042fb1e47de0b8db2343426429f7a2'),
	(353, 'generated_spEWAaIIqH', 'Lady', ' ', 'generated_spEWAaIIqH@encamy.com', '1fd44e36e147894fa4a15701398871293b206e5a6607858f51bd5999259e091b', NULL, NULL, NULL, '17ec74321d0c9f0c6cd0fe851283194dd3e5a7'),
	(354, 'generated_LjyHZhloeu', 'Abdul', ' ', 'generated_LjyHZhloeu@encamy.com', '95bceab3ebf1863b6f6e811cf16d6f3a8f9d0b8e50f4697f262fc7b773a3b7ee', NULL, NULL, NULL, 'e8467831f3c9ccb9f64c80b3dd39c4e358efd5c6'),
	(355, 'generated_nWSGcyhFuD', 'Taffy', ' ', 'generated_nWSGcyhFuD@encamy.com', '6bfd2241488cc4688c0b0504295cad0c76246bddc45c27119a8c2bffb61047f0', NULL, NULL, NULL, 'd2db52cea949265e7d71281e1515810e3a'),
	(356, 'generated_WpNjuQBkQm', 'Announcer', ' ', 'generated_WpNjuQBkQm@encamy.com', '53b790f71b71f4a4f64bf47975bb221111f5b1497ce3702f56e48c2997aebd56', NULL, NULL, NULL, '3411b8ae3c42ac41268f11555156e10a5e150b37'),
	(357, 'generated_bnTaLLRqzS', 'Zhora', ' ', 'generated_bnTaLLRqzS@encamy.com', '77085cc69bd7b0d83ea30a029c6d861e7b849aeeab39f939939aef1d6a416a94', NULL, NULL, NULL, '534bdc62cd79d7b37d81560abb00387d6856c136'),
	(358, 'generated_PvvfnSpIsC', 'Hari Krishnas', ' ', 'generated_PvvfnSpIsC@encamy.com', '83aef6548ed40fc60cb066d49fd80e651d8f6898d2fc09717da5bc43729066b0', NULL, NULL, NULL, 'b55f0a5af3b7b367fdbba37dcdbefd6e87da57b4'),
	(359, 'generated_mAmfdwuJet', 'Street Thing', ' ', 'generated_mAmfdwuJet@encamy.com', 'fa0230bab3f30055e68e6e9207e9bd871610f13f175e0961a403bbe7176a1637', NULL, NULL, NULL, 'dc1e2bdac62bb9e23c2289dbbe6d86a0add8828f'),
	(360, 'generated_ZeSnwWxJjM', 'Street thing', ' ', 'generated_ZeSnwWxJjM@encamy.com', 'd729103830ac30cecc13d6643b679f653c73b34db947a1be1aba15e95e6101e7', NULL, NULL, NULL, 'b68fa1b6cecef7585efb22a3b8368b71e50e69e4'),
	(361, 'generated_DMmLTudnJi', 'Computer', ' ', 'generated_DMmLTudnJi@encamy.com', 'e7a72f124fdbbf811df4756b84e9250fe37a66b7288b80eab764b6d078b5369c', NULL, NULL, NULL, 'fa528be07bac434f201391b0c1a6e0a1f2cf1d7a'),
	(362, 'generated_IWdFbLXRLm', 'Cop', ' ', 'generated_IWdFbLXRLm@encamy.com', 'd3fd5833533ff35d072f2ad7ff0586e7aa0f98163e223d3ffc93dface172f64f', NULL, NULL, NULL, '3bec51274c7d8e80bf27c3f11562be5e150'),
	(363, 'generated_yHEREVNMPz', 'Gaff ', ' ', 'generated_yHEREVNMPz@encamy.com', '40a21cc221d558295042552cf698997a2c117129b0a5f523b487671f727522ae', NULL, NULL, NULL, 'dabfccd249fb73289cdfb22741e03e23dd5079f'),
	(364, 'generated_XJzvdpiqOW', 'Cypher', ' ', 'generated_XJzvdpiqOW@encamy.com', '0a8d910e67651289232c62df48afda7338264f2613782ce68ea9eb13987c4e65', NULL, NULL, NULL, '048f4a0bc43f865b466e16499c52944868357e3f'),
	(365, 'generated_fxhkXVGnOg', 'Trinity', ' ', 'generated_fxhkXVGnOg@encamy.com', '38d0cec6dec3ac56fc885422b67107a633eee0755af7f7223b19be9feeb2a4a9', NULL, NULL, NULL, '08eef391380b7bf011c26e22b8ed08204eec5966'),
	(366, 'generated_wEKVajZVUo', 'Cop', ' ', 'generated_wEKVajZVUo@encamy.com', '31462b5485c56530963fb95dfe884238e7a7eed05ac170b688be1f748ed8ed36', NULL, NULL, NULL, 'ab0cf0c652d0ef1ea7b6019cf40090c4e95e1c51'),
	(367, 'generated_LLAFmLMsCw', 'Agent Smith', ' ', 'generated_LLAFmLMsCw@encamy.com', 'd424e503f670023a2c7c26f195e9ae8546e15133fa50d0badd3423fd1bbc57f3', NULL, NULL, NULL, 'dfb8bdbbb112cf2676113c2b1c31a9f9ceaf7d43'),
	(368, 'generated_OaDQIcZDts', 'Lieutenant', ' ', 'generated_OaDQIcZDts@encamy.com', 'f9dd1fc23843b99e0c3ec57410445998c039826fdd96f485c59913cf19e3f216', NULL, NULL, NULL, '9fb8e574fd070ec9c9e92ec19b2424da97f55211'),
	(369, 'generated_sucwyqDchW', 'Morpheus', ' ', 'generated_sucwyqDchW@encamy.com', '85605f278ea75af68e14bc16c8fda6788acae1a694b136c8dbcc46c1b328fe09', NULL, NULL, NULL, '171275ac08b8b46deccba40e90c223a11047'),
	(370, 'generated_XYeJVqwQnr', 'Agent Brown', ' ', 'generated_XYeJVqwQnr@encamy.com', 'eef7bcb802b1382019bbb369cdadecd999525a4105aa79b1f2e7093a7d746c52', NULL, NULL, NULL, '9b3c95d96c9d2a6586732783da76653fa8ad1e3d'),
	(371, 'generated_fNSHRKnFit', 'Agent Jones', ' ', 'generated_fNSHRKnFit@encamy.com', '857166022cf0ec365b1ab4510a93bc0aa6734b4c96601b80b21b122bad761f9b', NULL, NULL, NULL, 'fd0559828ae62afcda41f3b23817c11984d5'),
	(372, 'generated_DJNnkDhdtC', 'Neo', ' ', 'generated_DJNnkDhdtC@encamy.com', 'e60714ec6bde01e0024b6833a63b35c1ef6b2da186ea44bf5c309c3452e6aeb8', NULL, NULL, NULL, '9210a7d353ce2fe01c982773bea39d30d6a24334'),
	(373, 'generated_teCOGdbWvC', 'Choi', ' ', 'generated_teCOGdbWvC@encamy.com', 'e58d5e1191edf5762e730413f5e36df5e1516edf4d11e958673e70edb88d85fd', NULL, NULL, NULL, '7a6ab6a8fb4778b66c962062c2b5374f5470d62f'),
	(374, 'generated_ZYmnyQSlLF', 'DuJour', ' ', 'generated_ZYmnyQSlLF@encamy.com', 'd8fbb9e6b17513d10cbd0d58ebe488cbf39d7baffc79f96fc508c1e930169424', NULL, NULL, NULL, '46ddb290ff3aa82e7aaec1d2c4a00b55dc4957d'),
	(375, 'generated_NkBnatUQmk', 'Mr. Rhineheart', ' ', 'generated_NkBnatUQmk@encamy.com', '6554f12d0deb4f91fc517cbef101d1c8bffd1d5c2e297047c7c3782722001e14', NULL, NULL, NULL, '0cdace9972fbb408286b61f7f3eaeac980e3ab1c'),
	(376, 'generated_NgZcutswGB', 'FedEx man', ' ', 'generated_NgZcutswGB@encamy.com', '74e3a4fdef1745087e6d2e4c3af0b9403e3431bdc31f567202463df9aef0df1e', NULL, NULL, NULL, '29071a1607380c6ec07927a66b632e446e48d842'),
	(377, 'generated_ILYXdRQBCP', 'Switch', ' ', 'generated_ILYXdRQBCP@encamy.com', '3189af0dae8fdcc97b6166d39e99355bcf70557224cfd50a9ff7444ce2ee9ea0', NULL, NULL, NULL, '5c0a663870ad1343ad51fc05068072d20914b865'),
	(378, 'generated_yQBGshfVEl', 'Apoc', ' ', 'generated_yQBGshfVEl@encamy.com', '101839f7d7836bc92bdeb0899474588a6657c939599db84f5be30d0f3ec2cc41', NULL, NULL, NULL, 'cfeab2c89e5e3b177a14415143ad390032915'),
	(379, 'generated_zrsFYPLuDP', 'Dozer', ' ', 'generated_zrsFYPLuDP@encamy.com', 'e8d5985959aed27e5bac0a8d71332d20d6de2f219b76c2745f9d262d3f86ae19', NULL, NULL, NULL, '59e7a1cfa6c6d836290169672ecf849afdda3c54'),
	(380, 'generated_vUrqNMYLUU', 'Tank', ' ', 'generated_vUrqNMYLUU@encamy.com', '4db7069361641951066019950e123b6f246fce2d245925a29d8f9d13ed50521c', NULL, NULL, NULL, 'ff8653b9a1489d33fae673cd2d8cb08d94c8c953'),
	(381, 'generated_eSmjbBorih', 'Mouse', ' ', 'generated_eSmjbBorih@encamy.com', '6643ca4e530096a48dc7279efe8d8ffdf5118c9c8fabe280a45bec50c6db2bce', NULL, NULL, NULL, '43633ba4663133a39d907b7bb1adfda057476'),
	(382, 'generated_AAaYXvRbul', 'Priestess', ' ', 'generated_AAaYXvRbul@encamy.com', '39e475c9d77db90a65ea2b7ed9b6b2275a0e54f99e6f090372ca8eccb794cf59', NULL, NULL, NULL, '7bfe1029cee112df5813458adeb4743be'),
	(383, 'generated_zsaFcnaiew', 'Spoon boy', ' ', 'generated_zsaFcnaiew@encamy.com', '7d611b1fb104fa529c66ada59ed1b5081e440762b4cc7f71d58c694c58ff1320', NULL, NULL, NULL, 'a0c151a9b00feda066aac65b743bebf612af95a2'),
	(384, 'generated_puJEKtDpQR', 'Oracle', ' ', 'generated_puJEKtDpQR@encamy.com', '740e4222706a3d8288181fa27efcaec31e3b7aec61d6a989ca78944b8984b985', NULL, NULL, NULL, 'df96522081c794bdeeabca8f787e3f9596e1b402'),
	(385, 'generated_SsVPIcZIIg', 'Police', ' ', 'generated_SsVPIcZIIg@encamy.com', '858197a7e33555fec829f7d71d490121cd76cc702c98bbf454668e1630b8e2c4', NULL, NULL, NULL, '33126e043f4f9a1290563ff2351076d22716ce2e'),
	(386, 'generated_MiEnvwdDkn', 'Guard 1', ' ', 'generated_MiEnvwdDkn@encamy.com', '4b54047993da0435e96cb4f37da4f15d1c3fcd5528cbba5137f83abbde83bd46', NULL, NULL, NULL, 'f71fa2ce137ebfa93b5f25d7bb6e3a2cc94b54af'),
	(387, 'generated_QJWqZrhbib', 'Guard 2', ' ', 'generated_QJWqZrhbib@encamy.com', '479eb0c4654a0b3489f097009ac97c9f6635bccaced43cf74dc2d86dfda4d303', NULL, NULL, NULL, '68cd8fdcdbb35de815979e6490f66b636bf701c8'),
	(388, 'generated_EvKUaakFKm', 'Soldier', ' ', 'generated_EvKUaakFKm@encamy.com', '9ef614932e56728f52b94187181a053574b9ed6e7f2d0d3e83ea5d5d2cde8172', NULL, NULL, NULL, 'c8abd30fc77aacc0db1862ffa040e8c5af2169f0'),
	(389, 'generated_jRNMUInpNk', 'Pilot', ' ', 'generated_jRNMUInpNk@encamy.com', 'd33dc758fc48e028151808cfc980d131ea3893268d9589f04b50a5c2822e269d', NULL, NULL, NULL, '6002e3914ab81adfe8678f4e0a33e1550b6e7769'),
	(390, 'generated_mbFKJctljd', 'Man', ' ', 'generated_mbFKJctljd@encamy.com', '59489e6fe61463facf081737370fed9b78226c147f67f206c28250eac614205a', NULL, NULL, NULL, '76d961c68340eafc3f247764fbb8c622ec64c67f'),
	(391, 'generated_PkdZINVghz', 'The One', ' ', 'generated_PkdZINVghz@encamy.com', '16dc8e271b9511de61bcb4b26cb4a9ce614486ecbf071c3d36b18b6d90e4266d', NULL, NULL, NULL, '91463372a72f557399c3e96b4628b6ee15cebc68');

-- Dumping structure for table dcsm.user_role
CREATE TABLE IF NOT EXISTS `user_role` (
  `id` int NOT NULL AUTO_INCREMENT,
  `role` varchar(45) NOT NULL,
  `description` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table dcsm.user_role: ~0 rows (approximately)
DELETE FROM `user_role`;
INSERT INTO `user_role` (`id`, `role`, `description`) VALUES
	(1, 'SYSTEM', 'Система'),
	(2, 'admin', 'Администратор'),
	(3, 'regular', 'Пользователь');

-- Dumping structure for table dcsm.user_roles
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

-- Dumping data for table dcsm.user_roles: ~5 rows (approximately)
DELETE FROM `user_roles`;
INSERT INTO `user_roles` (`user_id`, `role_id`, `assigned_by`, `date_assigned`) VALUES
	(4, 3, 2, '2021-07-18 17:19:32'),
	(5, 2, 2, '2021-07-18 18:34:08'),
	(6, 3, 2, '2021-07-18 18:38:40'),
	(7, 3, 2, '2022-02-17 20:49:07'),
	(8, 2, 2, '2022-02-12 21:13:14'),
	(9, 3, 2, '2022-04-16 12:46:57'),
	(10, 3, 2, '2022-04-16 16:36:05'),
	(292, 3, 2, '2022-04-16 19:08:47'),
	(293, 3, 2, '2022-04-16 19:08:47'),
	(294, 3, 2, '2022-04-16 19:08:48'),
	(295, 3, 2, '2022-04-16 19:08:48'),
	(296, 3, 2, '2022-04-16 19:08:48'),
	(297, 3, 2, '2022-04-16 19:08:48'),
	(298, 3, 2, '2022-04-16 19:08:48'),
	(299, 3, 2, '2022-04-16 19:08:49'),
	(300, 3, 2, '2022-04-16 19:08:49'),
	(301, 3, 2, '2022-04-16 19:08:49'),
	(302, 3, 2, '2022-04-16 19:08:49'),
	(303, 3, 2, '2022-04-16 19:08:50'),
	(304, 3, 2, '2022-04-16 19:08:50'),
	(305, 3, 2, '2022-04-16 19:08:50'),
	(306, 3, 2, '2022-04-16 19:08:50'),
	(307, 3, 2, '2022-04-16 19:08:51'),
	(308, 3, 2, '2022-04-16 19:08:51'),
	(309, 3, 2, '2022-04-16 19:08:51'),
	(310, 3, 2, '2022-04-16 19:08:51'),
	(311, 3, 2, '2022-04-16 19:08:52'),
	(312, 3, 2, '2022-04-16 19:08:52'),
	(313, 3, 2, '2022-04-16 19:08:52'),
	(314, 3, 2, '2022-04-16 19:08:52'),
	(315, 3, 2, '2022-04-16 19:08:52'),
	(316, 3, 2, '2022-04-16 19:08:53'),
	(317, 3, 2, '2022-04-16 19:08:53'),
	(318, 3, 2, '2022-04-16 19:08:53'),
	(319, 3, 2, '2022-04-16 19:08:53'),
	(320, 3, 2, '2022-04-16 19:08:54'),
	(321, 3, 2, '2022-04-16 19:08:54'),
	(322, 3, 2, '2022-04-16 19:08:54'),
	(323, 3, 2, '2022-04-16 19:08:54'),
	(324, 3, 2, '2022-04-16 19:08:54'),
	(325, 3, 2, '2022-04-16 19:08:55'),
	(326, 3, 2, '2022-04-16 19:08:55'),
	(327, 3, 2, '2022-04-16 19:08:55'),
	(328, 3, 2, '2022-04-16 19:08:55'),
	(329, 3, 2, '2022-04-16 19:08:55'),
	(330, 3, 2, '2022-04-16 19:08:56'),
	(331, 3, 2, '2022-04-16 19:11:57'),
	(332, 3, 2, '2022-04-16 19:11:58'),
	(333, 3, 2, '2022-04-16 19:11:58'),
	(334, 3, 2, '2022-04-16 19:11:58'),
	(335, 3, 2, '2022-04-16 19:11:58'),
	(336, 3, 2, '2022-04-16 19:11:58'),
	(337, 3, 2, '2022-04-16 19:11:59'),
	(338, 3, 2, '2022-04-16 19:11:59'),
	(339, 3, 2, '2022-04-16 19:11:59'),
	(340, 3, 2, '2022-04-16 19:11:59'),
	(341, 3, 2, '2022-04-16 19:12:00'),
	(342, 3, 2, '2022-04-16 19:12:00'),
	(343, 3, 2, '2022-04-16 19:12:00'),
	(344, 3, 2, '2022-04-16 19:12:00'),
	(345, 3, 2, '2022-04-16 19:12:00'),
	(346, 3, 2, '2022-04-16 19:12:01'),
	(347, 3, 2, '2022-04-16 19:12:01'),
	(348, 3, 2, '2022-04-16 19:12:01'),
	(349, 3, 2, '2022-04-16 19:12:01'),
	(350, 3, 2, '2022-04-16 19:12:01'),
	(351, 3, 2, '2022-04-16 19:12:02'),
	(352, 3, 2, '2022-04-16 19:12:02'),
	(353, 3, 2, '2022-04-16 19:12:02'),
	(354, 3, 2, '2022-04-16 19:12:02'),
	(355, 3, 2, '2022-04-16 19:12:03'),
	(356, 3, 2, '2022-04-16 19:12:03'),
	(357, 3, 2, '2022-04-16 19:12:03'),
	(358, 3, 2, '2022-04-16 19:12:03'),
	(359, 3, 2, '2022-04-16 19:12:03'),
	(360, 3, 2, '2022-04-16 19:12:04'),
	(361, 3, 2, '2022-04-16 19:12:04'),
	(362, 3, 2, '2022-04-16 19:12:04'),
	(363, 3, 2, '2022-04-16 19:12:04'),
	(364, 3, 2, '2022-04-16 19:14:01'),
	(365, 3, 2, '2022-04-16 19:14:02'),
	(366, 3, 2, '2022-04-16 19:14:02'),
	(367, 3, 2, '2022-04-16 19:14:02'),
	(368, 3, 2, '2022-04-16 19:14:02'),
	(369, 3, 2, '2022-04-16 19:14:02'),
	(370, 3, 2, '2022-04-16 19:14:03'),
	(371, 3, 2, '2022-04-16 19:14:03'),
	(372, 3, 2, '2022-04-16 19:14:03'),
	(373, 3, 2, '2022-04-16 19:14:03'),
	(374, 3, 2, '2022-04-16 19:14:03'),
	(375, 3, 2, '2022-04-16 19:14:04'),
	(376, 3, 2, '2022-04-16 19:14:04'),
	(377, 3, 2, '2022-04-16 19:14:04'),
	(378, 3, 2, '2022-04-16 19:14:04'),
	(379, 3, 2, '2022-04-16 19:14:04'),
	(380, 3, 2, '2022-04-16 19:14:05'),
	(381, 3, 2, '2022-04-16 19:14:05'),
	(382, 3, 2, '2022-04-16 19:14:05'),
	(383, 3, 2, '2022-04-16 19:14:05'),
	(384, 3, 2, '2022-04-16 19:14:05'),
	(385, 3, 2, '2022-04-16 19:14:06'),
	(386, 3, 2, '2022-04-16 19:14:06'),
	(387, 3, 2, '2022-04-16 19:14:06'),
	(388, 3, 2, '2022-04-16 19:14:06'),
	(389, 3, 2, '2022-04-16 19:14:07'),
	(390, 3, 2, '2022-04-16 19:14:07'),
	(391, 3, 2, '2022-04-16 19:14:07');

-- Dumping structure for view dcsm.v_chat_members
-- Creating temporary table to overcome VIEW dependency errors
CREATE TABLE `v_chat_members` (
	`id` INT(10) NOT NULL,
	`joined_at` DATETIME NULL,
	`chat_id` INT(10) NOT NULL,
	`user_id` INT(10) NOT NULL,
	`username` VARCHAR(255) NOT NULL COLLATE 'utf8mb4_0900_ai_ci',
	`name` VARCHAR(255) NOT NULL COLLATE 'utf8mb4_0900_ai_ci',
	`surname` VARCHAR(255) NOT NULL COLLATE 'utf8mb4_0900_ai_ci',
	`image_small` VARCHAR(255) NULL COLLATE 'utf8mb4_0900_ai_ci',
	`image_medium` VARCHAR(255) NULL COLLATE 'utf8mb4_0900_ai_ci',
	`image_large` VARCHAR(255) NULL COLLATE 'utf8mb4_0900_ai_ci',
	`invited_by_username` VARCHAR(255) NOT NULL COLLATE 'utf8mb4_0900_ai_ci',
	`invited_by_name` VARCHAR(255) NOT NULL COLLATE 'utf8mb4_0900_ai_ci',
	`invited_by_surname` VARCHAR(255) NOT NULL COLLATE 'utf8mb4_0900_ai_ci'
) ENGINE=MyISAM;

-- Dumping structure for view dcsm.v_chat_members
-- Removing temporary table and create final VIEW structure
DROP TABLE IF EXISTS `v_chat_members`;
CREATE ALGORITHM=UNDEFINED SQL SECURITY DEFINER VIEW `v_chat_members` AS select `chat_members`.`chat_id` AS `id`,`chat_members`.`joined_at` AS `joined_at`,`chat_members`.`chat_id` AS `chat_id`,`member`.`id` AS `user_id`,`member`.`username` AS `username`,`member`.`name` AS `name`,`member`.`surname` AS `surname`,`member`.`image_small` AS `image_small`,`member`.`image_medium` AS `image_medium`,`member`.`image_large` AS `image_large`,`inviter`.`username` AS `invited_by_username`,`inviter`.`name` AS `invited_by_name`,`inviter`.`surname` AS `invited_by_surname` from ((`chat_members` join `user` `member` on((`chat_members`.`user_id` = `member`.`id`))) join `user` `inviter` on((`chat_members`.`added_by` = `inviter`.`id`)));

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
