using System;

namespace Urd.Utils
{
    public static class TimeSpanExtension
    {
        public static string ToShortTimeString(this TimeSpan time)
        {
            if (time.TotalDays > 1)
            {
                return time.ToString(@"dd\d\ hh\h");
            }

            if (time.TotalHours > 1)
            {
                return time.ToString(@"hh\h\ mm\m");
            }

            if (time.TotalMinutes > 1)
            {
                return time.ToString(@"mm\m\ ss\s");
            }
            
            return time.ToString(@"ss\s");
        }
    }
}


