/*** ---------------------------------------------------------------------------
/// DateUtils.cs
/// 
/// <company>Honey Rage Games</company>
/// <date>June 3rd, 2017</date>
/// ------------------------------------------------------------------------***/

using System;

namespace core.util
{
    /// <summary>
    /// Utility static methods around DateTime
    /// </summary>
    public class DateUtils
    {
        public static DateTime GetDateTimeFromEpoch(long value)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            epoch = epoch.AddSeconds(value);

            return epoch;
        }

        public static long GetEpochFromDateTime(DateTime date)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }
    }
}