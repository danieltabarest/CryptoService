using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NSU.Utilities.Extensions
{

    /// <summary>
    /// Contains extensions for the Enumerable Object
    /// </summary>
    public static class xEnumerable
    {

        /// <summary>
        /// checks to see if the enumerable object is empty. Is nothing or does not have any elements (count = 0)
        /// </summary>
        /// <param name="value">Enumerable object</param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this IEnumerable<T> value)
        {

            if (value == null || !value.Any())
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Toes the comma separated text.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns></returns>
        public static string ToCommaSeparatedText<T>(this IEnumerable<T> enumerable)
        {
            return ToDelimitedText(enumerable, ",");
        }

        /// <summary>
        /// Toes the comma separated text.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="quote">The quote.</param>
        /// <returns></returns>
        public static string ToCommaSeparatedText<T>(this IEnumerable<T> enumerable, string quote)
        {
            return ToDelimitedText(enumerable, ",", quote);
        }

        /// <summary>
        /// Toes the delimited text.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns></returns>
        public static string ToDelimitedText<T>(this IEnumerable<T> enumerable, string delimiter)
        {
            return ToDelimitedText(enumerable, delimiter, "");
        }

        /// <summary>
        /// Toes the delimited text.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="quote">The quote.</param>
        /// <returns></returns>
        public static string ToDelimitedText<T>(this IEnumerable<T> enumerable, string delimiter, string quote)
        {
            if (enumerable == null) return "";
            StringBuilder delimitedText = new StringBuilder();
            foreach (T elem in enumerable)
            {
                if (delimitedText.Length > 0)
                    delimitedText.Append(delimiter);
                delimitedText.Append(quote);
                delimitedText.Append(elem.ToString());
                delimitedText.Append(quote);
            }
            return delimitedText.ToString();
        }

        /// <summary>
        /// Firsts the specified enumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="nFirstElements">The n first elements.</param>
        /// <returns></returns>
        public static IEnumerable<T> First<T>(this IEnumerable<T> enumerable, int nFirstElements)
        {
            List<T> result = new List<T>();
            for (int i = 0; i < Math.Min(enumerable.Count(), nFirstElements); i++)
            {
                result.Add(enumerable.ElementAt(i));
            }
            return result;
        }

        /// <summary>
        /// Firsts the specified enumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="nFirstElements">The n first elements.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static IEnumerable<T> First<T>(this IEnumerable<T> enumerable, int nFirstElements, Func<T, bool> predicate)
        {
            return enumerable.Where(predicate).First(nFirstElements);
        }

        /// <summary>
        /// Fors the each.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="action">The action.</param>
        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action) 
        { 
            foreach (var item in list) { action.Invoke(item); } 
        }

        //class Program { internal class Employee { public string Name { get; set; } public string Department { get; set; } public string Function { get; set; } public decimal Salary { get; set; } } static void Main(string[] args) { var l = new List<Employee>() { new Employee() { Name = "Fons", Department = "R&D", Function = "Trainer", Salary = 2000 }, new Employee() { Name = "Jim", Department = "R&D", Function = "Trainer", Salary = 3000 }, new Employee() { Name = "Ellen", Department = "Dev", Function = "Developer", Salary = 4000 }, new Employee() { Name = "Mike", Department = "Dev", Function = "Consultant", Salary = 5000 }, new Employee() { Name = "Jack", Department = "R&D", Function = "Developer", Salary = 6000 }, new Employee() { Name = "Demy", Department = "Dev", Function = "Consultant", Salary = 2000 } }; var result1 = l.Pivot(emp => emp.Department, emp2 => emp2.Function, lst => lst.Sum(emp => emp.Salary)); foreach (var row in result1) { Console.WriteLine(row.Key); foreach (var column in row.Value) { Console.WriteLine("  " + column.Key + "\t" + column.Value); } } Console.WriteLine("----"); var result2 = l.Pivot(emp => emp.Function, emp2 => emp2.Department, lst => lst.Count()); foreach (var row in result2) { Console.WriteLine(row.Key); foreach (var column in row.Value) { Console.WriteLine("  " + column.Key + "\t" + column.Value); } } Console.WriteLine("----"); } }

        /// <summary>
        /// Pivots the specified source.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TFirstKey">The type of the first key.</typeparam>
        /// <typeparam name="TSecondKey">The type of the second key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="firstKeySelector">The first key selector.</param>
        /// <param name="secondKeySelector">The second key selector.</param>
        /// <param name="aggregate">The aggregate.</param>
        /// <returns></returns>
        public static Dictionary<TFirstKey, Dictionary<TSecondKey, TValue>> Pivot<TSource, TFirstKey, TSecondKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TFirstKey> firstKeySelector, Func<TSource, TSecondKey> secondKeySelector, Func<IEnumerable<TSource>, TValue> aggregate) 
        { 
            var retVal = new Dictionary<TFirstKey, Dictionary<TSecondKey, TValue>>(); 
            var l = source.ToLookup(firstKeySelector); 
            foreach (var item in l) 
            { 
                var dict = new Dictionary<TSecondKey, TValue>(); 
                retVal.Add(item.Key, dict); 
                var subdict = item.ToLookup(secondKeySelector); 
                foreach (var subitem in subdict) 
                { 
                    dict.Add(subitem.Key, aggregate(subitem)); 
                } 
            } 
            return retVal; 
        }

        /// <summary>
        /// To data table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="varlist">The varlist.</param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> varlist)        
        {            
            DataTable dtReturn = new DataTable();             
            // column names             
            PropertyInfo[] oProps = null;             
            if (varlist == null) return dtReturn;             
            foreach (T rec in varlist)            
            {                
                // Use reflection to get property names, to create table, Only first time, others will follow                 
                if (oProps == null)                
                {                    
                    oProps = ((Type)rec.GetType()).GetProperties();                    
                    foreach (PropertyInfo pi in oProps)                    
                    {                        
                        Type colType = pi.PropertyType;                         
                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))                        
                        {                            
                            colType = colType.GetGenericArguments()[0];                        
                        }                         
                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));                    
                    }                
                }                 
                DataRow dr = dtReturn.NewRow();                 
                foreach (PropertyInfo pi in oProps)                
                {                    
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue                    
                        (rec, null);                
                }                 
                dtReturn.Rows.Add(dr);            
            }            
            return dtReturn;        
        }

        /// <summary>
        /// Wheres if.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="condition">if set to <c>true</c> [condition].</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, int, bool> predicate) 
        { 
            if (condition)        
                return source.Where(predicate); 
            else        
                return source; 
        }

        /// <summary>
        /// converts a list of numbers into a string of numbers list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="varlist">The varlist.</param>
        /// <param name="numberPropertyName">Name of the number property.</param>
        /// <remarks>Need to make sure that the list being sent is ordered by the number property</remarks>
        /// <returns></returns>
        public static string ToNumberList<T>(this IEnumerable<T> varlist, string numberPropertyName)
        {
            string lst = string.Empty;
            int prev = 0;
            int last = 0;
            var a = 0;

            // make sure that there is a list
            if (!varlist.IsEmpty())
            {
                lst = "[";
                foreach (T rec in varlist)
                {
                    a = rec.GetPropertyValue(numberPropertyName).ToInteger();
                    if (prev == 0)
                    {
                        lst += a.ToString();
                        last = a;
                    }
                    else
                    {
                        if (a != (prev + 1))
                        {

                            if (prev != last)
                                lst += "-" + prev.ToString() + "],[";
                            else
                                lst += "],[";

                            lst += a.ToString();
                            last = a;
                        }
                    }

                    prev = a;

                }

            }

            if (a != last)
                lst += "-" + a.ToString() + "]";
            else
                lst += "]";

            return lst;
        }

    }

}

