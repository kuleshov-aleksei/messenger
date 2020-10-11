USE `dcsm`;
DROP procedure IF EXISTS `p_valid_password_email`;

DELIMITER $$
USE `dcsm`$$
CREATE DEFINER=`dcsm`@`%` PROCEDURE `p_valid_password_email`(
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
END$$

DELIMITER ;

