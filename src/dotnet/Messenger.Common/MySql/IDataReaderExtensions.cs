using System;
using System.Data;

namespace Messenger.Common.MySql
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

        public static int? GetInt32(this IDataRecord record, string fieldName)
        {
            int fieldId = record.GetOrdinal(fieldName);

            if (record.IsDBNull(fieldId))
            {
                return null;
            }

            return record.GetInt32(fieldId);
        }

        public static short? GetInt16(this IDataRecord record, string fieldName)
        {
            int fieldId = record.GetOrdinal(fieldName);

            if (record.IsDBNull(fieldId))
            {
                return null;
            }

            return record.GetInt16(fieldId);
        }

        public static long? GetInt64(this IDataRecord record, string fieldName)
        {
            int fieldId = record.GetOrdinal(fieldName);

            if (record.IsDBNull(fieldId))
            {
                return null;
            }

            return record.GetInt64(fieldId);
        }
    }
}
