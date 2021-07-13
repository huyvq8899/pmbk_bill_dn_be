using DLL.Enums;
using System;
using System.Collections.Generic;

namespace Services.ViewModels.DanhMuc
{
    public class ThongBaoKetQuaHuyHoaDonViewModel : ThongTinChungViewModel
    {
        public string ThongBaoKetQuaHuyHoaDonId { get; set; }
        public string CoQuanThue { get; set; }
        public DateTime? NgayGioHuy { get; set; }
        public string PhuongPhapHuy { get; set; }
        public string So { get; set; }
        public DateTime? NgayThongBao { get; set; }
        public TrangThaiNop TrangThaiNop { get; set; } // bỏ DaDuocChapNhan

        public string TenTrangThaiNop { get; set; }

        public List<ThongBaoKetQuaHuyHoaDonChiTietViewModel> ThongBaoKetQuaHuyHoaDonChiTiets { get; set; }
        public List<TaiLieuDinhKemViewModel> TaiLieuDinhKems { get; set; }
    }
}
