using System;
//using System.Security.Cryptography;
using System.Text;
using System.IO;
using PCLCrypto;

namespace test
{
    /* /// <summary>
     /// The <see cref="System.Security.Cryptography.Crypto"/> provides an easy way encrypt and decrypt
     /// data using a simple password.
     /// </summary>
     /// <remarks>
     /// Code based on the book "C# 3.0 in a nutshell by Joseph Albahari" (pages 630-632)
     /// and from this StackOverflow post by somebody called Brett
     /// http://stackoverflow.com/questions/202011/encrypt-decrypt-string-in-net/2791259#2791259
     /// </remarks>
     static internal class Cryptos
     {
         // Define the secret salt value for encrypting data
         private static readonly byte[] salt = Encoding.UTF8.GetBytes("Xamarin.iOS Version: 7.0.6.168");

         /// <summary>
         /// Takes the given text string and encrypts it using the given password.
         /// </summary>
         /// <param name="textToEncrypt">Text to encrypt.</param>
         /// <param name="encryptionPassword">Encryption password.</param>
         internal static string Encrypt(string textToEncrypt, string encryptionPassword)
         {
             var algorithm = GetAlgorithm(encryptionPassword);

             //Anything to process?
             if (textToEncrypt == null || textToEncrypt == "") return "";

             byte[] encryptedBytes;
             using (ICryptoTransform encryptor = algorithm.CreateEncryptor(algorithm.Key, algorithm.IV))
             {
                 byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(textToEncrypt);
                 encryptedBytes = InMemoryCrypt(bytesToEncrypt, encryptor);
             }
             return Convert.ToBase64String(encryptedBytes);
         }

         /// <summary>
         /// Takes the given encrypted text string and decrypts it using the given password
         /// </summary>
         /// <param name="encryptedText">Encrypted text.</param>
         /// <param name="encryptionPassword">Encryption password.</param>
         internal static string Decrypt(string encryptedText, string encryptionPassword)
         {
             var algorithm = GetAlgorithm(encryptionPassword);

             //Anything to process?
             if (encryptedText == null || encryptedText == "") return "";

             byte[] descryptedBytes;
             using (ICryptoTransform decryptor = algorithm.CreateDecryptor(algorithm.Key, algorithm.IV))
             {
                 byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                 descryptedBytes = InMemoryCrypt(encryptedBytes, decryptor);
             }
             return Encoding.UTF8.GetString(descryptedBytes, 0, descryptedBytes.Length);
             //return Encoding.UTF8.GetString(descryptedBytes);
         }

         /// <summary>
         /// Performs an in-memory encrypt/decrypt transformation on a byte array.
         /// </summary>
         /// <returns>The memory crypt.</returns>
         /// <param name="data">Data.</param>
         /// <param name="transform">Transform.</param>
         private static byte[] InMemoryCrypt(byte[] data, ICryptoTransform transform)
         {
             MemoryStream memory = new MemoryStream();
             using (Stream stream = new CryptoStream(memory, transform, CryptoStreamMode.Write))
             {
                 stream.Write(data, 0, data.Length);
             }
             return memory.ToArray();
         }

         /// <summary>
         /// Defines a RijndaelManaged algorithm and sets its key and Initialization Vector (IV) 
         /// values based on the encryptionPassword received.
         /// </summary>
         /// <returns>The algorithm.</returns>
         /// <param name="encryptionPassword">Encryption password.</param>
         private static RijndaelManaged GetAlgorithm(string encryptionPassword)
         {
             // Create an encryption key from the encryptionPassword and salt.
             var key = new Rfc2898DeriveBytes(encryptionPassword, salt);

             // Declare that we are going to use the Rijndael algorithm with the key that we've just got.
             var algorithm = new RijndaelManaged();
             int bytesForKey = algorithm.KeySize / 8;
             int bytesForIV = algorithm.BlockSize / 8;
             algorithm.Key = key.GetBytes(bytesForKey);
             algorithm.IV = key.GetBytes(bytesForIV);
             return algorithm;
         }

     }
     */
    /* static internal class Cryptos
     {
         static public string Encrypt(string originalPayload, byte[] privateKey)
         {
             string encryptedPayload = "";

             ISymmetricKeyAlgorithmProvider aes = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);
             ICryptographicKey symetricKey = aes.CreateSymmetricKey(privateKey);
             var iv = WinRTCrypto.CryptographicBuffer.GenerateRandom(aes.BlockLength);

             var input = Encoding.UTF8.GetBytes(originalPayload);

             using (var encrypter = WinRTCrypto.CryptographicEngine.CreateEncryptor(symetricKey, iv))
             {
                 using (var cipherStream = new MemoryStream())
                 {
                     using (var tCryptoStream = new CryptoStream(cipherStream, encrypter, CryptoStreamMode.Write))
                     using (var tBinaryWriter = new BinaryWriter(tCryptoStream))
                     {
                         //Prepend IV to data                        
                         cipherStream.Write(iv, 0, iv.Length);  //Write iv to the plain stream (not tested though)
                         tBinaryWriter.Write(input);
                         tCryptoStream.FlushFinalBlock();
                     }

                     encryptedPayload = Convert.ToBase64String(cipherStream.ToArray());
                 }
             }

             return encryptedPayload;
         }

         public static string DecryptAes(byte[] data, byte[] key)
         {
             ISymmetricKeyAlgorithmProvider aes = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);
             ICryptographicKey symetricKey = aes.CreateSymmetricKey(key);

             string returnValue = "";

             using (var ms = new MemoryStream(data))
             {
                 // Read the first 16 bytes which is the IV.
                 byte[] iv = new byte[16];
                 ms.Read(iv, 0, 16);
                 using (var decryptor = WinRTCrypto.CryptographicEngine.CreateDecryptor(symetricKey, iv))
                 {
                     using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                     {
                         using (var sr = new StreamReader(cs))
                         {
                             returnValue = sr.ReadToEnd();
                         }
                     }
                 }
             }
             return returnValue;
         }
     }*/


     public static class Cryptos
    {
         public static string Encrypt(string originalPayload, byte[] privateKey)
        {
            string encryptedPayload = "";

            ISymmetricKeyAlgorithmProvider aes = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);
            ICryptographicKey symetricKey = aes.CreateSymmetricKey(privateKey);
            var iv = WinRTCrypto.CryptographicBuffer.GenerateRandom(aes.BlockLength);

            var input = Encoding.UTF8.GetBytes(originalPayload);

            using (var encrypter = WinRTCrypto.CryptographicEngine.CreateEncryptor(symetricKey, iv))
            {
                using (var cipherStream = new MemoryStream())
                {
                    using (var tCryptoStream = new CryptoStream(cipherStream, encrypter, CryptoStreamMode.Write))
                    using (var tBinaryWriter = new BinaryWriter(tCryptoStream))
                    {
                        //Prepend IV to data                        
                        cipherStream.Write(iv, 0, iv.Length);  //Write iv to the plain stream (not tested though)
                        tBinaryWriter.Write(input);
                        tCryptoStream.FlushFinalBlock();
                    }

                    encryptedPayload = Convert.ToBase64String(cipherStream.ToArray());
                }
            }

            return encryptedPayload;
        }

        public static string DecryptAes(byte[] data, byte[] key, byte[] rgbKey, byte[] rgbIV)
        {
            ISymmetricKeyAlgorithmProvider aes = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbc);
            ICryptographicKey symetricKey = aes.CreateSymmetricKey(rgbKey);

            string returnValue = "";

            using (var ms = new MemoryStream(data))
            {
                // Read the first 16 bytes which is the IV.
                byte[] iv = new byte[16];
                ms.Read(iv, 0, 16);
                using (var decryptor = WinRTCrypto.CryptographicEngine.CreateDecryptor(symetricKey, iv))
                {
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (var sr = new StreamReader(cs))
                        {
                            returnValue = sr.ReadToEnd();
                        }
                    }
                }
            }
            return returnValue;
        }
    }
}