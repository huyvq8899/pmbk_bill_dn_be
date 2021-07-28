using System;
using System.Collections.Generic;

namespace Services.ViewModels.DanhMuc
{
    public class QuyetDinhApDungHoaDonViewModel : ThongTinChungViewModel
    {
        public string QuyetDinhApDungHoaDonId { get; set; }
        public string NguoiDaiDienPhapLuat { get; set; }
        public string ChucDanh { get; set; }
        public string SoQuyetDinh { get; set; }
        public DateTime? NgayQuyetDinh { get; set; }
        public string CanCuDeBanHanhQuyetDinh { get; set; }

        public bool? HasMayTinh { get; set; }
        public bool? HasMayIn { get; set; }
        public bool? HasChungTuSo { get; set; }

        public string Dieu3 { get; set; }
        public string Dieu4 { get; set; }
        public int? Dieu5 { get; set; } // 1: ngày ký, 2: ngày hiệu lực
        public DateTime? NgayHieuLuc { get; set; }
        public string CoQuanThue { get; set; }

        public string NguoiTao { get; set; }
        public string NgayQuyetDinhFilter { get; set; }
        public string NgayHieuLucFilter { get; set; }

        public List<QuyetDinhApDungHoaDonDieu1ViewModel> QuyetDinhApDungHoaDonDieu1s { get; set; }
        public List<QuyetDinhApDungHoaDonDieu2ViewModel> QuyetDinhApDungHoaDonDieu2s { get; set; }
    }
}
