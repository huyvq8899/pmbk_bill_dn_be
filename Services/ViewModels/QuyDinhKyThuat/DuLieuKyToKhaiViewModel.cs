using System;
using System.Collections.Generic;
using System.Text;
using TDiep103 = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._11.TDiep;
using TBao103 = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._4.TBao;

namespace Services.ViewModels.QuyDinhKyThuat
{
    public class DuLieuKyToKhaiViewModel
    {
        public string Id { get; set; }
        public string IdToKhai { get; set; }
        public string FileXMLDaKy { get; set; }
        public string Content { get; set; }
        public DateTime NgayKy { get; set; }
        public string MST { get; set; }
        public string Seri { get; set; }
    }

    public class DuLieuFormFile103 {
        public TBao103 TBao103 { get; set; }
        public TDiep103 TDiep103 { get; set; }
    }
}
