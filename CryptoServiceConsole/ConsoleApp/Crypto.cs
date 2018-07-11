using System.Text;
using PCLCrypto;

namespace CryptoForms
{
    /// <summary>
    /// Common cryptographic helper
    /// </summary>
    public static class Crypto
    {
        /// <summary>
        /// Creates Salt with given length in bytes.
        /// </summary>
        /// <param name="lengthInBytes">No. of bytes</param>
        /// <returns></returns>
        public static byte[] CreateSalt(string value)
        {
            //return WinRTCrypto.CryptographicBuffer.GenerateRandom(lengthInBytes);
            return WinRTCrypto.CryptographicBuffer.DecodeFromBase64String(value);
        }

        /// <summary>
        /// Creates a derived key from a comnination 
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <param name="keyLengthInBytes"></param>
        /// <param name="iterations"></param>
        /// <returns></returns>
        public static byte[] CreateDerivedKey(string password, byte[] salt, int keyLengthInBytes = 32, int iterations = 1000)
        {
            byte[] key = NetFxCrypto.DeriveBytes.GetBytes(password, salt, iterations, keyLengthInBytes);
            return key;
        }
     
        /// <summary>
        /// Encrypts given data using symmetric algorithm AES
        /// </summary>
        /// <param name="data">Data to encrypt</param>
        /// <param name="password">Password</param>
        /// <param name="salt">Salt</param>
        /// <returns>Encrypted bytes</returns>
        public static byte[] EncryptAes(string data, string password, byte[] salt)
        {
            byte[] key = CreateDerivedKey(password, salt);
       
            ISymmetricKeyAlgorithmProvider aes = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);
            ICryptographicKey symetricKey = aes.CreateSymmetricKey(key);
            var bytes = WinRTCrypto.CryptographicEngine.Encrypt(symetricKey, Encoding.UTF8.GetBytes(data));
            return bytes;
        }
        /// <summary>
        /// Decrypts given bytes using symmetric alogrithm AES
        /// </summary>
        /// <param name="data">data to decrypt</param>
        /// <param name="password">Password used for encryption</param>
        /// <param name="salt">Salt used for encryption</param>
        /// <returns></returns>
        public static string DecryptAes(string keymaterial,byte[] data, string password, byte[] salt, byte[] iv)
        {
            byte[] key = CreateDerivedKey(password, salt,16);
            ISymmetricKeyAlgorithmProvider aes = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);

            var v = aes.LegalKeySizes;
            ICryptographicKey symetricKey = aes.CreateSymmetricKey(key);
            //symetricKey.ExportParameters(true);
            var bytes = WinRTCrypto.CryptographicEngine.Decrypt(symetricKey, data);
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }

        public static string DecryptAes( byte[] data, string password, byte[] salt)
        {
            byte[] key = CreateDerivedKey(password, salt);
            ISymmetricKeyAlgorithmProvider aes = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmName.Aes, SymmetricAlgorithmMode.Cbc,SymmetricAlgorithmPadding.None);

            var v = aes.LegalKeySizes;
            ICryptographicKey symetricKey = aes.CreateSymmetricKey(key);
            //symetricKey.ExportParameters(true);
            var bytes = WinRTCrypto.CryptographicEngine.Decrypt(symetricKey, data);
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }

    }
}
