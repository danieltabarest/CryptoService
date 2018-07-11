using System.Xml;

namespace NSU.Utilities.Extensions
{
    public static class xXMLNode
    {
        /// <summary>
        /// Determines whether [contains attribute and is not null] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns></returns>
        public static bool ContainsAttributeAndIsNotNull(this XmlNode value, string attributeName)
        {
            //Make sure that the column and its value is not null
            if (value != null && value.Attributes != null && value.Attributes.Count > 0 && value.Attributes.GetNamedItem(attributeName) != null && !value.Attributes.GetNamedItem(attributeName).Value.IsNullEmpty())
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
