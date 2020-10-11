USE `dcsm`;
DROP procedure IF EXISTS `p_create_user`;

DELIMITER $$
USE `dcsm`$$
CREATE PROCEDURE `p_create_user` (
	IN `p_username` VARCHAR(255),
    IN `p_name` VARCHAR(255),
    IN `p_surname` VARCHAR(255),
    IN `p_email` VARCHAR(255),
    IN `p_password` VARCHAR(256)
)
BEGIN
	DECLARE salt_length INT;
    DECLARE salt VARCHAR(64);
    DECLARE salted_hash VARCHAR(256);
    
    SET salt_length = (SELECT FLOOR(RAND() * (64 - 32 + 1) + 32));
    SET salt = (SELECT SUBSTRING(SHA1(RAND()), 1, salt_length));
    SET salted_hash = (SELECT SHA2(CONCAT(salt, p_password), 256));
    
    INSERT INTO `user` (`username`, `name`, `surname`, `email`, `salt`, `password_hash`)
		VALUES (p_username, p_name, p_surname, p_email, salt, salted_hash);
END$$

DELIMITER ;

