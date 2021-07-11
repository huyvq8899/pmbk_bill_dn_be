using DLL.Enums;

namespace DLL.Entity.DanhMuc
{
    public class TaiLieuDinhKem : ThongTinChung
    {
        public string TaiLieuDinhKemId { get; set; }
        public string NghiepVuId { get; set; }
        public RefType LoaiNghiepVu { get; set; }
        public string TenGoc { get; set; }
        public string TenGuid { get; set; }
    }
}
