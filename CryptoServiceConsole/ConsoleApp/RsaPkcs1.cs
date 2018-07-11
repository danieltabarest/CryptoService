using System;
using System.Text;
using PCLCrypto;

using static PCLCrypto.WinRTCrypto;

namespace CryptoForms
{
    /// <summary>
    /// Common cryptographic helper
    /// </summary>
    public static class RsaPkcs1
    {
        /// <summary>
        private static string _serverPrivateKey;
        private static string _serverPublicKeyString;


        public static void CreateKey()
        {
            var asym = AsymmetricKeyAlgorithmProvider.OpenAlgorithm(AsymmetricAlgorithm.RsaPkcs1);
            var key = asym.CreateKeyPair(1024);
            var publicKey = key.ExportPublicKey(CryptographicPublicKeyBlobType.Pkcs1RsaPublicKey);
            _serverPublicKeyString = Convert.ToBase64String(publicKey);
            var Key = key.Export(CryptographicPrivateKeyBlobType.Pkcs1RsaPrivateKey);
            _serverPrivateKey = Convert.ToBase64String(Key);
        }

        public static string Encrypt(string plainString)
        {
            var encryptedString = string.Empty;

            try
            {
                var asym = AsymmetricKeyAlgorithmProvider.OpenAlgorithm(AsymmetricAlgorithm.RsaPkcs1);
                var plain = Encoding.UTF8.GetBytes(plainString);
                var pubKeyBytes = Convert.FromBase64String(_serverPublicKeyString);
                var pubKey = asym.ImportPublicKey(pubKeyBytes, CryptographicPublicKeyBlobType.Pkcs1RsaPublicKey);
                var encrypted = CryptographicEngine.Encrypt(pubKey, plain);
                encryptedString = Convert.ToBase64String(encrypted);
            }
            catch (Exception ex)
            {
            }

            return encryptedString;
        }

        public static string Decrypt(string encryptedString)
        {
            var decryptedString = string.Empty;

            try
            {
                var asym = AsymmetricKeyAlgorithmProvider.OpenAlgorithm(AsymmetricAlgorithm.RsaPkcs1);
                var privateKeyBytes = Convert.FromBase64String(_serverPrivateKey);
                var privateKey = asym.ImportKeyPair(privateKeyBytes, CryptographicPrivateKeyBlobType.Pkcs1RsaPrivateKey);
                var encrypted = Convert.FromBase64String(encryptedString);
                var decrypted = CryptographicEngine.Decrypt(privateKey, encrypted);
                decryptedString = Encoding.UTF8.GetString(decrypted, 0, decrypted.Length);
            }
            catch (Exception ex)
            {
            }

            return decryptedString;
        }
    }
}
