using DLL.Enums;
using Services.Enums;

namespace Services.ViewModels.TienIch
{
    public class NhatKyTruyCapViewModel : ThongTinChungViewModel
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

        public string CreatedDateFilter { get; set; }
        public string CreatedByUserName { get; set; }
        public LoaiHanhDong LoaiHanhDong { get; set; }
        public object DuLieuCu { get; set; }
        public object DuLieuMoi { get; set; }
        public string ClassName { get; set; }
    }
}
