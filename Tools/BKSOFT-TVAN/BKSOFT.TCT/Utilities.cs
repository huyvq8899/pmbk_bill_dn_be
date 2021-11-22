using System;
using System.Text;
using System.Data;
using System.Net.Sockets;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Web.Administration;
using System.Text.RegularExpressions;

namespace BKSOFT.TCT
{
    public static class Utilities
    {
        public static string GetPhysicalPathFromSiteName(string siteName)
        {
            string path = string.Empty;
            try
            {
                ServerManager m = new ServerManager();

                path = m.Sites[siteName].Applications["/"].VirtualDirectories["/"].PhysicalPath;
            }
            catch (Exception ex)
            {
                GPSFileLog.WriteLog(string.Empty, ex);
            }

            return path;
        }

        public static string GetConnectionStringFromConfig(string path)
        {
            string con = string.Empty;

            try
            {
                string line = string.Empty;

                // Read the file and display it line by line.  
                System.IO.StreamReader file = new System.IO.StreamReader(path);
                while ((line = file.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line.StartsWith("\"DefaultConnection\""))
                    {
                        con = Regex.Matches(line, "\\\"(.*?)\\\"")[1].ToString().Trim('"');
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                GPSFileLog.WriteLog(string.Empty, ex);
            }

            return con;
        }

        public static bool GetBit(this byte b, int bitNumber)
        {
            return (b & (1 << bitNumber)) != 0;
        }

        public static bool GetBitPosition(byte b, int position)
        {
            return (b & (1 << position)) != 0;
        }

        public static string convertToUnSign2(string s)
        {
            string stFormD = s.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            sb = sb.Replace('Đ', 'D');
            sb = sb.Replace('đ', 'd');
            return (sb.ToString().Normalize(NormalizationForm.FormD));
        }

        public static double CalculateDistance(double p1Lat, double p1Lon, double p2Lat, double p2Lon)
        {
            var dLat = (p2Lat - p1Lat) * Math.PI / 180;
            var dLon = (p2Lon - p1Lon) * Math.PI / 180;
            var lat1 = p1Lat * Math.PI / 180;
            var lat2 = p2Lat * Math.PI / 180;

            var R = 6378.15; // km  
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c;
            return d;
        }

        public static string GetAddress(double? lat, double? lng)
        {
            string address = string.Empty;

            try
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GPS_ADDRESS"].ConnectionString);

                conn.Open();

                // 1.  create a command object identifying the stored procedure
                SqlCommand cmd = new SqlCommand("usp_GetAddress", conn);

                // 2. set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // 3. add parameter to command, which will be passed to the stored procedure
                cmd.Parameters.Add(new SqlParameter("@Latitude", lat));
                cmd.Parameters.Add(new SqlParameter("@Longtitude", lng));

                // execute the command
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    // iterate through results, printing each to console
                    while (rdr.Read())
                    {
                        address = rdr["Addr"].ToString();
                    }
                }

                conn.Close();
                conn.Dispose();

            }
            catch (Exception ex)
            {
                GPSFileLog.WriteLog("Get address error", ex);
            }
            return address;
        }

        public static string GenarateSpeedPerSecond(int preSpeed, int Speed, int step)
        {
            StringBuilder speedPerSecond = new StringBuilder();
            Random rnd = new Random();
            int val = 0;
            try
            {
                for (int i = 0; i < step; i++)
                {
                    if (preSpeed < Speed)
                        val = rnd.Next(preSpeed, Speed);
                    else
                        val = rnd.Next(Speed, preSpeed);

                    // Add to list speeds
                    if (i < step - 1)
                        speedPerSecond.Append(val + ",");
                    else
                        speedPerSecond.Append(val.ToString());
                }
            }
            catch (Exception ex)
            {
                GPSFileLog.WriteLog("Genarate Speed Second", ex);
            }
            return speedPerSecond.ToString();
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

        public static bool TcpClient_Send(TcpClient tcpClient, string data)
        {
            try
            {
                Socket serverStream = tcpClient.Client;
                byte[] outStream = System.Text.Encoding.ASCII.GetBytes(data);
                serverStream.Send(outStream);
                return true;
            }
            catch (Exception ex)
            {
                GPSFileLog.WriteLog(string.Empty, ex);

                return false;
            }
        }

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
