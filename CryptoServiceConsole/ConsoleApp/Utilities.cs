using System;
using PCLCrypto;
using System.Text;
using System.IO;

namespace test
{
    static class Utilities
    {


        public static string SymmetricDecrypt(this string cipherText, string key, string eVec)
        {
            try
            {
                byte[] keyBuffer = Encoding.UTF8.GetBytes(key);
                byte[] iv = Encoding.UTF8.GetBytes(eVec);
                cipherText = cipherText.Replace(" ", "+");
                byte[] cipherTextBuffer = new byte[cipherText.Length + 1];
                cipherTextBuffer = Convert.FromBase64String(cipherText);

                //byte[] cipherTextBuffer = Encoding.UTF8.GetBytes(cipherText);

                ISymmetricKeyAlgorithmProvider symmetricAlgorithm = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(PCLCrypto.SymmetricAlgorithm.AesEcb);

                var symmetricKey = symmetricAlgorithm.CreateSymmetricKey(keyBuffer);
                var decryptor = WinRTCrypto.CryptographicEngine.CreateDecryptor(symmetricKey, iv);
                byte[] plainTextBuffer = decryptor.TransformFinalBlock(cipherTextBuffer, 0, 16);

                Convert.ToBase64String(plainTextBuffer, 0, plainTextBuffer.Length);
                return Convert.ToBase64String(plainTextBuffer, 0, plainTextBuffer.Length);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public static string Decrypt(this string val, string eKey, string eVec)
        {

            try
            {

                //if (!eKey.IsNullEmpty() && !eVec.IsNullEmpty() && eKey.Length >= 8 && eVec.Length >= 8)
                //{
                var encoding = Encoding.UTF8;

                MemoryStream ms = new MemoryStream();

                val = val.Replace(" ", "+");

                byte[] inputByteArray = new byte[val.Length + 1];

                byte[] key = Encoding.UTF8.GetBytes(eKey.Substring(0, 8));

                byte[] iv = Encoding.UTF8.GetBytes(eVec.Substring(0, 8));

                //DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                inputByteArray = Convert.FromBase64String(val);

                ISymmetricKeyAlgorithmProvider symmetricAlgorithm = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(PCLCrypto.SymmetricAlgorithm.DesCbcPkcs7);

                var symmetricKey = symmetricAlgorithm.CreateSymmetricKey(key);
                var decryptor = WinRTCrypto.CryptographicEngine.CreateDecryptor(symmetricKey, iv);

                CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write);

                cs.Write(inputByteArray, 0, inputByteArray.Length);

                cs.FlushFinalBlock();
                return encoding.GetString(ms.ToArray());

            }
            catch (Exception ex)
            {

                throw;
            }

            //}
            //else
            //{
            //    return string.Empty;

            //}

        }

        public static string SymmetricDecrypt2(this string cipherText, string key, string eVec)
        {
            try
            {
                var encoding = Encoding.UTF8;
                byte[] keyBuffer = Encoding.UTF8.GetBytes(key);
                byte[] iv = Encoding.UTF8.GetBytes(eVec);
                cipherText = cipherText.Replace(" ", "+");
                byte[] cipherTextBuffer = new byte[cipherText.Length + 1];
                cipherTextBuffer = Convert.FromBase64String(cipherText);

                //byte[] cipherTextBuffer = Encoding.UTF8.GetBytes(cipherText);

                ISymmetricKeyAlgorithmProvider symmetricAlgorithm = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(PCLCrypto.SymmetricAlgorithm.AesEcb);

                MemoryStream ms = new MemoryStream();
                var symmetricKey = symmetricAlgorithm.CreateSymmetricKey(keyBuffer);
                var decryptor = WinRTCrypto.CryptographicEngine.CreateDecryptor(symmetricKey, iv);
                //byte[] plainTextBuffer = decryptor.TransformFinalBlock(cipherTextBuffer, 0, 16);

                //Convert.ToBase64String(plainTextBuffer, 0, plainTextBuffer.Length);
                //return Convert.ToBase64String(plainTextBuffer, 0, plainTextBuffer.Length);

                CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write);

                cs.Write(cipherTextBuffer, 0, 16);

                cs.FlushFinalBlock();
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public static string SymmetricEncrypt(this string plaintext, string key)
        {
            byte[] keyBuffer = Convert.FromBase64String(key);
            byte[] plainTextBuffer = Convert.FromBase64String(plaintext);


            ISymmetricKeyAlgorithmProvider symmetricAlgorithm = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(PCLCrypto.SymmetricAlgorithm.AesEcbPkcs7);

            var symmetricKey = symmetricAlgorithm.CreateSymmetricKey(keyBuffer);
            var encryptor = WinRTCrypto.CryptographicEngine.CreateEncryptor(symmetricKey);
            byte[] cipherBuffer = encryptor.TransformFinalBlock(plainTextBuffer, 0, plainTextBuffer.Length);
            return Convert.ToBase64String(cipherBuffer);
        }

    }

}

