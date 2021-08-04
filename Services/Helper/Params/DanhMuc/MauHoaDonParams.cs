using ManagementServices.Helper;
using Services.ViewModels.DanhMuc;
using System.Collections.Generic;

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
        public List<string> MauHoaDonDuocPQ { get; set; }
    }

    public class ExportMauHoaDonParams
    {
        public string MauHoaDonId { get; set; }
        public List<HinhThucMauHoaDon> HinhThucMauHoaDon { get; set; }
        public DinhDangTepMau DinhDangTepMau { get; set; }
    }
}
