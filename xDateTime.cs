using System;
using System.Threading;

namespace NSU.Utilities.Extensions
{

    /// <summary>
    /// Contains extensions for the DateTime Object
    /// </summary>
    public static class xDateTime
    {

        #region "To"

        /// <summary>
        /// method for converting a System.DateTime value to a UNIX Timestamp
        /// </summary>
        /// <param name="value">date to convert</param>
        /// <returns></returns>
        public static double ToUnixTimestamp(this DateTime value)
        {

            //create Timespan by subtracting the value provided from
            //the Unix Epoch

            TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());

            //return the total seconds (which is a UNIX timestamp)
            return Convert.ToDouble(span.TotalSeconds);

        }

        /// <summary>
        /// Creates the epoch timestamp.
        /// </summary>
        /// <param name="value">The value.</param><returns></returns>
        public static double ToEpochTimestamp(this DateTime value)
        {

            TimeSpan ts = (value.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            return ts.TotalMilliseconds;

        }

        /// <summary>
        /// Converts datetime to a string formatted as follows:  YYYYMMDD.
        /// </summary>
        /// <param name="value">The value.</param><returns></returns>
        public static string ToYYYYMMDD(this DateTime value)
        {

            return value.ToString("yyyy") + value.ToString("MM") + value.ToString("dd");

        }

        /// <summary>
        /// Converts datetime to a string formatted as follows:  YYMMDD.
        /// </summary>
        /// <param name="value">The value.</param><returns></returns>
        public static string ToYYMMDD(this DateTime value)
        {

            return value.ToString("yy") + value.ToString("MM") + value.ToString("dd");

        }

        #endregion

        /// <summary>
        /// Firsts the day of week date.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static DateTime FirstDayOfWeekDate(this DateTime obj)
        {
            System.Globalization.CultureInfo info = Thread.CurrentThread.CurrentCulture;

            DayOfWeek firstday = info.DateTimeFormat.FirstDayOfWeek;
            DayOfWeek today = info.Calendar.GetDayOfWeek(DateTime.Now);

            int diff = today - firstday;
            obj = DateTime.Now.AddDays(-diff);

            return obj;
        }

        /// <summary>
        /// Finds the next available day of the week date
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="dow">The dow.</param>
        /// <returns></returns>
        public static DateTime ToNextAvailableDayOfWeek(this DateTime obj, DayOfWeek dow)
        {
            // loop through a week to get the next avaialbe
            for (int i = 0;i < 7; i++)
            {
                if (obj.AddDays(i).DayOfWeek == dow)
                    return obj.AddDays(i);
            }

            return obj;
        }

        #region "Is"

        /// <summary>
        /// Checks to see if the date and time passed in is between the start and end date
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        public static bool IsBetween(this DateTime value, DateTime startDate, DateTime endDate)
        {

            return (value >= startDate && value <= endDate);

        }

        #endregion

    }

}

