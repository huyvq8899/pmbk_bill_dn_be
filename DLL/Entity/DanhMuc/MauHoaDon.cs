using DLL.Entity.QuanLy;
using DLL.Enums;
using System;
using System.Collections.Generic;

namespace DLL.Entity.DanhMuc
{
    public class MauHoaDon : ThongTinChung
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

        public List<ThongBaoPhatHanhChiTiet> ThongBaoPhatHanhChiTiets { get; set; }
        public List<ThongBaoKetQuaHuyHoaDonChiTiet> ThongBaoKetQuaHuyHoaDonChiTiets { get; set; }
        public List<ThongBaoDieuChinhThongTinHoaDonChiTiet> ThongBaoDieuChinhThongTinHoaDonChiTiets { get; set; }
        public List<MauHoaDonThietLapMacDinh> MauHoaDonThietLapMacDinhs { get; set; }
        public List<MauHoaDonTuyChinhChiTiet> MauHoaDonTuyChinhChiTiets { get; set; }
        public List<BoKyHieuHoaDon> BoKyHieuHoaDons { get; set; }
    }
}
