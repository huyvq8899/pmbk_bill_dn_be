using Services.ViewModels.BaoCao;
using System;
using System.Collections.Generic;

namespace Services.Helper.Params.BaoCao
{
    public class BaoCaoParams
    {
        public DateTime? TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }
        public string Key { get; set; }
        public string Keyword { get; set; }
        public List<SoLuongHoaDonDaPhatHanhViewModel> ListSoLuongHoaDonDaPhatHanhs { get; set; }
        public List<BaoCaoBangKeChiTietHoaDonViewModel> BangKeChiTietHoaDons { get; set; }
        public List<TongHopGiaTriHoaDonDaSuDung> TongHopGiaTriHoaDonDaSuDungs { get; set; }
        public string FilePath { get; set; }
        public bool CongGopTheoHoaDon { get; set; } = false;
        public int? LoaiMau { get; set; } // 1: chuẩn, 2: ngoại tệ
        public string LoaiTienId { get; set; }
        public bool? IsKhongTinhGiaTriHoaDonGoc { get; set; }
        public bool? IsKhongTinhGiaTriHoaDonXoaBo { get; set; }
        public bool? IsKhongTinhGiaTriHoaDonThayThe { get; set; }
        public bool? IsKhongTinhGiaTriHoaDonDieuChinh { get; set; }
        public string KyBaoCao { get; set; }
        public string ThongTinVeLoaiTienVaTrangThai { get; set; }
        public bool? IsPrint { get; set; }
    }
}
