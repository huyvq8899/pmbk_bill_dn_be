using Services.ViewModels;
using Services.ViewModels.DanhMuc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Repositories.Implimentations.QuanLyHoaDon
{
    public class NhatKyThaoTacHoaDonViewModel : ThongTinChungViewModel
    {
        public string Id { get; set; }
        public DateTime NgayGio { get; set; }
        public string KhachHangId { get; set; }
        public string MoTa { get; set; }
        public DoiTuongViewModel KhachHang { get; set; }
        public string ErrorMessage { get; set; }
        public string HasError { get; set; }
        public int LoaiThaoTac { get; set; }
    }
}
