using DLL.Enums;
using System;
using System.Collections.Generic;

namespace Services.ViewModels.DanhMuc
{
    public class MauHoaDonViewModel : ThongTinChungViewModel
    {
        public string MauHoaDonId { get; set; }
        public string Ten { get; set; }
        public int? SoThuTu { get; set; }
        public string MauSo { get; set; }
        public string KyHieu { get; set; }
        public string TenBoMau { get; set; }
        public DateTime? NgayKy { get; set; }
        public QuyDinhApDung QuyDinhApDung { get; set; }
        public UyNhiemLapHoaDon UyNhiemLapHoaDon { get; set; }
        public HinhThucHoaDon HinhThucHoaDon { get; set; }
        public LoaiHoaDon LoaiHoaDon { get; set; }
        public LoaiMauHoaDon LoaiMauHoaDon { get; set; }
        public LoaiThueGTGT LoaiThueGTGT { get; set; }
        public LoaiNgonNgu LoaiNgonNgu { get; set; }
        public LoaiKhoGiay LoaiKhoGiay { get; set; }

        public bool? Active { get; set; }

        public string Username { get; set; }
        public string TenQuyDinhApDung { get; set; }
        public string TenLoaiHoaDon { get; set; }
        public string TenHinhThucHoaDon { get; set; }
        public string TenUyNhiemLapHoaDon { get; set; }
        public bool? IsDaThongBaoPhatHanh { get; set; }
        public string TenTrangThaiTBPH { get; set; }
        public string NgayCapNhatFilter { get; set; }

        public List<string> KyHieus { get; set; }
        public List<string> MauHoaDonIds { get; set; }
        public List<ThongTinChiTietKetQuaHuy> ThongTinChiTiets { get; set; }
        public List<MauHoaDonThietLapMacDinhViewModel> MauHoaDonThietLapMacDinhs { get; set; }
        public List<MauHoaDonTuyChinhChiTietViewModel> MauHoaDonTuyChinhChiTiets { get; set; }
    }

    public class ThongTinChiTietKetQuaHuy
    {
        public string KyHieu { get; set; }
        public int? TuSo { get; set; }
    }
}
