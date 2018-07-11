using System.Data;

namespace NSU.Utilities.Extensions
{

    /// <summary>
    /// Contains extensions for the DataView Object
    /// </summary>
    public static class xDataView
    {

        /// <summary>
        /// Will check to see if the Enumberable object is not null and contains items in its list
        /// </summary>
        /// <param name="dv">DataView</param>
        /// <returns></returns>
        public static bool IsEmpty(this DataView dv)
        {
            return (dv == null || dv.Count == 0);
        }

        /// <summary>
        /// Gets the first value for a specified column
        /// </summary>
        /// <param name="value">The DataView.</param>
        /// <param name="columnName">Name of the column.</param><returns></returns>
        public static string GetValue(this DataView value, string columnName)
        {

            string strReturn = string.Empty;

            // make sure a column was passed in
            if (columnName.Trim().Length > 0)
            {
                if (value.Count > 0)
                {
                    strReturn = value[0][columnName].ToString();
                }
            }

            return strReturn;

        }

        /// <summary>
        /// Sorts the data.
        /// </summary>
        /// <param name="dv">The dv.</param>
        /// <param name="sortOrder">The sort order.</param><returns></returns>
        public static DataSet SortData(this DataView dv, string sortOrder)
        {

            DataSet dsNew = new DataSet();

            // make sure a dataset exists
            if (dv.Count > 0)
            {
                if (sortOrder != null)
                {
                    dv.Sort = sortOrder;
                }

                DataTable dtCt = dv.ToTable();
                dsNew.Tables.Add(dtCt);

            }

            return dsNew;

        }

    }

}
