using PCLCrypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easyfood_Xamarin.Util
{
    public class UIHelper
    {
        #region Accounts
        public static string GenerateSalt()
        {
            //byte[] buf = new byte[16];
            //RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            //crypto.GetBytes(arr);

            var buf = WinRTCrypto.CryptographicBuffer.GenerateRandom(16);
            return Convert.ToBase64String(buf);
        }

        public static string GenerateHash(string lozinka, string salt)
        {
            byte[] byteLozinka;
            byte[] byteSalt;
            byte[] forHashing;
            try
            {
                byteLozinka = Encoding.Unicode.GetBytes(lozinka);
                byteSalt = Convert.FromBase64String(salt);
                forHashing = new byte[byteLozinka.Length + byteSalt.Length];
            }
            catch (Exception e)
            {
                // check the callstack here
                // exception ćemo uhvatiti ako ne možemo dohvatiti bytove ili base64 od lozinke ili salta (ako nisu dobrog formata ili dužine)
                // također, vrati bilo koji string kao hash, jer je svakako nije validan
                return "hashinvalid";
            }

            //strcpy i strcat
            System.Buffer.BlockCopy(byteLozinka, 0, forHashing, 0, byteLozinka.Length);
            System.Buffer.BlockCopy(byteSalt, 0, forHashing, byteLozinka.Length, byteSalt.Length);

            //HashAlgorithm alg = HashAlgorithm.Create("SHA1");
            var alg = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(HashAlgorithm.Sha1);

            return Convert.ToBase64String(alg.HashData(forHashing));
        }
        #endregion

    }
}
