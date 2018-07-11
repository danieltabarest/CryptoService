using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace NSU.Utilities.Extensions
{

    /// <summary>
    /// Contains extensions for the Enum Object
    /// </summary>
    public static class xEnum
    {

        /// <summary>
        /// Retrieve the description on the enum, e.g.
        /// Description("Pin is required!")
        /// PinRequired = 1
        /// Then when you pass in the enum, it will retrieve the description
        /// </summary>
        /// <param name="value">The Enumeration</param>
        /// <returns>A string representing the friendly name</returns>
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();

            MemberInfo[] memInfo = type.GetMember(value.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return value.ToString();

        }

        /// <summary>
        /// Gets the integer value of the enum
        /// </summary>
        /// <param name="value">The enum</param>
        /// <returns>The Underlying Value of the Enum</returns>
        public static int GetValue(this Enum value)
        {

            Type type = value.GetType();

            return Convert.ToInt32(Enum.Parse(type, Enum.GetName(type, value)));

        }

        /// <summary>
        /// Gets the Name of the enum
        /// </summary>
        /// <param name="value">The enum</param>
        /// <returns>The Name of the Enum</returns>
        public static string GetEnumName(this Enum value)
        {

            Type type = value.GetType();

            return Enum.GetName(type, value);

        }

        /// <summary>
        /// Converts List Of Enums to a Dictionary.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="useDescription">if set to <c>true</c> [use description].</param><returns></returns>
        public static Dictionary<int, string> ToDictionary(this Enum value, bool useDescription)
        {

            Type type = value.GetType();

            return Enum.GetValues(type).Cast<Enum>().ToDictionary(useDescription);

        }

        /// <summary>
        /// Converts List Of Enums to a Dictionary.
        /// </summary>
        /// <param name="enums">The enums.</param>
        /// <param name="useDescription">if set to <c>true</c> [use description].</param><returns></returns>
        public static Dictionary<int, string> ToDictionary(this IEnumerable<Enum> enums, bool useDescription)
        {

            Dictionary<int, string> dic = new Dictionary<int, string>();

            foreach (Enum item in enums)
            {
                string text = useDescription ? item.GetDescription() : item.GetEnumName();

                dic.Add(item.GetValue(), text);

            }

            return dic;

        }

    }

}
