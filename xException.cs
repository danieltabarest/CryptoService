using System;

namespace NSU.Utilities.Extensions
{
    /// <summary>
    /// Contains extensions for the Exception Object
    /// </summary>
    public static class xException
    {
        /// <summary>
        /// Gets the most inner exception.
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <returns></returns>
        public static Exception GetMostInner(this Exception ex) 
        { 
            Exception actualInnerEx = ex; 
            while (actualInnerEx != null) 
            { 
                actualInnerEx = actualInnerEx.InnerException; 
                if (actualInnerEx != null)            
                    ex = actualInnerEx; 
            } 
            return ex; 
        }
    }
}
