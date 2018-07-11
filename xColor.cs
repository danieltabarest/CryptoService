using System.Drawing;

namespace NSU.Utilities.Extensions
{
    /// <summary>
    /// Contains extensions for the Color Object
    /// </summary>
    public static class xColor
    {

        /// <summary>
        /// Toes the HTML.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ToHtml(this Color value)
        {
            return ColorTranslator.ToHtml(value);
        }

    }
}
