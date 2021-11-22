using System;
using System.Text;
using System.Data;
using System.Net.Sockets;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;

namespace BKSOFT.TCT.EMU
{
    public static class Utilities
    {
        public static byte[] bytesCombine(byte[] first, byte[] second)
        {
            byte[] ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            return ret;
        }

        public static int IndexOfBytes(byte[] array, byte[] pattern, int startIndex, int count)
        {
            int fidx = 0;
            int result = Array.FindIndex(array, startIndex, count, (byte b) =>
            {
                fidx = (b == pattern[fidx]) ? fidx + 1 : 0;
                return (fidx == pattern.Length);
            });
            return (result < 0) ? -1 : result - fidx + 1;
        }

        public static byte[] RemoveBytesFromBytes(byte[] source, int iStartPos, int iLength)
        {
            byte[] result = source;
            byte[] firstPart;
            byte[] secondPart;
            int iNewLength = source.Length - iLength;
            if (iNewLength >= 0)
            {
                result = new byte[iNewLength];
                firstPart = new byte[iStartPos];
                secondPart = new byte[source.Length - (iStartPos + iLength)];
                Array.Copy(source, 0, firstPart, 0, iStartPos);
                Array.Copy(source, iStartPos + iLength, secondPart, 0, source.Length - (iStartPos + iLength));
                result = bytesCombine(firstPart, secondPart);
            }

            return result;
        }

        public static byte[] RemoveBytesFromBytes(byte[] source, byte[] findPattern, int NumberOfBytesFollowingPattern)
        {

            byte[] result = source;
            int found = -1;

            while ((found = IndexOfBytes(result, findPattern, 0, result.Length)) >= 0)
            {
                //filter the First part of the bytes data 
                byte[] bytesFirstPart = new byte[found];
                Array.Copy(result, 0, bytesFirstPart, 0, found);

                //filter the Last part of the bytes data 
                int lastPartLenght = result.Length - (found + findPattern.Length + NumberOfBytesFollowingPattern);
                byte[] bytesLastPart = new byte[0];

                if (lastPartLenght > 0)
                {
                    bytesLastPart = new byte[lastPartLenght];
                    Array.Copy(result, found + findPattern.Length + NumberOfBytesFollowingPattern,
                        bytesLastPart, 0, lastPartLenght);

                }

                int newSize = bytesFirstPart.Length + bytesLastPart.Length;
                var ms = new MemoryStream(new byte[newSize], 0, newSize, true, true);
                ms.Write(bytesFirstPart, 0, bytesFirstPart.Length);
                ms.Write(bytesLastPart, 0, bytesLastPart.Length);
                result = ms.GetBuffer();
            }
            return result;
        }

        public static string GeneratePassword()
        {
            string text = string.Empty;

            try
            {
                text = $"@{DateTime.Now.ToString("dd-MM-yyyy")}#";

                // 1st
                text = Base64Encode(text);

                // 2st
                text = Base64Encode(text);
            }
            catch (Exception)
            {
                throw;
            }

            return text;
        }

        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string Between(string source, string left, string right)
        {
            return Regex.Match(
                    source,
                    string.Format("{0}(.*){1}", left, right))
                .Groups[1].Value;
        }
    }
}
