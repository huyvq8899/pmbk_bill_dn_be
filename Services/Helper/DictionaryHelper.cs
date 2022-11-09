using System.IO;

namespace Services.Helper
{
    public static class DictionaryHelper
    {
        public static void ClearAttributes(string currentDir)
        {
            if (Directory.Exists(currentDir))
            {
                File.SetAttributes(currentDir, FileAttributes.Normal);

                string[] subDirs = Directory.GetDirectories(currentDir);
                foreach (string dir in subDirs)
                {
                    ClearAttributes(dir);
                }

                string[] files = Directory.GetFiles(currentDir);
                foreach (string file in files)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                }
            }
        }
    }
}
