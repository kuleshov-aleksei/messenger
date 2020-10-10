USE `dcsm`;
CREATE 
     OR REPLACE ALGORITHM = UNDEFINED 
    DEFINER = `dcsm`@`%` 
    SQL SECURITY DEFINER
VIEW `dcsm`.`v_chat_members` AS
    SELECT 
        `dcsm`.`chat_members`.`chat_id` AS `id`,
        `dcsm`.`chat_members`.`joined_at` AS `joined_at`,
        `dcsm`.`chat_members`.`chat_id` AS `chat_id`,
        `member`.`id` AS `user_id`,
        `member`.`username` AS `username`,
        `member`.`name` AS `name`,
        `member`.`surname` AS `surname`,
        `member`.`image_small` AS `image_small`,
        `member`.`image_medium` AS `image_medium`,
        `member`.`image_large` AS `image_large`,
        `inviter`.`username` AS `invited_by_username`,
        `inviter`.`name` AS `invited_by_name`,
        `inviter`.`surname` AS `invited_by_surname`
    FROM
        ((`dcsm`.`chat_members`
        JOIN `dcsm`.`user` `member` ON ((`dcsm`.`chat_members`.`user_id` = `member`.`id`)))
        JOIN `dcsm`.`user` `inviter` ON ((`dcsm`.`chat_members`.`added_by` = `inviter`.`id`)));
