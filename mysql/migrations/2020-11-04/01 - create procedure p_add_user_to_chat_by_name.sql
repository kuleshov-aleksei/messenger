USE `dcsm`;
DROP procedure IF EXISTS `p_add_user_to_chat_by_name`;

DELIMITER $$
USE `dcsm`$$
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
END$$

DELIMITER ;

