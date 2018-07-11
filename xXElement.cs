using System.Xml.Linq;

namespace NSU.Utilities.Extensions
{
    public static class xXElement
    {
        /// <summary>
        /// Determines whether [contains attribute and is not null] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns></returns>
        public static bool ContainsAttributeAndIsNotNull(this XElement value, string attributeName)
        {
            //Make sure that the column and its value is not null
            try 
	            {	        
                    if (value != null && value.HasAttributes && string.IsNullOrEmpty(attributeName) && value.Attribute(attributeName).Value != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
		
	            }
	            catch
	            {		
                        return false;
	            }

        }

    }
}
