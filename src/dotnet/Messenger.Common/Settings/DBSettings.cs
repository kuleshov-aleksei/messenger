using MySql.Common;

namespace Messenger.Common.Settings
{
    public static class DBSettings
    {
        public static string ReadSettings(string name)
        {
            string value = string.Empty;
            GlobalSettings.Instance.Database.ExecuteSql(
                @$"SELECT `value` FROM `settings`
                WHERE `settings`.`name` = '{name}'
                LIMIT 1",
                reader =>
                {
                    value = reader.GetString("value");
                });

            return value;
        }
    }
}
