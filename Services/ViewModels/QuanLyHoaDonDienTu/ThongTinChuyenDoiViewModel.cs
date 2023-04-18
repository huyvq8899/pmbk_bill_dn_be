using Services.ViewModels.DanhMuc;
using System;

namespace Services.ViewModels.QuanLyHoaDonDienTu
{
    public class ThongTinChuyenDoiViewModel
    {
        public string Id { get; set; }
        public string HoaDonDienTuId { get; set; }
        public HoaDonDienTuViewModel HoaDonDienTu { get; set; }
        public DateTime NgayChuyenDoi { get; set; }
        public string NguoiChuyenDoiId { get; set; }
        public DoiTuongViewModel NguoiChuyenDoi { get; set; }
    }
}
