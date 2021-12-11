using Services.Helper.Params.HeThong;
using Services.ViewModels.Import;
using System.Collections.Generic;

namespace Services.Helper.Params.HoaDon
{
    public class NhapKhauResult
    {
        public List<HoaDonDienTuImport> ListResult { get; set; }
        public List<TruongDLHDExcel> ListTruongDuLieu { get; set; }
    }
}
