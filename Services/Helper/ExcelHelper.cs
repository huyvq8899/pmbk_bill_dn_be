using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helper
{
    public class ExcelHelper
    {
        public static bool ConvertExcelToPDF(string pathExcel, string pathPDF)
        {
            try
            {
                //Excel.Application app = new Excel.Application();
                //app.Visible = false;

                //// Load excel
                //Excel.Workbook wkb = app.Workbooks.Open(pathExcel);

                //// To convert PDF
                //wkb.ExportAsFixedFormat(Excel.XlFixedFormatType.xlTypePDF, pathPDF);

                //wkb.Close();
                //app.Quit();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
