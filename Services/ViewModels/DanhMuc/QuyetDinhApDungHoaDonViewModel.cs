using Services.Helper.LogHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.DanhMuc
{
    public class QuyetDinhApDungHoaDonViewModel : ThongTinChungViewModel
    {
        [IgnoreLogging]
        public string QuyetDinhApDungHoaDonId { get; set; }

        [Display(Name = "Người đại diện pháp luật")]
        public string NguoiDaiDienPhapLuat { get; set; }

        [Display(Name = "Chức danh")]
        public string ChucDanh { get; set; }

        [Display(Name = "Số quyết định")]
        public string SoQuyetDinh { get; set; }

        [Display(Name = "Ngày quyết định")]
        public DateTime? NgayQuyetDinh { get; set; }

        [Display(Name = "Căn cứ để ban hành quyết định")]
        public string CanCuDeBanHanhQuyetDinh { get; set; }

        [Display(Name = "Điều 1")]
        public string Dieu1 { get; set; }

        [IgnoreLogging]
        public List<QuyetDinhApDungHoaDonDieu1ViewModel> QuyetDinhApDungHoaDonDieu1s { get; set; }

        [Special]
        [DisplayStatus]
        [Display(Name = "Trạng thái hiển thị")]
        public bool? HasMayTinh { get; set; }

        [Special]
        [DisplayStatus]
        [Display(Name = "Trạng thái hiển thị")]
        public bool? HasMayIn { get; set; }

        [Special]
        [DisplayStatus]
        [Display(Name = "Trạng thái hiển thị")]
        public bool? HasChungThuSo { get; set; }

        [Special]
        public string ThietBi { get; set; } // Cái này phục vụ làm nhật ký truy cập thôi

        [Special]
        public string PhanMemUngDung { get; set; } // Cái này phục vụ làm nhật ký truy cập thôi

        [Display(Name = "Điều 2")]
        public string Dieu2 { get; set; }

        [Display(Name = "Điều 3")]
        public string Dieu3 { get; set; }

        [Display(Name = "Điều 3")]
        public string NoiDungDieu3 { get; set; }

        [Display(Name = "Điều 4")]
        public string Dieu4 { get; set; }

        [Display(Name = "Điều 4")]
        public string NoiDungDieu4 { get; set; }

        [IgnoreLogging]
        public int? Dieu5 { get; set; } // 1: ngày ký, 2: ngày hiệu lực

        [Display(Name = "Quyết định này có hiệu lực thi hành kể từ")]
        public string NoiDungDieu5 { get; set; }

        [Display(Name = "Ngày hiệu lực")]
        public DateTime? NgayHieuLuc { get; set; }

        [IgnoreLogging]
        public string CoQuanThue { get; set; }

        [Display(Name = "Cơ quan thuế")]
        public string TenCoQuanThue { get; set; }

        [IgnoreLogging]
        public string NguoiTao { get; set; }

        [IgnoreLogging]
        public string NgayQuyetDinhFilter { get; set; }

        [IgnoreLogging]
        public string NgayHieuLucFilter { get; set; }

        [IgnoreLogging]
        public List<QuyetDinhApDungHoaDonDieu2ViewModel> QuyetDinhApDungHoaDonDieu2s { get; set; }

        [IgnoreLogging]
        public List<TaiLieuDinhKemViewModel> TaiLieuDinhKems { get; set; }
    }
}
