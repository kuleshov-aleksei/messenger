USE `dcsm`;
DROP procedure IF EXISTS `p_add_user_to_chat`;

DELIMITER $$
USE `dcsm`$$
CREATE PROCEDURE `p_add_user_to_chat` (
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
END$$

DELIMITER ;

