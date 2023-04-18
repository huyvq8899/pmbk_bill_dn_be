using DLL.Enums;
using Services.Helper.LogHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.DanhMuc
{
    public class ThongBaoDieuChinhThongTinHoaDonViewModel : ThongTinChungViewModel
    {
        [IgnoreLogging]
        public string ThongBaoDieuChinhThongTinHoaDonId { get; set; }

        [Display(Name = "Ngày thông báo điều chỉnh")]
        public DateTime? NgayThongBaoDieuChinh { get; set; }

        [Display(Name = "Ngày thông báo phát hành")]
        public DateTime? NgayThongBaoPhatHanh { get; set; }

        [IgnoreLogging]
        public string CoQuanThue { get; set; }

        [Display(Name = "Cơ quan thuế")]
        public string TenCoQuanThue { get; set; }

        [Display(Name = "Tên đơn vị cũ")]
        public string TenDonViCu { get; set; }

        [Display(Name = "Tên đơn vị mới")]
        public string TenDonViMoi { get; set; }

        [Display(Name = "Địa chỉ cũ")]
        public string DiaChiCu { get; set; }

        [Display(Name = "Địa chỉ mới")]
        public string DiaChiMoi { get; set; }

        [Display(Name = "Điện thoại cũ")]
        public string DienThoaiCu { get; set; }

        [Display(Name = "Điện thoại mới")]
        public string DienThoaiMoi { get; set; }

        [Display(Name = "Số")]
        public string So { get; set; }

        [Display(Name = "Trạng thái")]
        public TrangThaiHieuLuc TrangThaiHieuLuc { get; set; }

        [IgnoreLogging]
        public string NoiDungThayDoi { get; set; }

        [IgnoreLogging]
        public string TenTrangThai { get; set; }

        [IgnoreLogging]
        public List<ThongTinThayDoi> ThongTinThayDois { get; set; }

        [IgnoreLogging]
        public List<ThongBaoDieuChinhThongTinHoaDonChiTietViewModel> ThongBaoDieuChinhThongTinHoaDonChiTiets { get; set; }

        [IgnoreLogging]
        public List<TaiLieuDinhKemViewModel> TaiLieuDinhKems { get; set; }

        public List<ThongTinThayDoi> GetThongTinThayDois()
        {
            List<ThongTinThayDoi> result = new List<ThongTinThayDoi>();

            if (!string.IsNullOrEmpty(TenDonViCu) || !string.IsNullOrEmpty(TenDonViMoi))
            {
                result.Add(new ThongTinThayDoi
                {
                    TenThongTin = "Tên đơn vị",
                    ThongTinCu = TenDonViCu,
                    ThongTinMoi = TenDonViMoi
                });
            }

            if (!string.IsNullOrEmpty(DiaChiCu) || !string.IsNullOrEmpty(DiaChiMoi))
            {
                result.Add(new ThongTinThayDoi
                {
                    TenThongTin = "Địa chỉ",
                    ThongTinCu = DiaChiCu,
                    ThongTinMoi = DiaChiMoi
                });
            }

            if (!string.IsNullOrEmpty(DienThoaiCu) || !string.IsNullOrEmpty(DienThoaiMoi))
            {
                result.Add(new ThongTinThayDoi
                {
                    TenThongTin = "Điện thoại",
                    ThongTinCu = DienThoaiCu,
                    ThongTinMoi = DienThoaiMoi
                });
            }

            return result;
        }
    }

    public class ThongTinThayDoi
    {
        public string TenThongTin { get; set; }
        public string ThongTinCu { get; set; }
        public string ThongTinMoi { get; set; }
    }
}
