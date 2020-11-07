USE `dcsm`;
DROP procedure IF EXISTS `p_change_password`;

DELIMITER $$
USE `dcsm`$$
CREATE DEFINER=`dcsm`@`%` PROCEDURE `p_change_password`(
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
		-- Generate new salt on password change. Do not reuse
		SET salt_length = (SELECT FLOOR(RAND() * (64 - 32 + 1) + 32));
		SET salt = (SELECT SUBSTRING(SHA1(RAND()), 1, salt_length));
		SET salted_hash = (SELECT SHA2(CONCAT(salt, p_new_password), 256));
        
        UPDATE `user`
        SET `salt` = salt,
			`password_hash` = salted_hash
        WHERE `id` = p_user_id;
    END IF;
END$$

DELIMITER ;

