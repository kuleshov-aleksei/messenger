USE `dcsm`;
DROP procedure IF EXISTS `p_valid_password_username`;

DELIMITER $$
USE `dcsm`$$
CREATE PROCEDURE `p_valid_password_username` (
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
END$$

DELIMITER ;

