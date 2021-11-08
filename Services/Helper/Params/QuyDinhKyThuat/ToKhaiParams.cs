using DLL.Enums;
using ManagementServices.Helper;

namespace Services.Helper.Params.QuyDinhKyThuat
{
    public class ToKhaiParams : PagingParams
    {
        public ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1.TKhai ToKhaiKhongUyNhiem { get; set; }
        public ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._2.TKhai ToKhaiUyNhiem { get; set; }
        public UyNhiemLapHoaDon UyNhiemLapHoaDon { get; set; }
        public int kyHieuMauSoHoaDon { get; set; }
        public string KyHieuHoaDon { get; set; }
        public HinhThucHoaDon HinhThucHoaDon { get; set; }
        public LoaiHoaDon LoaiHoaDon { get; set; }
    }
}
