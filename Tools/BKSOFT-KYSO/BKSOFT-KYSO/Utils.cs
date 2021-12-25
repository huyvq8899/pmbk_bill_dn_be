using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace BKSOFT_KYSO
{
    public class Utils
    {
        public static List<USBDeviceInfo> GetUSBDevices()
        {
            List<USBDeviceInfo> devices = null;
            try
            {
                ManagementObjectCollection collection;
                using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_USBHub"))
                {
                    collection = searcher.Get();

                    devices = new List<USBDeviceInfo>();
                    foreach (var device in collection)
                    {
                        devices.Add(new USBDeviceInfo(
                        (string)device.GetPropertyValue("DeviceID"),
                        (string)device.GetPropertyValue("PNPDeviceID"),
                        (string)device.GetPropertyValue("Description")
                        ));
                    }
                    collection.Dispose();
                }
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);
            }

            return devices;
        }

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

        public static bool RemoveDirectory(string path)
        {
            bool res = true;
            try
            {
                DirectoryInfo dir = new DirectoryInfo(path);

                foreach (FileInfo fi in dir.GetFiles())
                {
                    fi.Delete();
                }

                foreach (DirectoryInfo di in dir.GetDirectories())
                {
                    RemoveDirectory(di.FullName);

                    di.Delete();
                }
            }
            catch (Exception)
            {
                res = false;
            }

            return res;
        }

        public static string GetMaSoThueFromSubject(string subject)
        {
            string result = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(subject))
                {
                    return result;
                }
                subject = subject.Trim();

                string[] words = subject.Split(" \r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                foreach (var item in words)
                {
                    string word = item.Trim();
                    if (word.Contains("MST"))
                    {
                        int idx = word.IndexOf("MST") + 3;
                        word = word.Substring(idx);

                        int begin = 0;
                        int end = 0;

                        for (int i = 0; i < word.Length; i++)
                        {
                            if (Char.IsDigit(word[i]))
                            {
                                begin = i;
                                break;
                            }
                        }

                        for (int i = word.Length - 1; i >= 0; i--)
                        {
                            if (Char.IsDigit(word[i]))
                            {
                                end = i;
                                break;
                            }
                        }

                        if (end > begin)
                        {
                            result = word.Substring(begin, end - begin + 1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        public static void ClearFolder(string folderName)
        {
            try
            {
                if (!Directory.Exists(folderName))
                {
                    return;
                }

                DirectoryInfo dir = new DirectoryInfo(folderName);

                foreach (FileInfo fi in dir.GetFiles())
                {
                    if (fi.CreationTime < DateTime.Now.AddDays(-2))
                    {
                        fi.Delete();
                    }
                }

                foreach (DirectoryInfo di in dir.GetDirectories())
                {
                    ClearFolder(di.FullName);
                    di.Delete();
                }
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);
            }
        }

        public static void AddOrUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);

                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);
            }
        }

        public static byte[] HexStringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public static string Decompress(string compressedString)
        {
            byte[] decompressedBytes;

            var compressedStream = new MemoryStream(Convert.FromBase64String(compressedString));

            using (var decompressorStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
            {
                using (var decompressedStream = new MemoryStream())
                {
                    decompressorStream.CopyTo(decompressedStream);

                    decompressedBytes = decompressedStream.ToArray();
                }
            }

            return Encoding.UTF8.GetString(decompressedBytes);
        }

        public static string Compress(string uncompressedString)
        {
            byte[] compressedBytes;

            using (var uncompressedStream = new MemoryStream(Encoding.UTF8.GetBytes(uncompressedString)))
            {
                using (var compressedStream = new MemoryStream())
                {
                    // setting the leaveOpen parameter to true to ensure that compressedStream will not be closed when compressorStream is disposed
                    // this allows compressorStream to close and flush its buffers to compressedStream and guarantees that compressedStream.ToArray() can be called afterward
                    // although MSDN documentation states that ToArray() can be called on a closed MemoryStream, I don't want to rely on that very odd behavior should it ever change
                    using (var compressorStream = new DeflateStream(compressedStream, CompressionLevel.Optimal, true))
                    {
                        uncompressedStream.CopyTo(compressorStream);
                    }

                    // call compressedStream.ToArray() after the enclosing DeflateStream has closed and flushed its buffer to compressedStream
                    compressedBytes = compressedStream.ToArray();
                }
            }

            return Convert.ToBase64String(compressedBytes);
        }
    }
}
