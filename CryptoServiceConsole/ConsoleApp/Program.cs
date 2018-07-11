using CryptoForms;
using Newtonsoft.Json;
using NSU.Utilities.Extensions;
using PCLCrypto;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using test;

using static PCLCrypto.WinRTCrypto;


namespace ConsoleApp
{
    static class Program
    {

        /// <summary>
		/// Decrypts the specified value.
		/// </summary>
		/// <param name="val">The value.</param>
		/// <param name="eKey">The key.</param>
		/// <param name="eVec">The vector.</param>
		/// <returns></returns>
		//public static string Decrypt(this string val, string eKey, string eVec)
  //      {


  //          try
  //          {

  //              var encoding = Encoding.UTF8;

  //              //MemoryStream ms = new MemoryStream();

  //              val = val.Replace(" ", "+");

  //              byte[] inputByteArray = new byte[val.Length + 1];

  //              byte[] key = Encoding.ASCII.GetBytes(eKey.Substring(0, 8));

  //              byte[] iv = Encoding.ASCII.GetBytes(eVec.Substring(0, 8));


  //              //byte[] iv = Convert.FromBase64String(eVec);
  //              //byte[] key = Convert.FromBase64String(eKey);

  //              inputByteArray = Convert.FromBase64String(val);

  //              //ISymmetricKeyAlgorithmProvider aes = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesEcb);
  //              var aes = WinRTCrypto.KeyDerivationAlgorithmProvider.OpenAlgorithm(KeyDerivationAlgorithm.Pbkdf2Md5);
  //              ICryptographicKey symetricKey = aes.CreateKey(key);

  //              using (var ms = new MemoryStream())
  //              {
  //                  using (var decryptor = WinRTCrypto.CryptographicEngine.CreateDecryptor(symetricKey, iv))
  //                  {
  //                      //CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, iv), CryptoStreamMode.Write);
  //                      CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write);

  //                      cs.Write(inputByteArray, 0, inputByteArray.Length);

  //                      cs.FlushFinalBlock();
  //                      return encoding.GetString(ms.ToArray());
  //                  }
  //              }
  //          }
  //          catch (Exception ex)
  //          {

  //              throw;
  //          }

  //      }

        public static byte[] CreateSalt(int lengthInBytes)
        {
           var   salt = WinRTCrypto.CryptographicBuffer.GenerateRandom(lengthInBytes);
            return salt;
        }

        const string serviceID = "Diary";
        const string pwKey = "password";
        const string kmKey = "keymaterial";
        const string saltKey = "salt";
        static void Main(string[] args)
        {

            try
            {
                test();
                var data = "Cryptographic example";
                var pass = "MySecretKey";
                var eKey = "ASDFCVBFGRHETPOWTOEISJAHSGDTRIEO";
                var eVec = "KDSLAOIEYRTDGSND";
                var val = "PR/+kxycXMcINA6OssIrPkI5drh1ukIMYOXew5mQLI+7qDXoiMZg6A==";
                byte[] key = Encoding.UTF8.GetBytes(eKey.Substring(0, 8));
                byte[] iv = Encoding.UTF8.GetBytes(eVec.Substring(0, 8));
                val = val.Replace(" ", "+");
                byte[] inputByteArray = new byte[val.Length + 1];



                data = "Examples";
                string keyd = "ASDFCVBFGRHETPOWTOEISJAHSGDTRIEO".Trim();
                var Vecd = "KDSLAOIEYRTDGSND".Trim();

                string plasinText = Utilities.Decrypt(val, keyd, Vecd);

                //string cipherText = Utilities.SymmetricEncrypt(data, keyd);
                //string plainText = Utilities.SymmetricDecrypt(cipherText, keyd);

                var returnvm = callApi<object>(@"https://api.stage.nova.edu/appcentral/identity/instances/0/ad/user?userNameEmailAddress=tt451&createPassword=false", "6423b8e734d88153bee0160226e826b2".Trim(), plasinText);


                //Task<object> v = callApi<object>(@"https://api.stage.nova.edu/appcentral/identity/instances/0/ad/user?userNameEmailAddress=tt451&createPassword=false", "6423b8e734d88153bee0160226e826b2".Trim(), "PR/+kxycXMcINA6OssIrPkI5drh1ukIMYOXew5mQLI+7qDXoiMZg6A==".Trim().Decrypt("ASDFCVBFGRHETPOWTOEISJAHSGDTRIEO".Trim(), "KDSLAOIEYRTDGSND".Trim()));
                //object lst = await returnvm;


                Console.ReadKey();
                //var encrypted = Encrypt("This is a test");
                //RsaPkcs1.CreateKey();
                //var s= RsaPkcs1.Decrypt(val);
                // create the key we are going to use
                //test(val);


                //GetDiaryText(key);
                //Decrypt("PR/+kxycXMcINA6OssIrPkI5drh1ukIMYOXew5mQLI+7qDXoiMZg6A==","ASDFCVBFGRHETPOWTOEISJAHSGDTRIEO", "KDSLAOIEYRTDGSND");

                //var dkey = CryptoUtilities.StringToByteArray(eKey);


                ////var value = Cryptos.DecryptAes(inputByteArray, key);
                //var value = DecryptAes(val, eKey, key, iv);
                //var salt = Crypto.CreateSalt(eVec);
                ////await contentPage.DisplayAlert("Alert", "Encrypting String " + data + ", with salt " + BitConverter.ToString(salt), "OK");
                //var bytes = Crypto.EncryptAes(eKey, val, salt);
                //inputByteArray = Convert.FromBase64String(val);
            }
            catch (Exception ex)
            {

            }

        }
        private static void test()
        {
            try
            {
                var s = new SecurityService();
                string keyd = "ASDFCVBFGRHETPOWTOEISJAHSGDTRIEO".Trim();
                var Vecd = "KDSLAOIEYRTDGSND".Trim();
                s.Decrypt(Vecd, keyd);
            }
            catch (Exception ex)
            {

            }
        }

            private static async Task<object> callApi<T>(string url, string username, string password) where T : new()
        {
            try
            {
                using (var client = new HttpClient())
                {


                    CancellationToken cancellationToken = new CancellationToken();
                    HttpRequestMessage httpRequest = new HttpRequestMessage();
                    httpRequest.Method = new HttpMethod("GET");


                    httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var credentials = Encoding.UTF8.GetBytes(username + ":" + password);

                    httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(credentials));
                    httpRequest.RequestUri = new Uri(url);

                    HttpResponseMessage httpResponse = await client.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
                    HttpStatusCode statusCode = httpResponse.StatusCode;


                    var response = await httpResponse.Content.ReadAsStringAsync();
                    try
                    {
                        object serializedObject = JsonConvert.DeserializeObject<object>(response);
                        return serializedObject;
                    }
                    catch (Exception ex)
                    {

                        throw new Exception(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private static void test(string val)
        {
            var asym = AsymmetricKeyAlgorithmProvider.OpenAlgorithm(AsymmetricAlgorithm.RsaPkcs1);
            var hash = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithm.Sha1);

            var encryptedString = val;
            var encrypted = Convert.FromBase64String(encryptedString);
            var key = asym.CreateKeyPair(512);
            var publicKey = key.ExportPublicKey();
            var publicKeyString = Convert.ToBase64String(publicKey);

            var decrypted = CryptographicEngine.Decrypt(key, encrypted);
            var decryptedString = Encoding.UTF8.GetString(decrypted, 0, decrypted.Length);
        }

        public static string GetDiaryText(byte[] cipherText)
        {
          
            byte[] keyMaterial = Convert.FromBase64String(kmKey);

            return CryptoUtilities.ByteArrayToString(
                CryptoUtilities.Decrypt(cipherText, keyMaterial));
        }


        public static string DecryptAes(string encryptedValue, string encryptionKey, byte[] rgbKey, byte[] rgbIV)
        {
            string iv = encryptedValue.Substring(encryptedValue.IndexOf(';') + 1, encryptedValue.Length - encryptedValue.IndexOf(';') - 1);
            //encryptedValue = encryptedValue.Substring(0, encryptedValue.IndexOf(';'));

            //return Cryptos.DecryptAes(Convert.FromBase64String(encryptedValue), CreateKey(encryptionKey));
            return Cryptos.DecryptAes(Convert.FromBase64String(encryptedValue), CreateKey(encryptionKey, rgbKey), rgbKey, rgbIV);
        }

        /// <summary>
        /// Creates the key.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="keyBytes">The key bytes.</param>
        /// <returns></returns>
        private static byte[] CreateKey(string password, byte[] rgbKey, int keyBytes = 16)
        {
            byte[] salt = new byte[] { 80, 70, 60, 50, 40, 30, 20, 10 };
            int iterations = 300;
            //var keyGenerator = new Rfc2898DeriveBytes(password, salt, iterations);
            //return keyGenerator.GetBytes(keyBytes);
            int keyLengthInBytes = 8;
            byte[] key = NetFxCrypto.DeriveBytes.GetBytes(rgbKey, salt, iterations, keyLengthInBytes);
            return key;

        }
    }
}
