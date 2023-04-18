using DLL.Enums;

namespace DLL.Entity.TienIch
{
    public class NhatKyTruyCap : ThongTinChung
    {
        public string NhatKyTruyCapId { get; set; }
        public string DoiTuongThaoTac { get; set; }
        public string HanhDong { get; set; }
        public string ThamChieu { get; set; }
        public string MoTaChiTiet { get; set; }
        public string DiaChiIP { get; set; }
        public string TenMayTinh { get; set; }
        public string RefFile { get; set; }
        public string RefId { get; set; }
        public RefType RefType { get; set; }
    }
}
