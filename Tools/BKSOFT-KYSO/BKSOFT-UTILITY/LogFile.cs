using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace BKSOFT.UTILITY
{
    public class LogFile
    {
        /// <summary>
        /// Newline character
        /// </summary>
        public const string Newline = "\n";

        /// <summary>
        /// Slash character: "/"
        /// </summary>
        public const string Slash = @"/";

        /// <summary>
        /// File log
        /// </summary>
        public const string FolderLog = "log";

        /// <summary>
        /// Max size M of file log
        /// </summary>
        public const int MaxFileSize = 16;

        /// <summary>
        /// The directory log path
        /// </summary>
        public static string DirLogPath { set; get; }

        /// <summary>
        /// The file log path
        /// </summary>
        public static string FileLogPath { set; get; }

        private LogFile()
        {
            try
            {
                // Directory store log
                DirLogPath = String.Format("{0}\\{1}", AppDomain.CurrentDomain.BaseDirectory, FolderLog);

                // Check folder exist
                if (!Directory.Exists(DirLogPath))
                {
                    // Create directory 
                    Directory.CreateDirectory(DirLogPath);
                }

                // Check exist file log
                FileLogPath = String.Format("{0}\\{1}.log", DirLogPath, DateTime.Now.ToString("ddMMyy_HHmmss"));
            }
            catch (Exception)
            {
            }
        }

        private static void WriteBuffer(StringBuilder sbLogSource)
        {
            try
            {
                // Directory store log
                if (string.IsNullOrEmpty(DirLogPath))
                {
                    DirLogPath = String.Format("{0}\\{1}", AppDomain.CurrentDomain.BaseDirectory, FolderLog);
                }

                // Check folder exist
                if (!Directory.Exists(DirLogPath))
                {
                    // Create directory 
                    Directory.CreateDirectory(DirLogPath);
                }

                // Check exist file log
                if (string.IsNullOrEmpty(FileLogPath))
                {
                    FileLogPath = String.Format("{0}\\{1}.log", DirLogPath, DateTime.Now.ToString("ddMMyy_HHmmss"));
                }

                // Check current file exist
                if (!File.Exists(FileLogPath))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(FileLogPath))
                    {
                        sw.WriteLine("{0}", DateTime.Now.ToString("dd-MM-yy HH:mm:ss"));
                        sw.WriteLine(sbLogSource.ToString());
                    }

                    return;
                }

                // Check file size current
                FileInfo file = new FileInfo(FileLogPath);
                if (file.Length > MaxFileSize * 1024 * 1024)      // Check file size > MaxFileSize Mb
                {
                    FileLogPath = String.Format("{0}\\{1}.log", DirLogPath, DateTime.Now.ToString("ddMMyy_HHmmss"));
                    using (StreamWriter sw = File.CreateText(FileLogPath)) { }
                }

                // Write buffer
                using (StreamWriter sw = File.AppendText(FileLogPath))
                {
                    sw.WriteLine("{0}", DateTime.Now.ToString("dd-MM-yy HH:mm:ss"));
                    sw.WriteLine(sbLogSource.ToString());
                }
            }
            catch (Exception)
            {
            }
        }

        public static void WriteLog(string logMessage)
        {
            try
            {
                // Genarate LogSource
                StringBuilder sbLogSource = new StringBuilder();

                // Detail exception name
                if (!string.IsNullOrEmpty(logMessage))
                {
                    sbLogSource.Append(logMessage + Newline);
                }

                // Write buffer to log file
                WriteBuffer(sbLogSource);
            }
            catch
            {
            }
        }

        public static void WriteLog(string logMessage, Exception exception)
        {
            try
            {
                MethodBase methodBase = new StackTrace(true).GetFrame(1).GetMethod();

                // Genarate LogSource
                StringBuilder sbLogSource = new StringBuilder();

                // Get Module's name
                sbLogSource.Append(methodBase.DeclaringType.Assembly.GetName().Name + Slash);

                // Get Class name
                sbLogSource.Append(methodBase.ReflectedType.Name + Slash);

                // Get Method's name
                sbLogSource.Append(methodBase.Name + Newline);

                // Detail exception name
                if (!string.IsNullOrEmpty(logMessage))
                {
                    sbLogSource.Append(logMessage + Newline);
                }
                sbLogSource.Append("Exception:  " + exception.ToString() + Newline);

                // Write buffer to log file
                WriteBuffer(sbLogSource);
            }
            catch
            {
            }
        }

        public static void WriteLog(byte[] buffer, Exception exception)
        {
            try
            {
                MethodBase methodBase = new StackTrace(true).GetFrame(1).GetMethod();

                // Genarate LogSource
                StringBuilder sbLogSource = new StringBuilder();

                // Get Module's name
                sbLogSource.Append(methodBase.DeclaringType.Assembly.GetName().Name + Slash);

                // Get Class name
                sbLogSource.Append(methodBase.ReflectedType.Name + Slash);

                // Get Method's name
                sbLogSource.Append(methodBase.Name + Newline);

                if (buffer == null)
                {
                    sbLogSource.Append("BUFFER IS NULL" + Newline);
                }
                else if (buffer.Length == 0)
                {
                    sbLogSource.Append("BUFFER LENGTH ZERO" + Newline);
                }
                else
                {
                    for (int idx = 0; idx < buffer.Length; idx++)
                    {
                        sbLogSource.Append(String.Format("{0:x2} ", buffer[idx]));
                    }
                    sbLogSource.Append(Newline);
                }

                sbLogSource.Append("Exception:  " + exception.ToString() + Newline);

                // Write buffer to log file
                WriteBuffer(sbLogSource);
            }
            catch
            {
            }
        }

        public static void WriteLog(byte[] buffer)
        {
            try
            {
                MethodBase methodBase = new StackTrace(true).GetFrame(1).GetMethod();

                // Genarate LogSource
                StringBuilder sbLogSource = new StringBuilder();

                // Get Module's name
                sbLogSource.Append(methodBase.DeclaringType.Assembly.GetName().Name + Slash);

                // Get Class name
                sbLogSource.Append(methodBase.ReflectedType.Name + Slash);

                // Get Method's name
                sbLogSource.Append(methodBase.Name + Newline);

                if (buffer == null)
                {
                    sbLogSource.Append("BUFFER IS NULL" + Newline);
                }
                else if (buffer.Length == 0)
                {
                    sbLogSource.Append("BUFFER LENGTH ZERO" + Newline);
                }
                else
                {
                    for (int idx = 0; idx < buffer.Length; idx++)
                    {
                        sbLogSource.Append(String.Format("{0:x2} ", buffer[idx]));
                    }
                    sbLogSource.Append(Newline);
                }

                // Write buffer to log file
                WriteBuffer(sbLogSource);
            }
            catch
            {
            }
        }
    }
}
