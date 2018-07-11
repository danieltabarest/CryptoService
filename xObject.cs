using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Caching;

namespace NSU.Utilities.Extensions
{
	/// <summary>
	/// Contains extensions for Objects
	/// </summary>
	public static class xObject
	{

		#region General Code

		#region Conversion

		/// <summary>
		/// Converts to date time.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <returns></returns>
		public static DateTime ToDateTime(this object val)
		{

			if (val != null)
			{
			    DateTime dt;

                DateTime.TryParse(val.ToString(), out dt);

                return dt;
			}
			else
			{
				return DateTime.MinValue;
			}
		}

		/// <summary>
		/// Converts to time span.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <returns></returns>
		public static TimeSpan ToTimeSpan(this object val)
		{

			if (val != null)
			{
			    TimeSpan ts;
				TimeSpan.TryParse(val.ToString(), out ts);
				return ts;
			}
			else
			{
				return TimeSpan.MinValue;
			}
		}

		/// <summary>
		/// Converts to boolean.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <returns></returns>
		public static bool ToBoolean(this object val)
		{

			if (val != null && !val.ToTryString().ToLower().IsIn("yes", "no"))
			{
			    bool bol;
				bool.TryParse(val.ToString(), out bol);
				return bol;
			}
			else
			{
                if (val != null && val.ToString().ToLower() == "yes")
                {
                    return true;
                }
                else
                {
                    return false;
                }
			}
		}

		/// <summary>
		/// Converts to integer.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <returns></returns>
		public static int ToInteger(this object val)
		{
			if (val != null)
			{
				int i;

				int.TryParse(val.ToString(), out i);

				return i;
			}
			else
			{
				return 0;
			}
		}

		/// <summary>
		/// Converts to nullable int.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <returns></returns>
		public static int? ToNullableInt(this object val)
		{
			if (val != null)
			{
				int i;

				int.TryParse(val.ToString(), out i);

				return i;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Converts to string.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <returns></returns>
		public static string ToTryString(this object val)
		{
			if (val != null)
				return val.RemoveHTMLSpace().ToString().Trim();
			else
				return string.Empty;
		}

		/// <summary>
		/// Converts to decimal.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <returns></returns>
		public static decimal ToDecimal(this object val)
		{
			if (val != null)
			{
				decimal i;

				decimal.TryParse(val.ToString(), out i);

				return i;
			}
			else
			{
				return 0;
			}
		}

		/// <summary>
		/// Converts to number.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <returns></returns>
		public static string ToNumber(this object val)
		{
			if (val != null)
			{
				return string.Join(null, System.Text.RegularExpressions.Regex.Split(val.ToString(), "[^\\d+]"));
			}
			else
			{
				return string.Empty;
			}
		}

		/// <summary>
		/// Removes the HTML space.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <returns></returns>
		public static object RemoveHTMLSpace(this object val)
		{
			if (val != null && val.ToString().Trim() == "&nbsp;")
				return string.Empty;
			else
				return val;
		}

		#endregion

		#region "Is"

		/// <summary>
		/// Determines whether the specified value is in.
		/// </summary>
		/// <param name="obj">The obj.</param>
		/// <param name="values">The values.</param><returns>
		///   <c>true</c> if the specified obj is in; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsIn(this object obj, params object[] values)
		{

			foreach (object val in values)
			{
				if (val.Equals(obj))
					return true;
			}

			return false;

		}

		/// <summary>
		/// Determines whether the specified value is numeric.
		/// </summary>
		/// <param name="value">The value.</param><returns>
		///   <c>true</c> if the specified value is numeric; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsNumeric(this object value)
		{
			if (!value.ToTryString().IsNullEmpty())
			{
				return System.Text.RegularExpressions.Regex.IsMatch(value.ToTryString(), "[^0-9]");
			}
			else
			{
				return false;
			}

		}

		/// <summary>
		/// Determines whether [is entity empty] [the specified obj].
		/// </summary>
		/// <param name="obj">The obj.</param>
		/// <remarks>Function will check to see if the entity has an ID Property in the check if there is one</remarks>
		/// <returns>
		///   <c>true</c> if [is entity empty] [the specified obj]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsEntityEmpty(this object obj)
		{

			//make sure the object has something

			if (obj == null)
			{
				return true;

			}
			else
			{
				//if the object contains a property called ID

				if (obj.HasProperty("ID"))
				{
					object itm = obj.GetPropertyValue("ID");

					//If the property is numeric then make sure that there is an ID
					if (itm.IsNumeric() && itm.ToInteger() <= 0)
					{
						return true;

					}
					else
					{
						//if the ID property is no numeric then check its contents
						if (itm.ToString().Length == 0)
						{
							return true;
						}
						else
						{
							return false;
						}
					}
				}
				else
				{
					return false;
				}

			}

		}

        #endregion

        #region "Encryption"

        /// <summary>
        /// Decrypts the specified obj.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="eKey">The e key.</param>
        /// <param name="eVec">The e vec.</param>
        /// <returns></returns>
        public static string Decrypt(this object obj, string eKey, string eVec)
		{


			if (!eKey.IsNullEmpty() && !eVec.IsNullEmpty() && eKey.Length >= 8 && eVec.Length >= 8)
			{
				Encoding encoding = Encoding.UTF8;

				MemoryStream ms = new MemoryStream();


				obj = obj.ToTryString().Replace(" ", "+");

				byte[] inputByteArray = new byte[obj.ToTryString().Length + 1];

				byte[] key = Encoding.UTF8.GetBytes(eKey.Substring(0, 8));

				byte[] iv = Encoding.UTF8.GetBytes(eVec.Substring(0, 8));

				DESCryptoServiceProvider des = new DESCryptoServiceProvider();

				inputByteArray = Convert.FromBase64String(obj.ToTryString());

				CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, iv), CryptoStreamMode.Write);

				cs.Write(inputByteArray, 0, inputByteArray.Length);

				cs.FlushFinalBlock();

				return encoding.GetString(ms.ToArray());

			}
			else
			{
				return string.Empty;

			}

		}

        /// <summary>
        /// Encrypts the specified obj.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="eKey">The e key.</param>
        /// <param name="eVec">The e vec.</param>
        /// <returns></returns>
        public static string Encrypt(this object obj, string eKey, string eVec)
		{

			if (!eKey.IsNullEmpty() && !eVec.IsNullEmpty() && eKey.Length >= 8 && eVec.Length >= 8)
			{
				MemoryStream ms = new MemoryStream();

				byte[] key = Encoding.UTF8.GetBytes(eKey.Substring(0, 8));

				byte[] iv = Encoding.UTF8.GetBytes(eVec.Substring(0, 8));

				DESCryptoServiceProvider des = new DESCryptoServiceProvider();

				byte[] inputByteArray = Encoding.UTF8.GetBytes(obj.ToTryString());

				CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, iv), CryptoStreamMode.Write);

				cs.Write(inputByteArray, 0, inputByteArray.Length);

				cs.FlushFinalBlock();

				return Convert.ToBase64String(ms.ToArray());


			}
			else
			{
				return string.Empty;

			}


		}

		#endregion

		#region "Cache"

		/// <summary>
		/// Gets the object from Cache if it exists
		/// </summary>
		/// <param name="obj">The obj.</param>
		/// <param name="cacheName">Name of the cache.</param>
		/// <param name="rebuild">if set to <c>true</c> will [rebuild] and store the object again into cache</param>
		/// <returns>Will return back the object that was passed in regardless if the item is to be cached or not. Please note in order to get the object cached the object name must appear within the server settings.</returns>
		public static object GetCacheItem(this object obj, string cacheName, bool rebuild = false)
			{

				Cache ch = new Cache();
				object objItem = ch.Get(cacheName);

				//If the item is not to be cached then remove from cache
				if (objItem != null || rebuild)
				{
					ch.Remove(cacheName);
					objItem = null;
				}

				return objItem;

			}


		/// <summary>
		/// Sets the object into cache.
		/// </summary>
		/// <param name="obj">The obj.</param>
		/// <param name="cacheName">Name of the cache.</param>
		/// <param name="cacheMinutes">How many minutes before the cached item expires.</param>
		/// <returns></returns>
		public static object SetCacheItem(this object obj, string cacheName, double cacheMinutes = 30)
			{

				//Create an instance of the cache class
				Cache ch = new Cache();


				if (obj != null)
				{
					//Make sure the item is not already in cache
					if (cacheName.IsCached())
						ch.Remove(cacheName);

					//Save the object in cache
					ch.Add(cacheName, obj, null, DateTime.Now.AddMinutes(cacheMinutes), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);

				}

				return obj;

			}

		#endregion

		#region "Reflection"

		/// <summary>
		/// Determines whether the specified obj has method.
		/// </summary>
		/// <param name="obj">The obj.</param>
		/// <param name="methodName">Name of the method.</param><returns>
		///   <c>true</c> if the specified obj has method; otherwise, <c>false</c>.
		/// </returns>
		public static bool HasMethod(this object obj, string methodName)
		{
			return (obj.GetType().GetMethod(methodName) != null);
		}

		/// <summary>
		/// Determines whether the specified obj has property.
		/// </summary>
		/// <param name="obj">The obj.</param>
		/// <param name="propertyName">Name of the property.</param><returns>
		///   <c>true</c> if the specified obj has property; otherwise, <c>false</c>.
		/// </returns>
		public static bool HasProperty(this object obj, string propertyName)
		{
			return (obj.GetType().GetProperty(propertyName) != null);
		}

		/// <summary>
		/// Gets the property value.
		/// </summary>
		/// <param name="obj">The obj.</param>
		/// <param name="propertyName">Name of the property.</param><returns></returns>
		public static object GetPropertyValue(this object obj, string propertyName)
		{
			var propertyInfo = obj.GetType().GetProperty(propertyName);
			if (propertyInfo != null)
			    return propertyInfo.GetValue(obj, null);
			else
			    return null;
		}

		#endregion

		#region Web Runtime Cache
		/// <summary>
		/// Sets the web runtime cache.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <param name="key">The key.</param>
		/// <param name="minutesExpiration">The minutes expiration.</param>
		/// <returns></returns>
		public static bool SetWebRuntimeCache(this object val, string key, double minutesExpiration = 30)
		{
			try
			{
				if (val != null)
				{
					// make sure that the object does not already exist
					object obj = key.GetWebRuntimeCache();
					if (obj != null)
						System.Web.HttpRuntime.Cache.Remove(key);

					System.Web.HttpRuntime.Cache.Insert(key, val, null, DateTime.Now.AddMinutes(minutesExpiration), TimeSpan.Zero, CacheItemPriority.Normal, null);
				}

			}
			catch (Exception ex)
			{
				return false;
			}

			return true;
		}
		#endregion

		#endregion

	}
}
