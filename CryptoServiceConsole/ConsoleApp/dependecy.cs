using System;
using PCLCrypto;
using System.Text;
using System.IO;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;

namespace test
{
    public class SecurityService 
    {
        private readonly Encoding _encoding;
        private PaddedBufferedBlockCipher _cipher;

        public SecurityService()
        {
            _encoding = Encoding.UTF8;
        }

        public string Encrypt(string plain, string key)
        {
            byte[] result = BouncyCastleCrypto(true, _encoding.GetBytes(plain), key);
            return Convert.ToBase64String(result);
        }

        public string Decrypt(string cipher, string key)
        {
            byte[] result = BouncyCastleCrypto(false, Convert.FromBase64String(cipher), key);
            return _encoding.GetString(result, 0, result.Length);
        }

        private byte[] BouncyCastleCrypto(bool forEncrypt, byte[] input, string key)
        {
            try
            {
                _cipher = new PaddedBufferedBlockCipher(new DesEngine());
                byte[] keyByte = _encoding.GetBytes(key);
                _cipher.Init(forEncrypt, new KeyParameter(keyByte));
                return _cipher.DoFinal(input);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}

