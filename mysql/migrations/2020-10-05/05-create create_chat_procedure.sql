USE `dcsm`;
DROP procedure IF EXISTS `p_create_chat`;

DELIMITER $$
USE `dcsm`$$
CREATE DEFINER = `dcsm` @`%` PROCEDURE `p_create_chat`(
  IN title VARCHAR(255), 
  IN user_id INT
) BEGIN DECLARE row_id INT;
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
END$$

DELIMITER ;

