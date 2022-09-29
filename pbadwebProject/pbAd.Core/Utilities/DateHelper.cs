
#region Using

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#endregion

namespace pbAd.Core.Utilities
{
    public static class DateHelper
    {
        /// <summary>
        /// Converts to nullable short date string
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToNullableShortDateString(this DateTime? date)
        {
            return date.HasValue ? DateTime.Parse(date.ToString()).ToShortDateString() : string.Empty;
        }
        public static string ToNullableDateString(this DateTime? date)
        {
            return date.HasValue ? DateTime.Parse(date.ToString()).ToString() : string.Empty;
        }
        /// <summary>
        /// Converts date to nullable short date string
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToNullableLongDateString(this DateTime? date)
        {
            return date.HasValue ? DateTime.Parse(date.ToString()).ToLongDateString() : string.Empty;
        }

        /// <summary>
        /// Converts date to nullable short date string
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToNullableShortDateStringOrNull(this DateTime? date)
        {
            return date.HasValue ? DateTime.Parse(date.ToString()).ToShortDateString() : null;
        }

        /// <summary>
        ///     Get the list of months as numbers
        /// </summary>
        /// <returns></returns>
        public static List<string> GetMonthsListAsNumbers()
        {
            var today = DateTime.Today;
            var beforeMonths = Enumerable.Range(1, 12)
                //.Where(i => i <= today.Month)
                .Select(i => new DateTime(today.Year, i, 1).Month.ToString(CultureInfo.InvariantCulture))
                .ToList();
            return beforeMonths;
        }

        /// <summary>
        ///     Get the list of months as numbers
        /// </summary>
        /// <returns></returns>
        public static List<string> GetMonthsList()
        {
            var today = DateTime.Today;
            var beforeMonths = Enumerable.Range(1, 12)
                .Where(i => i <= today.Month)
                .Select(i => new DateTime(today.Year, i, 1).ToString("MMMM"))
                .ToList();
            return beforeMonths;
        }

        /// <summary>
        ///     Gets the first day of current month.
        /// </summary>
        /// <returns></returns>
        public static DateTime GetFirstDayOfCurrentMonth()
        {
            var currentDate = DateHelper.GetCurrentDateTime();
            return new DateTime(currentDate.Year, currentDate.Month, 1);
        }

        /// <summary>
        ///     Gets the last day of current month.
        /// </summary>
        /// <returns></returns>
        public static DateTime GetLastDayOfCurrentMonth()
        {
            var dtTo = DateHelper.GetCurrentDateTime();
            dtTo = dtTo.AddMonths(1);
            return dtTo.AddDays(-dtTo.Day);
        }

        /// <summary>
        ///     Gets the month name based on number.
        /// </summary>
        /// <param name="monthNumber"></param>
        /// <returns></returns>
        public static string GetMonthNameByNumber(int monthNumber)
        {
            // dirty way
            var currentDate = DateHelper.GetCurrentDateTime();
            var temDate = new DateTime(currentDate.Year, monthNumber, currentDate.Day);
            return temDate.ToString("MMMM");
        }

        /// <summary>
        /// Get current datetime by timezone, if conversion fails, it'll return the currrent datetime
        /// </summary>
        /// <param name="timeZoneId">default timezone is "Eastern Standard Time"</param>
        /// <returns></returns>
        public static DateTime GetCurrentDateTime(string timeZoneId= "Eastern Standard Time")
        {
            var currentDate = DateTime.Now;
            try
            {
                var est = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

                return TimeZoneInfo.ConvertTime(currentDate, est);
            }
            catch (Exception)
            {
                //if conversion fails, let's return current datetime
                return currentDate;
            }
        }

        /// <summary>
        /// Get Null able int
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int? ToNullableInt(this string s)
        {
            int i;
            if (int.TryParse(s, out i)) return i;
            return null;
        }
    }
}