using System;
using System.Globalization;

namespace Services.Helper
{
    public static partial class DateTimeExtensions
    {
        public static DateTime FirstDayOfWeek(this DateTime dt)
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            var diff = dt.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;

            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-diff).Date;
        }

        public static DateTime LastDayOfWeek(this DateTime dt) =>
            dt.FirstDayOfWeek().AddDays(6);

        public static DateTime FirstDayOfMonth(this DateTime dt) =>
            new DateTime(dt.Year, dt.Month, 1);

        public static DateTime LastDayOfMonth(this DateTime dt) =>
            dt.FirstDayOfMonth().AddMonths(1).AddDays(-1);

        public static DateTime FirstDayOfNextMonth(this DateTime dt) =>
            dt.FirstDayOfMonth().AddMonths(1);

        public static DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            // Use first Thursday in January to get first week of the year as
            // it will never be in Week 52/53
            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            // As we're adding days to a date in Week 1,
            // we need to subtract 1 in order to get the right date for week #1
            if (firstWeek == 1)
            {
                weekNum -= 1;
            }

            // Using the first Thursday as starting week ensures that we are starting in the right year
            // then we add number of weeks multiplied with days
            var result = firstThursday.AddDays(weekNum * 7);

            // Subtract 3 days from Thursday to get Monday, which is the first weekday in ISO8601
            return result.AddDays(-3);
        }

        public static DateTime FirstDateOfWeek(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);

            int daysOffset = (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;

            DateTime firstMonday = jan1.AddDays(daysOffset);

            int firstWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(jan1, CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule, CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);

            if (firstWeek <= 1)
            {
                weekOfYear -= 1;
            }

            return firstMonday.AddDays(weekOfYear * 7);
        }

        public static DateTime GetLastDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
        }

        public static TimeSpan GetTimeSpan(this DateTime? value)
        {
            return (value ?? Convert.ToDateTime("00:00:00")).TimeOfDay;
        }

        public static DateTime GetNgayTao(this DateTime? value)
        {
            return value ?? DateTime.Parse("01/01/0001");
        }

        public static DateTime? ConvertStringToDate(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            try
            {
                return DateTime.Parse(value);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static DateTime ParseStringToDate(this string dt)
        {
            if (!string.IsNullOrEmpty(dt))
            {
                if (dt.Contains("."))
                {
                    dt = dt.Split(".")[0];
                }

                var Dateformats = new[] {
                "M/d/yyyy hh:mm:ss tt",
                "M/dd/yyyy hh:mm:ss tt",
                "M/d/yyyy hh:mm:ss",
                "M/dd/yyyy hh:mm:ss",
                "MM/d/yyyy hh:mm:ss",
                "MM/dd/yyyy hh:mm:ss",
                "MM/dd/yyyy hh:mm:ss tt",
                "yyyy-MM-dd hh:mm:ss",
                "yyyy-MM-dd" };

                DateTime.TryParseExact(dt, Dateformats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDateThas);
                return parsedDateThas;
            }
            else
            {
                return DateTime.Parse("0001-01-01");
            }

        }

        /// <summary>
        /// truncate miliseconds off of a datetime
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime TruncateMiliseconds(this DateTime dt)
        {
            var result = new DateTime(dt.Ticks - (dt.Ticks % TimeSpan.TicksPerSecond), dt.Kind);
            return result;
        }
    }
}
