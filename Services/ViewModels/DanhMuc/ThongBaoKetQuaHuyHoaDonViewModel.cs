using DLL.Enums;
using Services.Helper.LogHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.DanhMuc
{
    public class ThongBaoKetQuaHuyHoaDonViewModel : ThongTinChungViewModel
    {
        [IgnoreLogging]
        public string ThongBaoKetQuaHuyHoaDonId { get; set; }

        [IgnoreLogging]
        public string CoQuanThue { get; set; }

        [Display(Name = "Cơ quan thuế")]
        public string TenCoQuanThue { get; set; }

        public DateTime? NgayGioHuy { get; set; }

        [Display(Name = "Phương pháp hủy")]
        public string PhuongPhapHuy { get; set; }

        [Display(Name = "Số thông báo")]
        public string So { get; set; }

        [Display(Name = "Ngày thông báo")]
        public DateTime? NgayThongBao { get; set; }

        [Display(Name = "Trạng thái")]
        public TrangThaiNop TrangThaiNop { get; set; } // bỏ DaDuocChapNhan

        [IgnoreLogging]
        public string TenTrangThaiNop { get; set; }

        [IgnoreLogging]
        public List<ThongBaoKetQuaHuyHoaDonChiTietViewModel> ThongBaoKetQuaHuyHoaDonChiTiets { get; set; }

        [IgnoreLogging]
        public List<TaiLieuDinhKemViewModel> TaiLieuDinhKems { get; set; }
    }
}
