using System;
using System.Data;

namespace MySql.Common
{
    public static class IDataReaderExtensions
    {
        public static string GetString(this IDataRecord record, string fieldName)
        {
            int fieldId = record.GetOrdinal(fieldName);

            if (record.IsDBNull(fieldId))
            {
                return null;
            }

            return record.GetString(fieldId);
        }

        public static DateTime? GetDateTime(this IDataRecord record, string fieldName)
        {
            int fieldId = record.GetOrdinal(fieldName);

            if (record.IsDBNull(fieldId))
            {
                return null;
            }

            return record.GetDateTime(fieldId);
        }

    }
}
