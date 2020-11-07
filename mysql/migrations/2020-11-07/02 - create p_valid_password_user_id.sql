USE `dcsm`;
DROP procedure IF EXISTS `p_valid_password_user_id`;

DELIMITER $$
USE `dcsm`$$
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
END$$

DELIMITER ;

