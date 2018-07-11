using System;

namespace NSU.Utilities.Extensions
{

    /// <summary>
    /// Contains extensions for the Integer Object
    /// </summary>
    public static class xInteger
    {

        /// <summary>
        /// Nums to enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param><returns></returns>
        public static T ToEnum<T>(this int value)
        {

            return (T)Enum.ToObject(typeof(T), value);

        }

        /// <summary>
        /// Determines whether the specified value is in.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="values">The values to compare.</param><returns>
        ///   <c>true</c> if the specified obj is in; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsIn(this int value, params object[] values)
        {

            foreach (object val in values)
            {
                if (val.Equals(value))
                    return true;
            }

            return false;

        }

        /// <summary>
        /// Converts the decimal number to it's Roman Numeral equivalent.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Value must be between 1 and 3999
        /// or
        /// Value must be between 1 and 3999
        /// </exception>
        public static string ToRoman(this int number)
        {

            if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("Value must be between 1 and 3999");
            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + ToRoman(number - 1000);
            if (number >= 900) return "CM" + ToRoman(number - 900); //EDIT: i've typed 400 instead 900
            if (number >= 500) return "D" + ToRoman(number - 500);
            if (number >= 400) return "CD" + ToRoman(number - 400);
            if (number >= 100) return "C" + ToRoman(number - 100);
            if (number >= 90) return "XC" + ToRoman(number - 90);
            if (number >= 50) return "L" + ToRoman(number - 50);
            if (number >= 40) return "XL" + ToRoman(number - 40);
            if (number >= 10) return "X" + ToRoman(number - 10);
            if (number >= 9) return "IX" + ToRoman(number - 9);
            if (number >= 5) return "V" + ToRoman(number - 5);
            if (number >= 4) return "IV" + ToRoman(number - 4);
            if (number >= 1) return "I" + ToRoman(number - 1);
            throw new ArgumentOutOfRangeException("Value must be between 1 and 3999");

        }

        /// <summary>
        /// Determines whether the specified value is between.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="startNum">The start number.</param>
        /// <param name="endNum">The end number.</param><returns>
        ///   <c>true</c> if the specified value is between; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsBetween(this int value, int startNum, int endNum)
        {

            return (value >= startNum && value <= endNum);

        }


        /// <summary>
        /// Generates a random number.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="min">The min.</param>
        /// <param name="max">The max.</param><returns></returns>
        public static int ToRandomNumber(this int value, int min = 1, int max = int.MaxValue)
        {

            Random generator = new Random(DateTime.Now.Millisecond);
            return generator.Next(min, max);

        }

    }

}
