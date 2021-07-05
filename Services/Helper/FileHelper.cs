using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace Services.Helper
{
    public static class FileHelper
    {
        public static void ClearFolder(string folderName)
        {
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

        //public static bool ConvertExcelToPDF(string pathExcel, string pathPDF)
        //{
        //    try
        //    {
        //        FileLog.Create($"pathExcel: {pathExcel}");
        //        FileLog.Create($"pathPDF: {pathPDF}");

        //        Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
        //        app.Visible = false;

        //        // Load excel
        //        Microsoft.Office.Interop.Excel.Workbook wkb = app.Workbooks.Open(pathExcel);

        //        // To convert PDF
        //        wkb.ExportAsFixedFormat(Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF, pathPDF);

        //        wkb.Close(false);
        //        app.Quit();

        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        FileLog.Create($"ExceptionConvertExcelToPDF: {e.Message}");
        //        FileLog.Create($"ExceptionConvertExcelToPDFFull: {JsonConvert.SerializeObject(e)}");
        //        throw;
        //    }
        //}

        public static bool ConvertExcelToPDF(string pathRoot, string pathExcel, string pathPDF)
        {
            try
            {
                string encodePathExcel = Convert.ToBase64String(Encoding.UTF8.GetBytes(pathExcel));
                string encodePathPDF = Convert.ToBase64String(Encoding.UTF8.GetBytes(pathPDF));

                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = Path.Combine(pathRoot, $"Tools/ExcelToPdf.exe");
                process.StartInfo.Arguments = $"{encodePathExcel} {encodePathPDF}";
                process.StartInfo.Verb = "runas";
                process.Start();
                process.WaitForExit();

                return true;
            }
            catch (Exception e)
            {
                FileLog.Create($"ExceptionConvertExcelToPDF: {e.Message}");
                FileLog.Create($"ExceptionConvertExcelToPDFFull: {JsonConvert.SerializeObject(e)}");
                throw;
            }
        }
    }
}
