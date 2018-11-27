using System;

namespace Worktile.Common
{
    public static class WtDateTimeHelper
    {
        public static DateTime GetDateTime(string timestamp)
        {
            long ticks = long.Parse(timestamp);
            return GetDateTime(ticks);
        }

        public static DateTime GetDateTime(long timestamp) => new DateTime(timestamp * 10000000 + 621355968000000000).ToLocalTime();

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
    }
}
