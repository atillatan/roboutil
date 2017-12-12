using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboUtil
{
    partial class Utils
    {
        public enum Days { Sunday = 1, Monday = 2, Tuesday = 3, Wednesday = 4, Thursday = 5, Friday = 6, Saturday = 7 };

        public enum DaysTr { Pazar = 1, Pazartesi = 2, Salı = 3, Çarşamba = 4, Perşembe = 5, Cuma = 6, Cumartesi = 7 };

        public enum Months : byte { Jan = 1, Feb = 2, Mar = 3, Apr = 4, May = 5, Jun = 6, Jul = 7, Aug = 8, Sep = 9, Oct = 10, Nov = 11, Dec = 12 };

        public enum MonthsTr : byte { Ocak = 1, Şubat = 2, Mart = 3, Nisan = 4, Mayıs = 5, Haziran = 6, Temmuz = 7, Ağustos = 8, Eylül = 9, Ekim = 10, Kasım = 11, Aralık = 12 };

        public class DateUtil
        {
            public static string ToReadableTime(DateTime value)
            {
                var ts = new TimeSpan(DateTime.UtcNow.Ticks - value.Ticks);
                double delta = ts.TotalSeconds;
                if (delta < 60)
                {
                    return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";
                }
                if (delta < 120)
                {
                    return "a minute ago";
                }
                if (delta < 2700) // 45 * 60
                {
                    return ts.Minutes + " minutes ago";
                }
                if (delta < 5400) // 90 * 60
                {
                    return "an hour ago";
                }
                if (delta < 86400) // 24 * 60 * 60
                {
                    return ts.Hours + " hours ago";
                }
                if (delta < 172800) // 48 * 60 * 60
                {
                    return "yesterday";
                }
                if (delta < 2592000) // 30 * 24 * 60 * 60
                {
                    return ts.Days + " days ago";
                }
                if (delta < 31104000) // 12 * 30 * 24 * 60 * 60
                {
                    int months = System.Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                    return months <= 1 ? "one month ago" : months + " months ago";
                }
                var years = System.Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "one year ago" : years + " years ago";
            }
        }

        public static DateTime NextWorkday(this DateTime date)
        {
            var nextDay = date;

            while (!IsWorkingDay(nextDay))
            {
                nextDay = nextDay.AddDays(1);
            }
            return nextDay;
        }

        public static bool Between(this DateTime dt, DateTime rangeBeg, DateTime rangeEnd)
        {
            return dt.Ticks >= rangeBeg.Ticks && dt.Ticks <= rangeEnd.Ticks;
        }

        public static bool IsWorkingDay(this DateTime date)
        {
            return date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
        }

        public static bool IsWeekend(this DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }
    }
}