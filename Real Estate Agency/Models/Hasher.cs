using System.Security.Cryptography;
using System.Text;

namespace REA.Models
{
    static class Hasher
    {
        private static string sSourceData;
        private static byte[] tmpSource;
        private static byte[] tmpHash;

        private static string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }

        public static string GetHash(string data)
        {
            sSourceData = data;
            tmpSource = Encoding.ASCII.GetBytes(sSourceData);
            tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
            return ByteArrayToString(tmpHash);
        }
    }
}
