using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class DateTimeExtension
    {
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
    }
}
