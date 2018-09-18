using System;
using System.Globalization;

namespace Dualog.eCatch.Shared.Extensions
{
    public static class DateTimeExtensions
    {
        private const string DateFormat = "yyyyMMdd";
        private const string TimeFormat = "HHmm";

        public static string ToFormattedDate(this DateTime dt)
        {
            return dt.ToString(DateFormat);
        }

        public static string ToFormattedTime(this DateTime dt)
        {
            return dt.ToString(TimeFormat);
        }

        public static DateTime FromFormattedDateTime(this string dateTimeString)
        {
            return DateTime.ParseExact(dateTimeString, DateFormat + TimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None);
        }
    }
}