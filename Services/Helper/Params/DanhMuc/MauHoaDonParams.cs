using ManagementServices.Helper;
using Services.ViewModels.DanhMuc;

namespace Services.Helper.Params.DanhMuc
{
    public class MauHoaDonParams : PagingParams
    {
        public MauHoaDonViewModel Filter { get; set; }
        public int? LoaiHoaDon { get; set; }
        public int? LoaiMau { get; set; }
        public int? LoaiThueGTGT { get; set; }
        public int? LoaiNgonNgu { get; set; }
        public int? LoaiKhoGiay { get; set; }
    }
}
