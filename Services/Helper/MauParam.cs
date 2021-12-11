using DLL.Enums;

namespace Services.Helper
{
    public class MauParam
    {
        public string Key { get; set; }
        public string Code { get; set; }
        public string File { get; set; }
        public int? Stt { get; set; }
        public UyNhiemLapHoaDon UyNhiemLapHoaDon { get; set; }
        public HinhThucHoaDon HinhThucHoaDon { get; set; }
        public int? LoaiHoaDon { get; set; }
        public int? LoaiMauHoaDon { get; set; }
        public int? LoaiThueGTGT { get; set; }
        public int? LoaiNgonNgu { get; set; }
        public int? LoaiKhoGiay { get; set; }
    }
}
