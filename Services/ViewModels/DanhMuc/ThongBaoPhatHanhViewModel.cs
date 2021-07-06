using DLL.Enums;
using System;
using System.Collections.Generic;

namespace Services.ViewModels.DanhMuc
{
    public class ThongBaoPhatHanhViewModel : ThongTinChungViewModel
    {
        public string ThongBaoPhatHanhId { get; set; }
        public string DienThoai { get; set; }
        public string CoQuanThue { get; set; }
        public string NguoiDaiDienPhapLuat { get; set; }
        public DateTime Ngay { get; set; }
        public string So { get; set; }
        public TrangThaiNop TrangThaiNop { get; set; }
        public bool? IsXacNhan { get; set; }

        public string TenTrangThaiNop { get; set; }

        public List<ThongBaoPhatHanhChiTietViewModel> ThongBaoPhatHanhChiTiets { get; set; }
    }
}
