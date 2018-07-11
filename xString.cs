using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Caching;
using System.Web.UI;
using System.Xml;
using System.Configuration;
using RestSharp;
using System.Threading.Tasks;

namespace NSU.Utilities.Extensions
{

	/// <summary>
	/// Contains extensions for the String Object
	/// </summary>
	public static class xString
	{

		#region "To"

		/// <summary>
		/// Reverses the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static string ToReverse(this string value)
		{

			var sb = new StringBuilder();


			if (!value.IsNullEmpty())
			{
				for (int i = value.Length; i >= 1; i += -1)
				{
					sb.Append(value.Substring(i, 1));
				}

			}

			return sb.ToString();

		}

	

		/// <summary>
		/// Converts a string to a safe string to send to SQL.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static string ToSafeSqlLiteral(this string value)
		{


			if (!value.IsNullEmpty())
			{
				return value.Replace("'", "").Replace("[", "[[]").Replace("%", "[%]").Replace("#", "").Replace("\\r", "").Replace("\\n", "");


			}
			else
			{
				return string.Empty;

			}

		}

		/// <summary>
		/// Converts a string to a phone.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static string ToPhone(this string value)
		{


			if (!value.IsNullEmpty())
			{
				// remove any non numerics from the phone
				value = value.ToNumber();

				// the format for the numbers to be displayed
				string formatPattern = "(\\d{3})(\\d{3})(\\d{4})";

				return Regex.Replace(value, formatPattern, "($1) $2-$3");


			}
			else
			{
				return string.Empty;

			}

		}

		/// <summary>
		/// Converts a string to a number, stripping any characters from the string.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static string ToNumber(this string value)
		{

			// function will only return only numbers from
			// a given string. if there are any alpha characters, they will be stripped.
			if (!value.IsNullEmpty())
			{
				return Regex.Replace(value, "[^0-9]", "");
			}
			else
			{
				return string.Empty;
			}

		}

		/// <summary>
		/// Converts a string to a date.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static Boolean ToBoolean(this string value)
		{

			var dt = false;

			bool bol = Boolean.TryParse(value, out dt);
			if (bol)
			{
				return dt;
			}
			else
			{
				if (!value.IsNullEmpty() && value.ToLower() == "yes")
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
		/// Converts a string to a date.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static DateTime ToDateTime(this string value)
		{

			DateTime dt = default(DateTime);

			bool bol = DateTime.TryParse(value, out dt);
			if (bol)
			{
				return dt;
			}
			else
			{
				return DateTime.Now;
			}

		}

		/// <summary>
		/// Converts a string to integer.
		/// </summary>
		/// <param name="value">The value.</param><returns></returns>
		public static int ToInteger(this string value)
		{

			int itm = 0;

			bool bol = int.TryParse(value, out itm);
			if (bol)
			{
				return itm;
			}
			else
			{
				return 0;
			}

		}

		/// <summary>
		/// Converts a string to double.
		/// </summary>
		/// <param name="value">The value.</param><returns></returns>
		public static double ToDouble(this string value)
		{

			double itm = 0;

			bool bol = double.TryParse(value, out itm);
			if (bol)
			{
				return itm;
			}
			else
			{
				return 0;
			}

		}

		/// <summary>
		/// Converts a string to integer.
		/// </summary>
		/// <param name="value">The value.</param><returns></returns>
		public static char ToChar(this string value)
		{

			if (!value.IsNullEmpty())
			{
				return Convert.ToChar(value);
			}
			else
			{
				return Convert.ToChar(string.Empty);
			}


		}

		/// <summary>
		/// Gets the masked credit card number.
		/// </summary>
		public static string ToMaskedCreditCardNumber(this string value)
		{

			//make sure that we have 4 dgitis in the expiration date

			if (!value.IsNullEmpty() && value.Length > 4)
			{
				return new String("*".ToChar(), value.Length - 4) + value.ToRight(4);


			}
			else
			{
				return string.Empty;

			}

		}

		/// <summary>
		/// Text to enum.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">The value.</param><returns></returns>
		public static T ToEnum<T>(this string value)
		{

			return (T)Enum.Parse(typeof(T), value);

		}

		/// <summary>
		/// Generates a the unique string id.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="maxSize">Maximum size of the string.</param>
		/// <param name="minSize">Minimize size of the string.</param>
		/// <param name="useLowerCaseLetters">if set to <c>true</c> [use lower case letters].</param><returns></returns>
		public static string ToUniqueId(this string value, int maxSize = 8, int minSize = 5, bool useLowerCaseLetters = false)
		{

			char[] chars = new char[62];

			string a = useLowerCaseLetters ? "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890" : "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

			chars = a.ToCharArray();

			int size = maxSize;
			byte[] data = new byte[1];
			RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();

			crypto.GetNonZeroBytes(data);
			size = maxSize;
			data = new byte[size];
			crypto.GetNonZeroBytes(data);

			StringBuilder result = new StringBuilder(size);
			foreach (byte b in data)
			{
				result.Append(chars[b % (chars.Length - 1)]);
			}

			return result.ToString();

		}

		/// <summary>
		/// Creates or concats a string value based on the number of times (ie. XXX) repeat value = "X" and count = 3
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="repeatValue">The repeat value.</param>
		/// <param name="count">The count.</param><returns></returns>
		public static string ToRepeat(this string value, string repeatValue, int count)
		{

			string x = new String(repeatValue.ToChar(), count);

			return value + x;

		}

		/// <summary>
		/// To the right.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="length">The length.</param><returns></returns>
		public static string ToRight(this string value, int length)
		{

			if (!value.IsNullEmpty())
			{
			    if (value.Length >= length)
				    return value.Substring(value.Length - length, length);
			    else
			        return value;
			}

            return string.Empty;

		}

		/// <summary>
		/// To the left.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="length">The length.</param><returns></returns>
		public static string ToLeft(this string value, int length)
		{

			if (!value.IsNullEmpty())
			{
			    if (value.Length >= length)
			        return value.Substring(0, length);
			    else
			        return value;
			}

            return string.Empty;

		}

		/// <summary>
		/// Toes the mid.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="start">The starting position</param>
		public static string ToMid(this string value, int start)
		{

			if (!value.IsNullEmpty())
			{
                if (value.Length <= start)
				    return value.Substring(start);
                else
			        return value;
			}

            return string.Empty;

		}

		/// <summary>
		/// Toes the mid.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="start">The starting position</param>
		/// <param name="length">The length. to return</param><returns></returns>
		public static string ToMid(this string value, int start, int length)
		{

			if (!value.IsNullEmpty())
			{
			    if (value.Length <= start)
			        return value.Substring(start, length);
			    else
                    return value;
			}

            return string.Empty;

		}

		/// <summary>
		/// Toes the alpha numeric.
		/// </summary>
		/// <param name="value">The value.</param><returns></returns>
		public static string ToAlphaNumeric(this string value)
		{

			if (!value.IsNullEmpty())
			{
				return Regex.Replace(value, "[^a-zA-Z0-9]", "");
			}
			else
			{
				return string.Empty;
			}

		}

		/// <summary>
		/// Toes the color.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static Color ToColor(this string value)
		{
			if (!value.IsNullEmpty())
			{
				return ColorTranslator.FromHtml(value);
			}
			else
			{
				return ColorTranslator.FromHtml("#FFFFFF");
			}
		}

		/// <summary>
		/// Toes the strip HTML.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <returns></returns>
		public static string ToStripHtml(this string input)
		{    
			// Will this simple expression replace all tags???    
			var tagsExpression = new Regex(@"</?.+?>");    
			return tagsExpression.Replace(input, " ");
		}

		/// <summary>
		/// Compresses the string
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static string ToCompress(this string value)
		{
			var bytes = Encoding.Unicode.GetBytes(value);
			using (var msi = new MemoryStream(bytes))
			using (var mso = new MemoryStream())
			{
				using (var gs = new GZipStream(mso, CompressionMode.Compress))
				{
					msi.CopyTo(gs);
				}
				return Convert.ToBase64String(mso.ToArray());
			}
		}

		/// <summary>
		/// DeCompresses the string
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static string ToDecompress(this string value)
		{
			var bytes = Convert.FromBase64String(value);
			using (var msi = new MemoryStream(bytes))
			using (var mso = new MemoryStream())
			{
				using (var gs = new GZipStream(msi, CompressionMode.Decompress))
				{
					gs.CopyTo(mso);
				}
				return Encoding.Unicode.GetString(mso.ToArray());
			}
		}

		/// <summary>
		/// To the split camel case.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static string ToSplitCamelCase(this string value)
		{
			return Regex.Replace(value, "(?<=[a-z])([A-Z])", " $1", RegexOptions.Compiled);
		}


		/// <summary>
		/// Converts a string To the MD5 hash.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static string ToMd5Hash(this string value)
		{
		    MD5 md5 = MD5.Create();

		    byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(value);
		    byte[] hash = md5.ComputeHash(inputBytes);

		    StringBuilder sb = new StringBuilder();

		    for (int i = 0; i < hash.Length; i++)
		    {
		        sb.Append(hash[i].ToString("x2"));
		    }

		    return sb.ToString();
		}

        #endregion

        #region "Contains"

        /// <summary>
        /// Determines whether the specified value contains letter.
        /// </summary>
        /// <param name="value">The value.</param><returns>
        ///   <c>true</c> if the specified value contains letter; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsLetter(this string value)
		{

			if (!value.IsNullEmpty())
			{
				return Regex.IsMatch(value.ToLower(), "[a-z]");
			}
			else
			{
				return false;
			}

		}

		/// <summary>
		/// Determines whether the specified value contains place holders.
		/// </summary>
		/// <param name="value">The value.</param><returns>
		///   <c>true</c> if the specified value contains letter; otherwise, <c>false</c>.
		/// </returns>
		public static bool ContainsPlaceHolders(this string value)
		{

			if (!value.IsNullEmpty())
			{
				return Regex.IsMatch(value, "\\|[a-zA-Z0-9]+\\|");
			}
			else
			{
				return false;
			}

		}

		#endregion

		#region "Is"

		/// <summary>
		/// Determines whether [contains valid email] [the specified value].
		/// </summary>
		/// <param name="value">The value.</param><returns>
		///   <c>true</c> if [contains valid email] [the specified value]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsValidEmail(this string value)
		{

			if (!value.IsNullEmpty())
			{
				return Regex.IsMatch(value.ToUpper(), "^[_a-zA-Z0-9-]+(\\.[_a-zA-Z0-9-]+)*@[a-zA-Z0-9-]+(\\.[a-zA-Z0-9-]+)*\\.(([0-9]{1,3})|([a-zA-Z]{2,3})|(aero|coop|info|museum|name))$");
			}
			else
			{
				return false;
			}

		}

		/// <summary>
		/// Determines whether [contains valid date] [the specified value].
		/// </summary>
		/// <param name="value">The value.</param><returns>
		///   <c>true</c> if [contains valid date] [the specified value]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsValidDate(this string value)
		{


			try
			{
				if (!value.IsNullEmpty())
				{
					DateTime dt = Convert.ToDateTime(value);
				}
				else
				{
					return false;
				}

				return true;

			}
			catch
			{
				return false;
			}

		}

		/// <summary>
		/// Determines whether [contains valid URL] [the specified value].
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///   <c>true</c> if [contains valid URL] [the specified value]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsValidUrl(this string value)
		{

			if (!value.IsNullEmpty())
			{
				return Regex.IsMatch(value, "(([\\w]+?//)?(([\\d\\w]|%[a-fA-f\\d]{2,2})+([\\d\\w]|%[a-fA-f\\d]{2,2})+)?@)?([\\d\\w][-\\d\\w]{0,253}[\\d\\w]\\.)+[\\w]{2,4}(:[\\d]+)?(/([-+_~.\\d\\w]|%[a-fA-f\\d]{2,2})*)*(\\?(&?([-+_~.\\d\\w]|%[a-fA-f\\d]{2,2})=?)*)?(#([-+_~.\\d\\w]|%[a-fA-f\\d]{2,2})*)?");
			}
			else
			{
				return false;
			}

		}

		/// <summary>
		/// Validate an IP address format
		/// </summary>
		/// <param name="value">IP Address</param>
		/// <returns></returns>
		/// <remarks></remarks>
		public static bool IsValidIPAddress(this string value)
		{

			if (!value.IsNullEmpty())
			{
				return Regex.IsMatch(value, "^(\\d{1,2}|1\\d\\d|2[0-4]\\d|25[0-5])\\.(\\d{1,2}|1\\d\\d|2[0-4]\\d|25[0-5])\\.(\\d{1,2}|1\\d\\d|2[0-4]\\d|25[0-5])\\.(\\d{1,2}|1\\d\\d|2[0-4]\\d|25[0-5])$");
			}
			else
			{
				return false;
			}

		}

		/// <summary>
		/// Determines whether the specified value is in.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="values">The values to compare.</param><returns>
		///   <c>true</c> if the specified obj is in; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsIn(this string value, params object[] values)
		{

			foreach (object val in values)
			{
				if (val.Equals(value))
					return true;
			}

			return false;

		}

		/// <summary>
		/// Determines whether the specified value is null or contains an empty string
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///   <c>true</c> if the specified obj is null or empty; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsNullEmpty(this string value)
		{

			return string.IsNullOrEmpty(value);

		}

		/// <summary>
		/// Determines whether the specified value is null or contains a white space
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///   <c>true</c> if the specified obj is null or empty; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsNullWhiteSpace(this string value)
		{

			return string.IsNullOrWhiteSpace(value);

		}

		/// <summary>
		/// Determines whether the specified value has an item cached.
		/// </summary>
		/// <remarks>The string value is based on the key of the cached item</remarks>
		/// <param name="value">The value.</param><returns>
		///   <c>true</c> if the specified value is cached; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsCached(this string value)
		{

			Cache ch = new Cache();

			try
			{
				if (ch.Get(value) != null)
					return true;
				else
					return false;
			}
			catch 
			{
				return false;
			}

		}

		/// <summary>
		/// Determines whether [is web runtime cached] [the specified value].
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static bool IsWebRuntimeCached(this string value)
		{

			try
			{
				if (System.Web.HttpRuntime.Cache.Get(value) != null)
					return true;
				else
					return false;
			}
			catch
			{
				return false;
			}

		}

		/// <summary>
		/// Determines whether the specified value is numeric.
		/// </summary>
		/// <param name="value">The value.</param><returns>
		///   <c>true</c> if the specified value is numeric; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsNumeric(this string value)
		{
			if (!value.IsNullEmpty())
			{
				return Regex.IsMatch(value, "[^0-9]");
			}
			else
			{
				return false;
			}

		}

		/// <summary>
		/// Determines whether the specified value is GUID.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///   <c>true</c> if the specified value is GUID; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsGuid(this string value) 
		{
			if (!value.IsNullEmpty())
			{
				Regex format = new Regex("^[A-Fa-f0-9]{32}$|" + "^({|\\()?[A-Fa-f0-9]{8}-([A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}(}|\\))?$|" + "^({)?[0xA-Fa-f0-9]{3,10}(, {0,1}[0xA-Fa-f0-9]{3,6}){2}, {0,1}({)([0xA-Fa-f0-9]{3,4}, {0,1}){7}[0xA-Fa-f0-9]{3,4}(}})$");
				Match match = format.Match(value);
				return match.Success; 
			}
			else
			{
				return false;
			}
		}

		#endregion

		#region "XML"

		/// <summary>
		/// Converts XMLs the schema to data set.
		/// </summary>
		/// <param name="value">The XML Schema.</param>
		/// <returns>Dataset</returns>
		public static DataSet XmlSchemaToDataSet(this string value)
		{

			DataSet dsXml = new DataSet();


			try
			{
				dsXml.ReadXmlSchema(new StringReader(value));
				return dsXml;

			}
			catch (Exception ex)
			{
				return null;
			}

		}

		/// <summary>
		/// ConvertXMLToDataSet - Convert the Xml into the Dataset
		/// </summary>
		/// <param name="value">The XML Data</param>
		/// <returns>Dataset</returns>
		public static DataSet XmlDataToDataSet(this string value)
		{

			XmlTextReader reader = null;

			try
			{
				DataSet xmlDs = new DataSet();
			    StringReader stream = new StringReader(value);

				// Load the XmlTextReader from the stream
				reader = new XmlTextReader(stream);
				xmlDs.ReadXmlSchema(reader);

				return xmlDs;


			}
			catch (Exception ex)
			{
				return null;
			}
			finally
			{
				if (reader != null)
				{
					reader.Close();
				}

			}

		}

		#endregion

		#region "System Configuration"

		/// <summary>
		/// Gets the Application Systems Setting.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>Return the system setting if one exists</returns>
		public static string SystemConfiguration(this string value)
		{

			if (!value.IsNullEmpty() && ConfigurationManager.AppSettings.AllKeys.Contains(value) && !ConfigurationManager.AppSettings[value].IsNullEmpty())
			{
				return ConfigurationManager.AppSettings[value];
			}
			else
			{
				return string.Empty;
			}

		}

		public static string SystemConnection(this string value)
		{
			var conns =  ConfigurationManager.ConnectionStrings.GetEnumerator();
		   
			do
			{
				if (conns.Current.GetPropertyValue("name").ToString() == value)
				{
					return ConfigurationManager.ConnectionStrings[value].ConnectionString;
				}

			} while (conns.MoveNext());
		  
			return string.Empty;
			

		}


		#endregion

		#region "Encryption"

		/// <summary>
		/// Decrypts the specified value.
		/// </summary>
		/// <param name="val">The value.</param>
		/// <param name="eKey">The key.</param>
		/// <param name="eVec">The vector.</param>
		/// <returns></returns>
		public static string Decrypt(this string val, string eKey, string eVec)
		{


			if (!eKey.IsNullEmpty() && !eVec.IsNullEmpty() && eKey.Length >= 8 && eVec.Length >= 8)
			{
				var encoding = Encoding.UTF8;

				MemoryStream ms = new MemoryStream();

				val = val.Replace(" ", "+");

				byte[] inputByteArray = new byte[val.Length + 1];

				byte[] key = Encoding.UTF8.GetBytes(eKey.Substring(0, 8));

				byte[] iv = Encoding.UTF8.GetBytes(eVec.Substring(0, 8));

				DESCryptoServiceProvider des = new DESCryptoServiceProvider();

				inputByteArray = Convert.FromBase64String(val);

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
		/// Encrypts the specified value.
		/// </summary>
		/// <param name="val">The value.</param>
		/// <param name="eKey">The key.</param>
		/// <param name="eVec">The vector.</param>
		/// <returns></returns>
		public static string Encrypt(this string val, string eKey, string eVec)
		{

			if (!eKey.IsNullEmpty() && !eVec.IsNullEmpty() && eKey.Length >= 8 && eVec.Length >= 8)
			{
				MemoryStream ms = new MemoryStream();

				byte[] key = Encoding.UTF8.GetBytes(eKey.Substring(0, 8));

				byte[] iv = Encoding.UTF8.GetBytes(eVec.Substring(0, 8));

				DESCryptoServiceProvider des = new DESCryptoServiceProvider();

				byte[] inputByteArray = Encoding.UTF8.GetBytes(val);

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

        /// <summary>
        /// Using Aes Encryption to encrypt a string.
        /// </summary>
        /// <param name="val">The value.</param>
        /// <param name="encryptionKey">The encryption key.</param>
        /// <returns></returns>
        public static string EncryptAes(this string val, string encryptionKey)
	    {
	        using (Aes aes = Aes.Create())
	        {
	            if (aes != null)
	            {
	                aes.Key = CreateKey(encryptionKey);

	                byte[] encrypted = AesEncryptStringToBytes(val, aes.Key, aes.IV);
	                return Convert.ToBase64String(encrypted) + ";" + Convert.ToBase64String(aes.IV);
	            }
	            else
	            {
                    return String.Empty;
	            }
	        }
	    }

        /// <summary>
        /// Aeses the encrypt string to bytes.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="key">The key.</param>
        /// <param name="iv">The iv.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// plainText
        /// or
        /// key
        /// or
        /// iv
        /// </exception>
        private static byte[] AesEncryptStringToBytes(string plainText, byte[] key, byte[] iv)
	    {
	        //if (plainText == null || plainText.Length <= 0)
	        //    throw new ArgumentNullException($"{nameof(plainText)}");
	        //if (key == null || key.Length <= 0)
	        //    throw new ArgumentNullException($"{nameof(key)}");
	        //if (iv == null || iv.Length <= 0)
	        //    throw new ArgumentNullException($"{nameof(iv)}");

	        byte[] encrypted = null;

	        using (Aes aes = Aes.Create())
	        {
	            if (aes != null)
	            {
	                aes.Key = key;
	                aes.IV = iv;

	                using (MemoryStream memoryStream = new MemoryStream())
	                {
	                    using (ICryptoTransform encryptor = aes.CreateEncryptor())
	                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
	                    using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
	                    {
	                        streamWriter.Write(plainText);
	                    }
	                    encrypted = memoryStream.ToArray();
	                }
	            }
	        }
	        return encrypted;
	    }

        /// <summary>
        /// Using Aes Decryption to encrypt a string.
        /// </summary>
        /// <param name="encryptedValue">The encrypted value.</param>
        /// <param name="encryptionKey">The encryption key.</param>
        /// <returns></returns>
        public static string DecryptAes(this string encryptedValue, string encryptionKey)
	    {
	        string iv = encryptedValue.Substring(encryptedValue.IndexOf(';') + 1, encryptedValue.Length - encryptedValue.IndexOf(';') - 1);
	        encryptedValue = encryptedValue.Substring(0, encryptedValue.IndexOf(';'));

	        return AesDecryptStringFromBytes(Convert.FromBase64String(encryptedValue), CreateKey(encryptionKey), Convert.FromBase64String(iv));
	    }

        /// <summary>
        /// Aeses the decrypt string from bytes.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="key">The key.</param>
        /// <param name="iv">The iv.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// cipherText
        /// or
        /// key
        /// or
        /// iv
        /// </exception>
        private static string AesDecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
	    {
	        //if (cipherText == null || cipherText.Length <= 0)
	        //    throw new ArgumentNullException($"{nameof(cipherText)}");
	        //if (key == null || key.Length <= 0)
	        //    throw new ArgumentNullException($"{nameof(key)}");
	        //if (iv == null || iv.Length <= 0)
	        //    throw new ArgumentNullException($"{nameof(iv)}");

	        string plaintext = null;

	        using (Aes aes = Aes.Create())
	        {
	            if (aes != null)
	            {
	                aes.Key = key;
	                aes.IV = iv;

	                using (MemoryStream memoryStream = new MemoryStream(cipherText))
	                using (ICryptoTransform decryptor = aes.CreateDecryptor())
	                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
	                using (StreamReader streamReader = new StreamReader(cryptoStream))
	                    plaintext = streamReader.ReadToEnd();
	            }
	        }
	        return plaintext;
	    }

        /// <summary>
        /// Creates the key.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="keyBytes">The key bytes.</param>
        /// <returns></returns>
        private static byte[] CreateKey(string password, int keyBytes = 32)
	    {
	        byte[] salt = new byte[] { 80, 70, 60, 50, 40, 30, 20, 10 };
	        int iterations = 300;
	        var keyGenerator = new Rfc2898DeriveBytes(password, salt, iterations);
	        return keyGenerator.GetBytes(keyBytes);
	    }

        #endregion

        #region "Formating"



        public static string RemoveWhiteSpaces(this string value)
		{
			var s = value.Where(c => !Char.IsWhiteSpace(c));
			return new String(s.ToArray());
		}
		/// <summary>
		/// Encodes the base64.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static string EncodeBase64(this string value)
		{
			string s = value.Trim().Replace(" ", "+");
			if (s.Length % 4 > 0)
				s = s.PadRight(s.Length + 4 - s.Length % 4, '=');
			return Encoding.UTF8.GetString(Convert.FromBase64String(s));
		}

		/// <summary>
		/// Formats the string replacing the placeholder with the property values that are within the object .
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="source">The source.</param><returns></returns>
		public static string FormatWithObject(this string value, object source)
		{

			return FormatWithObject(value, null, source);

		}

		/// <summary>
		/// Formats the string replacing the placeholder with the property values that are within the object .
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="provider">The provider.</param>
		/// <param name="source">The source.</param><returns></returns>
		public static string FormatWithObject(this string value, IFormatProvider provider, object source)
		{


			if (!value.IsNullEmpty() && value.ContainsPlaceHolders())
			{
				Regex r = new Regex("(?<start>\\|)+(?<property>[\\w\\.\\[\\]]+)(?<format>:[^}]+)?(?<end>\\|)+", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

				List<object> values = new List<object>();

				string rewrittenFormat = r.Replace(value, (Match m) =>
				{
					Group startGroup = m.Groups["start"];
					Group propertyGroup = m.Groups["property"];
					Group formatGroup = m.Groups["format"];
					Group endGroup = m.Groups["end"];
					values.Add((propertyGroup.Value == "0") ? source : DataBinder.Eval(source, propertyGroup.Value));
					return (new string('{', startGroup.Captures.Count) + (values.Count - 1)) + formatGroup.Value + new string('}', endGroup.Captures.Count);
				});

				return string.Format(provider, rewrittenFormat, values.ToArray());
			}
			else
			{
				return value;
			}

		}

		#endregion

		#region Cache

		/// <summary>
		/// Gets the web runtime cache.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <returns></returns>
		public static object GetWebRuntimeCache(this string val)
		{
			return System.Web.HttpRuntime.Cache.Get(val);
		}

        #endregion

        #region Log

        /// <summary>
        /// Writes a message To a defined log file.
        /// </summary>
        /// <param name="val">The value.</param>
        /// <param name="logFileLocation">The log file location.</param>
        /// <returns></returns>
        public static bool ToLogFile(this string val, string logFileLocation)
	    {

	        bool rtn = false;

	        try
	        {
                //create the path if it does not exist
	            if (!string.IsNullOrEmpty(logFileLocation) && !Directory.Exists(logFileLocation)) Directory.CreateDirectory(logFileLocation);

                // make sure that the directory exists
                if (!string.IsNullOrEmpty(logFileLocation) && Directory.Exists(logFileLocation))
	            {
	                //sLogFormat used to create log files format :
	                // dd/mm/yyyy hh:mm:ss AM/PM ==> Log Message
	                string sLogFormat = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " ==> ";

                    using (StreamWriter sw = new StreamWriter(logFileLocation.EnsureTerminatingDirectorySeparator() + "Log_" + DateTime.Now.ToYYYYMMDD() + ".log", true))
                    {
                        sw.WriteLine(sLogFormat + val);
                        sw.Flush();
                        sw.Close();
                    }

	            }

	            rtn = true;

	        }
	        catch (Exception e)
	        {
	            Console.WriteLine(e);
	        }

	        return rtn;

	    }

        #endregion

        #region File

        /// <summary>
        /// Ensures the terminating directory separator.
        /// </summary>
        /// <param name="val">The value.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">val</exception>
        public static string EnsureTerminatingDirectorySeparator(this string val)
	    {
	        if (val == null)
	            throw new ArgumentNullException("value");

	        int length = val.Length;
	        if (length == 0)
	            return "." + Path.DirectorySeparatorChar;

	        char lastChar = val[length - 1];
	        if (lastChar == Path.DirectorySeparatorChar || lastChar == Path.AltDirectorySeparatorChar)
	            return val;

	        int lastSep = val.LastIndexOfAny(new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar });
	        if (lastSep >= 0)
	            return val + val[lastSep];
	        else
	            return val + Path.DirectorySeparatorChar;
	    }

        #endregion

        #region RESTClient

        /// <summary>
        /// Rests the call.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="verb">The verb.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        public static T RestCall<T>(this string url, Method verb, Dictionary<string, string> parameters,
	        string user, string password, bool throwException = true, object body = null) where T : new()
	    {
	        return RestClientHelper.DoCall<T>(verb, parameters, url, user, password, throwException, body);
	    }

        /// <summary>
        /// Rests the call.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="verb">The verb.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="token">The token.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        public static T RestCall<T>(this string url, Method verb, Dictionary<string, string> parameters,
	        string token = "", bool throwException = true, object body = null) where T : new()
	    {
	        return RestClientHelper.DoCall<T>(verb, parameters, url, token, throwException, body);
	    }

        /// <summary>
        /// Rests the call a synchronize.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="verb">The verb.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        public static Task<T> RestCallASync<T>(this string url, Method verb, Dictionary<string, string> parameters,
	        string user, string password, bool throwException = true, object body = null) where T : new()
	    {
	        return RestClientHelper.DoCallAsync<T>(verb, parameters, url, user, password, throwException, body);
	    }


        /// <summary>
        /// Rests the call a synchronize.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">The URL.</param>
        /// <param name="verb">The verb.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="token">The token.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        public static Task<T> RestCallASync<T>(this string url, Method verb, Dictionary<string, string> parameters,
	        string token = "", bool throwException = true, object body = null) where T : new()
	    {
	        return RestClientHelper.DoCallAsync<T>(verb, parameters, url, token, throwException, body);
	    }

        #endregion

    }

}

