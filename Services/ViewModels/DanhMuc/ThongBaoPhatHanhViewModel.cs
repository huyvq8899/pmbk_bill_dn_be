using DLL.Enums;
using Services.Helper.LogHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.DanhMuc
{
    public class ThongBaoPhatHanhViewModel : ThongTinChungViewModel
    {
        [IgnoreLogging]
        public string ThongBaoPhatHanhId { get; set; }

        [Display(Name = "Điện thoại")]
        public string DienThoai { get; set; }

        [IgnoreLogging]
        public string CoQuanThue { get; set; }

        [Display(Name = "Cơ quan thuế")]
        public string TenCoQuanThue { get; set; }

        [Display(Name = "Người đại diện")]
        public string NguoiDaiDienPhapLuat { get; set; }

        [Display(Name = "Ngày")]
        public DateTime Ngay { get; set; }

        [Display(Name = "Số")]
        public string So { get; set; }

        [Display(Name = "Trạng thái")]
        public TrangThaiNop TrangThaiNop { get; set; }

        [CheckBox]
        [Display(Name = "Xác nhận")]
        public bool? IsXacNhan { get; set; }

        [IgnoreLogging]
        public string TenTrangThaiNop { get; set; }

        [IgnoreLogging]
        public List<ThongBaoPhatHanhChiTietViewModel> ThongBaoPhatHanhChiTiets { get; set; }
    }
}
