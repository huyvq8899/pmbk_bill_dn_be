using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKSOFT_SmartCA_P12
{
    public class MessageObj
    {
        public string PathCert { set; get; }

        public string Password { set; get; }

        public MLTDiep MLTDiep { set; get; }

        public string Description { set; get; }

        public string PathXMLSigned { set; get; }

        public string PathXMLOriginal { set; get; }

        public bool Status { set; get; }
    }

    public static class TextHelper
    {
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Compress(string uncompressedString)
        {
            byte[] compressedBytes;

            using (var uncompressedStream = new MemoryStream(Encoding.UTF8.GetBytes(uncompressedString)))
            {
                using (var compressedStream = new MemoryStream())
                {
                    using (var compressorStream = new DeflateStream(compressedStream, CompressionLevel.Optimal, true))
                    {
                        uncompressedStream.CopyTo(compressorStream);
                    }

                    compressedBytes = compressedStream.ToArray();
                }
            }

            return Convert.ToBase64String(compressedBytes);
        }

        public static string Decompress(string compressedString)
        {
            byte[] decompressedBytes;

            using (var compressedStream = new MemoryStream(Convert.FromBase64String(compressedString)))
            {
                using (var decompressorStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
                {
                    using (var decompressedStream = new MemoryStream())
                    {
                        decompressorStream.CopyTo(decompressedStream);

                        decompressedBytes = decompressedStream.ToArray();
                    }
                }
            }

            return Encoding.UTF8.GetString(decompressedBytes);
        }
    }
}
