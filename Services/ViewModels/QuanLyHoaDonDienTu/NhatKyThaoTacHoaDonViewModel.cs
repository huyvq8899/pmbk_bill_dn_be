using Services.ViewModels;
using Services.ViewModels.DanhMuc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels.QuanLyHoaDonDienTu
{
    public class NhatKyThaoTacHoaDonViewModel : ThongTinChungViewModel
    {
        public string HoaDonDienTuId { get; set; }
        public HoaDonDienTuViewModel HoaDonDienTu { get; set; }
        public string Id { get; set; }
        public DateTime NgayGio { get; set; }
        public string KhachHangId { get; set; }
        public string MoTa { get; set; }
        public DoiTuongViewModel KhachHang { get; set; }
        public string ErrorMessage { get; set; }
        public bool HasError { get; set; }
        public int LoaiThaoTac { get; set; }
        public string HanhDong { get; set; }
        public string NguoiThucHienId { get; set; }
        public UserViewModel NguoiThucHien { get; set; }
        public string DiaChiIp { get; set; }
    }
}
