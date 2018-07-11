using System.IO;

namespace NSU.Utilities.Extensions
{
    /// <summary>
    /// Contains extensions for Streams
    /// </summary>
    public static class xStream
    {
        /// <summary>
        /// Converts a stream to a byte array.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static byte[] ToByteArray(this Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

    }
}
