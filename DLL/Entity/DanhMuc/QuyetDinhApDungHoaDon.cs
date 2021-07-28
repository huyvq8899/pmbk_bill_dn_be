using System;
using System.Collections.Generic;

namespace DLL.Entity.DanhMuc
{
    public class QuyetDinhApDungHoaDon : ThongTinChung
    {
        public string QuyetDinhApDungHoaDonId { get; set; }
        public string NguoiDaiDienPhapLuat { get; set; }
        public string ChucDanh { get; set; }
        public string SoQuyetDinh { get; set; }
        public DateTime? NgayQuyetDinh { get; set; }
        public string CanCuDeBanHanhQuyetDinh { get; set; }

        public bool? HasMayTinh { get; set; }
        public bool? HasMayIn { get; set; }
        public bool? HasChungThuSo { get; set; }

        public string Dieu1 { get; set; }
        public string Dieu2 { get; set; }
        public string Dieu3 { get; set; }
        public string Dieu4 { get; set; }
        public int? Dieu5 { get; set; } // 1: ngày ký, 2: ngày hiệu lực
        public DateTime? NgayHieuLuc { get; set; }
        public string CoQuanThue { get; set; }

        public List<QuyetDinhApDungHoaDonDieu1> QuyetDinhApDungHoaDonDieu1s { get; set; }
        public List<QuyetDinhApDungHoaDonDieu2> QuyetDinhApDungHoaDonDieu2s { get; set; }
    }
}
