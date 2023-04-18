using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.ESignCloud
{
    public class Utils
    {
        public static string getPKCS1Signature(string data, string key, string passkey)
        {
            MakeSignature mks = new MakeSignature(data, key, passkey);
            return mks.getSignature();
        }

        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }

        private static long nanoTime()
        {
            long nano = 10000L * Stopwatch.GetTimestamp();
            nano /= TimeSpan.TicksPerMillisecond;
            nano *= 100L;
            return nano;
        }

        internal static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static byte[] Base64Encode(byte[] rawData)
        {
            //Console.WriteLine(Encoding.Default.GetString(rawData));
            var data = Convert.ToBase64String(rawData);
            return Encoding.UTF8.GetBytes(data);
        }
        public static byte[] Base64Decode(byte[] base64EncodedData)
        {
            var data = Encoding.UTF8.GetString(base64EncodedData);
            var base64EncodedBytes = Convert.FromBase64String(data);
            //Console.WriteLine(Encoding.Default.GetString(base64EncodedBytes));
            return base64EncodedBytes;
        }
    }
}
