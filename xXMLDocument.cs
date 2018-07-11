using System;
using System.Web.Caching;
using System.Xml;

namespace NSU.Utilities.Extensions
{

    /// <summary>
    /// Contains extensions for the XMLDocument Object
    /// </summary>
    public static class xXMLDocument
    {
        #region Cache

        /// <summary>
        /// Load a new instatiated XML Document with the specified file name and path and if the item is to be saved in cache
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="cacheName">Key Name of the cached item.</param>
        /// <param name="fileName">The XML file to be loaded into cache.</param>
        /// <param name="cacheMinutes">How many minutes before the cached item expires.</param>
        /// <param name="rebuild">force a reload the item into cache.</param>
        /// <remarks>The XML Document will be loaded into cache if the cache Name is in the server configuration file under XMLDocument</remarks>
        /// <returns>Loaded file into XMLDocument </returns>
        public static XmlDocument LoadAndCache(this XmlDocument value, string cacheName, string fileName, double cacheMinutes = 30, bool rebuild = false)
        {

            //Include file as the cache Name
            Cache ch = new Cache();
            XmlDocument xmlItem = ch.Get(cacheName) as XmlDocument;

            //See if the item already is in cache
            if (xmlItem == null || rebuild)
            {
                xmlItem = new XmlDocument();

                //Make sure that the file exists

                if (!fileName.IsNullEmpty() && System.IO.File.Exists(fileName))
                {
                    xmlItem.Load(fileName);

                    //Make sure the item is not already in cache
                    if (cacheName.IsCached())
                        ch.Remove(cacheName);

                    //Check to see if the item is to be cached
                    ch.Add(cacheName, xmlItem, null, DateTime.Now.AddMinutes(cacheMinutes), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
                }

            }

            return xmlItem;

        }

        #endregion
    }

}
