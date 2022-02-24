using Spire.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            catch (Exception)
            {
                return false;
            }
        }

        public static void DeleteFile(string path)
        {
            try
            {
                if (File.Exists(path))           // If file exist to delete
                {
                    File.Delete(path);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool MergePDF(List<string> files, string path)
        {
            bool res = false;
            try
            {
                using (PdfDocumentBase doc = PdfDocument.MergeFiles(files.ToArray()))
                {
                    doc.Save(path, FileFormat.PDF);
                    doc.Dispose();
                    doc.Close();
                    res = true;
                }
            }
            catch (Exception)
            {
            }

            return res;
        }

        //public static string MergePDF(string[] fileArray, string outputPdfPath)
        //{
        //    var fileName = Guid.NewGuid().ToString() + ".pdf";
        //    try
        //    {
        //        PdfReader reader = null;
        //        Document sourceDocument = null;
        //        PdfCopy pdfCopyProvider = null;
        //        PdfImportedPage importedPage;

        //        if (!Directory.Exists(outputPdfPath))
        //        {
        //            Directory.CreateDirectory(outputPdfPath);
        //        }

        //        var filePath = Path.Combine(outputPdfPath, fileName);
        //        sourceDocument = new Document();
        //        pdfCopyProvider = new PdfCopy(sourceDocument, new System.IO.FileStream(filePath, System.IO.FileMode.Create));

        //        //output file Open  
        //        sourceDocument.Open();


        //        //files list wise Loop  
        //        for (int f = 0; f < fileArray.Length; f++)
        //        { 
        //            reader = new PdfReader(fileArray[f]);

        //            int pages = reader.NumberOfPages;
        //            //Add pages in new file  
        //            for (int i = 1; i <= pages; i++)
        //            {
        //                importedPage = pdfCopyProvider.GetImportedPage(reader, i);
        //                pdfCopyProvider.AddPage(importedPage);
        //            }

        //            reader.Close();
        //        }
        //        //save the output file  
        //        sourceDocument.Close();
        //    }
        //    catch(Exception ex)
        //    {
        //        FileLog.WriteLog(ex.Message);
        //        fileName = string.Empty;
        //    }
        //    return fileName;
        //}

        //private static int TotalPageCount(string file)
        //{
        //    try
        //    {
        //        using (StreamReader sr = new StreamReader(System.IO.File.OpenRead(file)))
        //        {
        //            Regex regex = new Regex(@"/Type\s*/Page[^s]");
        //            MatchCollection matches = regex.Matches(sr.ReadToEnd());

        //            return matches.Count;
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        FileLog.WriteLog(ex.Message);
        //    }

        //    return 0;
        //}

        /// <summary>
        /// Quét và tìm file để xóa
        /// </summary>
        /// <param name="targetDirectory"></param>
        /// <param name="fileToDelete"></param>
        public static void ScanToDeleteFile(string targetDirectory, string fileToDelete)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory).Where(x => x.Contains(fileToDelete)).ToArray();
            foreach (string fileName in fileEntries)
            {
                File.Delete(fileName);
            }

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
            {
                ScanToDeleteFile(subdirectory, fileToDelete);
            }
        }

        /// <summary>
        /// Quét và tìm các file để xóa
        /// </summary>
        /// <param name="targetDirectory"></param>
        /// <param name="fileToDelete"></param>
        public static void ScanToDeleteFiles(string targetDirectory, List<string> filesToDelete)
        {
            // Process the list of files found in the directory.
            var test = Directory.GetFiles(targetDirectory);
            if (filesToDelete.Any())
            {
                string[] fileEntries = Directory.GetFiles(targetDirectory).Where(x => filesToDelete.Any(y => x.Contains(y))).ToArray();
                foreach (string fileName in fileEntries)
                {
                    File.Delete(fileName);
                }

                // Recurse into subdirectories of this directory.
                string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
                foreach (string subdirectory in subdirectoryEntries)
                {
                    ScanToDeleteFiles(subdirectory, filesToDelete);
                }
            }
            else return;
        }
    }
}
