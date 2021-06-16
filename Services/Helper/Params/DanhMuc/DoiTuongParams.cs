using ManagementServices.Helper;
using Services.ViewModels.DanhMuc;

namespace Services.Helper.Params.DanhMuc
{
    public class DoiTuongParams : PagingParams
    {
        public DoiTuongViewModel Filter { get; set; }
        public int? LoaiKhachHang { get; set; } // 1: Tổ chức, 2: Cá nhân, 3: cả hai
        public int? LoaiDoiTuong { get; set; } // 1: Khách hàng, 2: Nhân viên
    }
}
