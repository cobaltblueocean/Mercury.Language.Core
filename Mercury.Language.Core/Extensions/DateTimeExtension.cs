using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// The number of seconds in one day.
        /// </summary>
        public static long SECONDS_PER_DAY = 86400L;

        /// <summary>
        /// The number of days in one year (estimated as 365.25).
        /// </summary>
        //TODO change this to 365.2425 to be consistent with JSR-310
        public static double DAYS_PER_YEAR = 365.25;

        /// <summary>
        /// The number of milliseconds in one day.
        /// </summary>
        public static long MILLISECONDS_PER_DAY = SECONDS_PER_DAY * 1000;

        /// <summary>
        /// The number of seconds in one year.
        /// </summary>
        public static long SECONDS_PER_YEAR = (long)(SECONDS_PER_DAY * DAYS_PER_YEAR);

        /// <summary>
        /// The number of milliseconds in one year.
        /// </summary>
        public static long MILLISECONDS_PER_YEAR = SECONDS_PER_YEAR * 1000;

        /// <summary>
        /// The number of milliseconds in one month.
        /// </summary>
        public static long MILLISECONDS_PER_MONTH = MILLISECONDS_PER_YEAR / 12L;

        public static Boolean IsAfter(this DateTime now, DateTime target)
        {
            if (DateTime.Compare(now, target) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Boolean IsBefore(this DateTime now, DateTime target)
        {
            if (DateTime.Compare(now, target) < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Boolean IsAfter(this DateTime? now, DateTime target)
        {
            if (DateTime.Compare((DateTime)now, target) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Boolean IsBefore(this DateTime? now, DateTime target)
        {
            if (DateTime.Compare((DateTime)now, target) < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static DateTime? FirstNonNull(this DateTime? source, DateTime val)
        {
            if (source == null)
                source = val;

            return source;
        }

        public static DateTime AddNanos(this DateTime source, long nanos)
        {
            return source.AddTicks(nanos * 100);
        }

        public static DateTime AddNanos(this DateTime source, double nanos)
        {
            return AddNanos(source, Convert.ToInt64(nanos));
        }

        public static Int64 GetTime(this DateTime d)
        {
            Int64 retval = 0;
            var st = new DateTime(1970, 1, 1);
            TimeSpan t = (d - st);
            retval = (Int64)(t.TotalMilliseconds + 0.5);
            return retval;
        }

        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static long CurrentTimeMillis(this DateTime dt)
        {
            return (long)(dt.ToUniversalTime() - Jan1st1970).TotalMilliseconds;
        }

    }
}
