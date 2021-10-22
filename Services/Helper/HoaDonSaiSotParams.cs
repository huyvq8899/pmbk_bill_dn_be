using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helper
{
    public class HoaDonSaiSotParams
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string UyNhiemLapHoaDon { get; set; }
        public byte LoaiHoaDon { get; set; }
        public string HinhThucHoaDon { get; set; }
        public string KyHieuHoaDon { get; set; }
    }

    public class FileXMLThongDiepGuiParams
    {
        public string DataXML { get; set; }
    }
}
