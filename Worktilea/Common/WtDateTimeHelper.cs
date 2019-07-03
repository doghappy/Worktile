using System;

namespace Worktile.Common
{
    public static class WtDateTimeHelper
    {
        public static DateTime GetDateTime(string timestamp)
        {
            long ticks = long.Parse(timestamp);
            return UnixToDateTime(ticks);
        }

        public static DateTime UnixToDateTime(long unix)
        {
            var date = DateTimeOffset.FromUnixTimeSeconds(unix);
            return date.LocalDateTime;
        }

        public static string ToWtKanbanDate(this DateTime date)
        {
            if (date == new DateTime(date.Year, date.Month, date.Day))
            {
                return date.ToShortDateString();
            }
            else
            {
                return date.ToString("G");
            }
        }

        public static string ToWtKanbanDate(string timestamp)
        {
            DateTime dateTime = GetDateTime(timestamp);
            return dateTime.ToWtKanbanDate();
        }
    }
}
