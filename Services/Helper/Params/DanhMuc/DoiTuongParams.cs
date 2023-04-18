using ManagementServices.Helper;
using Services.ViewModels.DanhMuc;

namespace Services.Helper.Params.DanhMuc
{
    public class DoiTuongParams : PagingParams
    {
        public DoiTuongViewModel Filter { get; set; }
        public int? LoaiKhachHang { get; set; } // 1: Cá nhân, 2: Tổ chức, 3: cả hai
        public int? LoaiDoiTuong { get; set; } // 1: Khách hàng, 2: Nhân viên
        public string Ma { get; set; }
        public bool? IsSearchKhachHangDeTrong { get; set; }

        public DoiTuongSearch TimKiemTheo { get; set; }

    }
}
