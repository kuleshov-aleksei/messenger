USE `dcsm`;
CREATE  OR REPLACE VIEW `chat_members_view` AS
SELECT 
    `chat_members`.`chat_id` AS id,
    `chat_members`.`joined_at`,
    `member`.`username` AS 'username',
    `member`.`name` AS 'name',
    `member`.`surname` AS 'surname',
    `inviter`.`username` AS 'invited_by_username',
    `inviter`.`name` AS 'invited_by_name',
    `inviter`.`surname` AS 'invited_by_surname'
FROM
    `chat_members`
        INNER JOIN
    `user` `member` ON `chat_members`.`user_id` = `member`.`id`
        INNER JOIN
    `user` `inviter` ON `chat_members`.`added_by` = `inviter`.`id`;
