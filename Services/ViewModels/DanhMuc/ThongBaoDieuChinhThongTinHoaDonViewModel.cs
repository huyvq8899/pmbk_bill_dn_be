using DLL.Enums;
using System;
using System.Collections.Generic;

namespace Services.ViewModels.DanhMuc
{
    public class ThongBaoDieuChinhThongTinHoaDonViewModel : ThongTinChungViewModel
    {
        public string ThongBaoDieuChinhThongTinHoaDonId { get; set; }
        public DateTime? NgayThongBaoDieuChinh { get; set; }
        public DateTime? NgayThongBaoPhatHanh { get; set; }
        public string CoQuanThue { get; set; }
        public string So { get; set; }
        public TrangThaiNop TrangThaiNop { get; set; } // bỏ DaDuocChapNhan

        public bool? IsCheckedTenDonVi { get; set; }
        public bool? IsCheckedDiaChi { get; set; }
        public bool? IsCheckedDienThoai { get; set; }
        public string TenDonViCu { get; set; }
        public string TenDonViMoi { get; set; }
        public string DiaChiCu { get; set; }
        public string DiaChiMoi { get; set; }
        public string DienThoaiCu { get; set; }
        public string DienThoaiMoi { get; set; }

        public List<ThongBaoDieuChinhThongTinHoaDonChiTietViewModel> ThongBaoDieuChinhThongTinHoaDonChiTiets { get; set; }
    }
}
