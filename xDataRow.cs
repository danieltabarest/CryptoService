using System.Data;

namespace NSU.Utilities.Extensions
{

    /// <summary>
    /// Contains extensions for the DataRow Object
    /// </summary>
    public static class xDataRow
    {

        /// <summary>
        /// Determines whether [contains column and is not null] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="columnName">Name of the column.</param><returns></returns>
        public static bool ContainsColumnAndIsNotNull(this DataRow value, string columnName)
        {

            //Make sure that the column and its value is not null
            if (value != null && value.Table.Columns.Contains(columnName) && !System.Convert.IsDBNull(value[columnName]))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }

}
