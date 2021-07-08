using DLL.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Entity.DanhMuc
{
    public class ThongBaoKetQuaHuyHoaDon : ThongTinChung
    {
        public string ThongBaoKetQuaHuyHoaDonId { get; set; }
        public string CoQuanThue { get; set; }
        public DateTime? NgayGioHuy { get; set; }
        public string PhuongPhapHuy { get; set; }
        public string TaiLieuDinhKem { get; set; }
        public DateTime? NgayThongBao { get; set; }
        public TrangThaiNop TrangThaiNop { get; set; } // bỏ DaDuocChapNhan

        public List<ThongBaoKetQuaHuyHoaDonChiTiet> ThongBaoKetQuaHuyHoaDonChiTiets { get; set; }
    }
}
