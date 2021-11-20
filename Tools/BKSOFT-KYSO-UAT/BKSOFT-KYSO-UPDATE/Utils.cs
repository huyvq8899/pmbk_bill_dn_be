using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKSOFT_KYSO_UPDATE
{
    public class Utils
    {
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string BytesToHexStr(byte[] buffer)
        {
            StringBuilder sbLogSource = new StringBuilder();
            for (int idx = 0; idx < buffer.Length; idx++)
            {
                sbLogSource.Append(String.Format("{0:x2}", buffer[idx]));
            }
            return (sbLogSource.ToString().ToUpper());
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}
