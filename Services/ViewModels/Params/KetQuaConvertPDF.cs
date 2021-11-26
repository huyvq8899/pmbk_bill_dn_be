using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels.Params
{
    public class KetQuaConvertPDF
    {
        public string FilePDF { get; set; }
        public string FileXML { get; set; }
        public string PdfName { get; set; }
        public string XMLName { get; set; }
        public string XMLBase64 { get; set; }
        public string PDFBase64 { get; set; }
    }
}
