
using System;
using System.Data;

namespace NSU.Utilities.Extensions
{

    /// <summary>
    /// Contains extensions for the DataSet Object
    /// </summary>
    public static class xDataSet
    {

        /// <summary>
        /// Determines whether the specified dataset is empty.
        /// </summary>
        /// <param name="value">The dataSet.</param>
        /// <returns></returns>
        public static bool IsEmpty(this DataSet value)
        {

            if (value == null || value.Tables.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Determines whether the specified dataset has data.
        /// </summary>
        /// <param name="value">The dataSet.</param>
        /// <returns></returns>
        public static bool HasData(this DataSet value)
        {

            if (!value.IsEmpty() && value.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Gets the row count.
        /// </summary>
        /// <param name="value">The value.</param><returns></returns>
        public static int GetRowCount(this DataSet value)
        {
            int rowCount = 0;

            if (!value.IsEmpty())
            {
                rowCount = value.Tables[0].Rows.Count;
            }

            return rowCount;

        }

        /// <summary>
        /// Gets the first value for a specified column
        /// </summary>
        /// <param name="value">The dataSet.</param>
        /// <param name="columnName">Name of the column.</param><returns></returns>
        public static string GetValue(this DataSet value, string columnName)
        {

            string strReturn = string.Empty;

            // make sure a column was passed in
            if (columnName.Trim().Length > 0 && !value.IsEmpty())
            {
                if (value.GetRowCount() > 0)
                {
                    strReturn = value.Tables[0].Rows[0][value.Tables[0].Columns[columnName].Ordinal].ToString();
                }
            }

            return strReturn;

        }

        /// <summary>
        /// Gets the distinct data for a given dataset
        /// </summary>
        /// <param name="value">The ds.</param>
        /// <param name="rowFilter">The row filter.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="dataColumns">The data columns.</param><returns></returns>
        public static DataSet GetDistinctData(this DataSet value, string rowFilter, string sortOrder, params string[] dataColumns)
        {

            DataSet dsNew = new DataSet();


            if (!value.IsEmpty())
            {
                DataView dv = value.Tables[0].DefaultView;

                if (!string.IsNullOrEmpty(rowFilter))
                {
                    dv.RowFilter = rowFilter.Trim();
                }

                if (!string.IsNullOrEmpty(sortOrder))
                {
                    dv.Sort = sortOrder;
                }

                DataTable dtCt = dv.ToTable(true, dataColumns);
                dsNew.Tables.Add(dtCt);

            }

            return dsNew;

        }

        /// <summary>
        /// Sorts the data.
        /// </summary>
        /// <param name="value">The DataSet.</param>
        /// <param name="sortOrder">The sort order.</param><returns></returns>
        public static DataSet SortData(this DataSet value, string sortOrder)
        {
            DataSet dsNew = new DataSet();

            // make sure a dataset exists
            if (value.GetRowCount() > 0)
            {
                DataView dv = value.Tables[0].DefaultView;

                if (sortOrder != null)
                {
                    dv.Sort = sortOrder;
                }

                DataTable dtCt = dv.ToTable();
                dsNew.Tables.Add(dtCt);
            }

            return dsNew;

        }
        
        /// <summary>
        /// Sets the web runtime cache.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <param name="key">The key.</param>
        /// <param name="minutesExpiration">The minutes expiration.</param>
        /// <returns></returns>
        public static bool SetWebRuntimeCache(this DataSet val, string key, double minutesExpiration)
        {
            if (!val.IsEmpty())
            {
                // make sure that the object does not already exist
                object obj = key.GetWebRuntimeCache();
                if (obj != null)
                    System.Web.HttpRuntime.Cache.Remove(key);

                System.Web.HttpRuntime.Cache.Insert(key, val, null, DateTime.Now.AddMinutes(minutesExpiration), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
            }

            return true;
        }


    }

}
