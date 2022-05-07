using Messenger.Common.MySql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public static async Task<string> ReadSettingsAsync(string name)
        {
            string value = string.Empty;
            await GlobalSettings.Instance.Database.ExecuteSqlAsync(
                @$"SELECT `value` FROM `settings`
                WHERE `settings`.`name` = '{name}'
                LIMIT 1",
                reader =>
                {
                    value = reader.GetString("value");
                });

            return value;
        }

        public static T ReadSettings<T>(string name)
        {
            string value = ReadSettings(name);
            T convertedValue = (T)Convert.ChangeType(value, typeof(T));
            return convertedValue;
        }

        public static async Task<T> ReadSettingsAsync<T>(string name)
        {
            string value = await ReadSettingsAsync(name);
            T convertedValue = (T)Convert.ChangeType(value, typeof(T));
            return convertedValue;
        }

        public static void ChangeSettings(string name, string newValue)
        {
            string value = string.Empty;
            GlobalSettings.Instance.Database.ExecuteSql(
                @$"UPDATE `settings`
                SET `value` = @p_new_value
                WHERE `settings`.`name` = @p_name",
                parameters: new Dictionary<string, object>
                {
                    { "p_name", name },
                    { "p_new_value", newValue }
                });
        }

        public static async Task ChangeSettingsAsync(string name, string newValue)
        {
            string value = string.Empty;
            await GlobalSettings.Instance.Database.ExecuteSqlAsync(
                @$"UPDATE `settings`
                SET `value` = @p_new_value
                WHERE `settings`.`name` = @p_name",
                parameters: new Dictionary<string, object>
                {
                    { "p_name", name },
                    { "p_new_value", newValue }
                });
        }
    }
}
