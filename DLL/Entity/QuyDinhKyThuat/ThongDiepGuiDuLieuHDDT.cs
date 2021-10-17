using DLL.Entity.QuanLyHoaDon;
using DLL.Enums;
using System.Collections.Generic;

namespace DLL.Entity.QuyDinhKyThuat
{
    public class ThongDiepGuiDuLieuHDDT : ThongTinChung
    {
        public string ThongDiepGuiDuLieuHDDTId { get; set; }

        // TTChung
        public string PhienBan { get; set; }
        public string MaNoiGui { get; set; }
        public string MaNoiNhan { get; set; }
        public string MaLoaiThongDiep { get; set; }
        public string MaThongDiep { get; set; }
        public string MaThongDiepThamChieu { get; set; }
        public string MaSoThue { get; set; }
        public int SoLuong { get; set; }
        public string FileXMLGui { get; set; }
        public string FileXMLNhan { get; set; }
        public TrangThaiGuiToKhaiDenCQT TrangThaiGui { get; set; }
        public TrangThaiTiepNhanCuaCoQuanThue TrangThaiTiepNhan { get; set; }
        public string HoaDonDienTuId { get; set; }
        ///
        public List<ThongDiepGuiDuLieuHDDTChiTiet> ThongDiepGuiDuLieuHDDTChiTiets { get; set; }
    }
}
