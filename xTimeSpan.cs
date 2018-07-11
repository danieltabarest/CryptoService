using System;

namespace NSU.Utilities.Extensions
{
    /// <summary>
    /// Contains extensions for the Timespan Object
    /// </summary>
    public static class xTimeSpan
    {

        /// <summary>
        /// Convert timespan to a date time.
        /// </summary>
        /// <param name="param">The param.</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this TimeSpan param)
        {
            //return the result of the operation
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddTicks(param.Ticks);
        }

    }
}
