using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;

namespace NSU.Utilities.Extensions
{

    /// <summary>
    /// Contains extensions for the DataTable Object
    /// </summary>
    public static class xDataTable
    {

        /// <summary>
        /// Determines whether the specified data Table is empty.
        /// </summary>
        /// <param name="value">The data table.</param>
        /// <returns></returns>
        public static bool IsEmpty(this DataTable value)
        {

            if (value == null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Determines whether the specified data Table is empty.
        /// </summary>
        /// <param name="value">The data table.</param>
        /// <returns></returns>
        public static bool HasData(this DataTable value)
        {

            if (!value.IsEmpty() && value.Rows.Count > 0)
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
        public static int GetRowCount(this DataTable value)
        {
            int rowCount = 0;

            // make sure a dataset exists
            if (!value.IsEmpty())
            {
                rowCount = value.Rows.Count;
            }

            return rowCount;

        }

        /// <summary>
        /// Gets the first value for a specified column
        /// </summary>
        /// <param name="value">The DataTable.</param>
        /// <param name="columnName">Name of the column.</param><returns></returns>
        public static string GetValue(this DataTable value, string columnName)
        {

            string strReturn = string.Empty;

            // make sure a column was passed in
            if (columnName.Trim().Length > 0)
            {
                if (value.GetRowCount() > 0)
                {
                    strReturn = value.Rows[0][value.Columns[columnName].Ordinal].ToString();
                }
            }

            return strReturn;

        }

        /// <summary>
        /// Gets the distinct data.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="rowFilter">The row filter.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="dataColumns">The data columns.</param><returns></returns>
        public static DataSet GetDistinctData(this DataTable value, string rowFilter, string sortOrder, params string[] dataColumns)
        {

            DataSet ds;

            if (value.DataSet == null)
            {
                ds = new DataSet();
                ds.Tables.Add(value);
            }
            else
            {
                ds = value.DataSet;
            }

            return ds.GetDistinctData(rowFilter, sortOrder, dataColumns);

        }

        /// <summary>
        /// Saves the item within a Hatable into a data Table
        /// </summary>
        /// <param name="tbl">The TBL.</param>
        /// <param name="insert">if set to <c>true</c> [insert].</param>
        /// <param name="dt">The dt.</param>
        /// <param name="primaryKeyName">Name of the primary key.</param><returns></returns>
        public static bool SaveHashTable(this DataTable dt, Hashtable tbl, bool insert, string primaryKeyName)
        {
            bool success = false;

            if (dt != null)
            {

                if (insert)
                {
                    DataRow newRow = dt.NewRow();

                    if (!string.IsNullOrEmpty(primaryKeyName))
                    {
                        int id = (dt.Rows.Count + 1) * -1;
                        newRow[primaryKeyName] = id;
                    }

                    success = SaveHashTable(tbl, insert, ref newRow);

                    dt.Rows.Add(newRow);

                }

            }

            return success;

        }

        /// <summary>
        /// Saves the data.
        /// </summary>
        /// <param name="tbl">The TBL.</param>
        /// <param name="insert">if set to <c>true</c> [insert].</param>
        /// <param name="dr">The dr.</param><returns></returns>
        private static bool SaveHashTable(Hashtable tbl, bool insert, ref DataRow dr)
        {
            bool success;

            // begin the edit if we are in edit mode, not insert
            if (!insert)
            {
                dr.BeginEdit();
            }

            // loop through each item in the hash table and edit or insert
            // into data table
            foreach (DictionaryEntry myDe in tbl)
            {
                if (dr.Table.Columns.Contains(myDe.Key.ToString()) && myDe.Value != null && !dr.Table.Columns[myDe.Key.ToString()].ReadOnly)
                {
                    dr[myDe.Key.ToString()] = myDe.Value.ToString();
                }
            }

            // begin the edit if we are in edit mode, not insert
            if (!insert)
            {
                dr.EndEdit();
            }

            success = true;

            return success;
        }

        /// <summary>
        /// Toes the CSV.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="includeHeader">if set to <c>true</c> [include header].</param>
        public static void ToCSV(this DataTable table, string delimiter, bool includeHeader)        
        {             
            StringBuilder result = new StringBuilder();                          
            if (includeHeader)            
            {                 
                foreach (DataColumn column in table.Columns)                
                {                     
                    result.Append(column.ColumnName);                     
                    result.Append(delimiter);                 
                }                   
                result.Remove(--result.Length, 0);                 
                result.Append(Environment.NewLine);             
            }               
            foreach (DataRow row in table.Rows)            
            {                 
                foreach (object item in row.ItemArray)                
                {                     
                    if (item is DBNull)                         
                        result.Append(delimiter);                     
                    else                    
                    {                         
                        string itemAsString = item.ToString();                         
                        // Double up all embedded double quotes                         
                        itemAsString = itemAsString.Replace("\"", "\"\"");                           
                        // To keep things simple, always delimit with double-quotes                        
                        // so we don't have to determine in which cases they're necessary                         
                        // and which cases they're not.                         
                        itemAsString = "\"" + itemAsString + "\"";                           
                        result.Append(itemAsString + delimiter);                     
                    }                 
                }                   
                result.Remove(--result.Length, 0);                 
                result.Append(Environment.NewLine);             
            }              
            using (StreamWriter writer = new StreamWriter(@"C:\log.csv", true))            
            {                
                writer.Write(result.ToString());                             
            }                        
        
        }

        /// <summary>
        /// Sets the web runtime cache.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <param name="key">The key.</param>
        /// <param name="minutesExpiration">The minutes expiration.</param>
        /// <returns></returns>
        public static bool SetWebRuntimeCache(this DataTable val, string key, double minutesExpiration)
        {
            try
            {
                if (!val.IsEmpty())
                {
                    // make sure that the object does not already exist
                    object obj = key.GetWebRuntimeCache();
                    if (obj != null)
                        System.Web.HttpRuntime.Cache.Remove(key);

                    System.Web.HttpRuntime.Cache.Insert(key, val, null, DateTime.Now.AddMinutes(minutesExpiration), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
                }

            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

    }

}
