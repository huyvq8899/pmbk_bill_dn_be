using Ionic.Zip;
using KYSO_UPDATE;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;

namespace BKSOFT_KYSO_UPDATE
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args == null || args.Length != 4)
                {
                    return;
                }

                // Process name
                string processName = Utils.Base64Decode(args[0]);
                string path = Utils.Base64Decode(args[1]);
                string pathZip = Path.Combine(path, Utils.Base64Decode(args[2]));
                string json = Utils.Base64Decode(args[3]);

                // Check file exist
                if (!File.Exists(pathZip))
                {
                    return;
                }


                // Get process with name
                Process[] pros = Process.GetProcessesByName(processName);
                foreach (var pro in pros)
                {
                    pro.Kill();
                    pro.WaitForExit();

                    //System.Diagnostics.Process process = new System.Diagnostics.Process();
                    //process.StartInfo.FileName = @"C:\windows\system32\cmd.exe";
                    //process.StartInfo.Arguments = $"/c taskkill /PID {pro.Id} /F";
                    //process.Start();
                    //process.WaitForExit();
                }

                // Uzip to update
                if (UnZip(pathZip, path))
                {
                    // Update version
                    Setting setting = GetCurrentVer();
                    if(setting != null)
                    {
                        VersionInfo ver = JsonConvert.DeserializeObject<VersionInfo>(json);
                        setting.Version = ver.Version;
                        setting.Date = ver.Date;

                        string pathVer = $"{path}\\ver.dat";
                        File.WriteAllText(pathVer, JsonConvert.SerializeObject(setting));
                    }    

                    // Re-Open application with
                    Process.Start($"{path}{processName}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        static bool UnZip(string sourceZipFile, string unZipToPath)
        {
            bool res = true;
            try
            {
                using (ZipFile zipFile = ZipFile.Read(sourceZipFile))
                {
                    zipFile.ExtractAll(unZipToPath, ExtractExistingFileAction.OverwriteSilently);
                }
                File.Delete(sourceZipFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                res = false;
            }

            return res;
        }

        static Setting GetCurrentVer()
        {
            Setting setting = null;

            try
            {
                string path = $"{AppDomain.CurrentDomain.BaseDirectory}ver.dat";

                // Check exit
                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    setting = JsonConvert.DeserializeObject<Setting>(json);
                }
            }
            catch (Exception)
            {
            }

            return setting;
        }
    }
}
