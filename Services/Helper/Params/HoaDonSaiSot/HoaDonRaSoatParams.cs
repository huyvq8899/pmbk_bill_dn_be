using Services.Helper.Params.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helper.HoaDonSaiSot
{
    public class HoaDonRaSoatParams
    {
        public string ThongBaoHoaDonRaSoatId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int HinhThucHoaDon { get; set; }
        public int? LoaiSaiSot { get; set; }

        public List<FilterColumn> FilterColumns { get; set; }
        public string SortKey { get; set; }
        public string SortValue { get; set; }

        public ThongBaoRaSoatSearch TimKiemTheo { get; set; }
        public string TimKiemBatKy { get; set; }
    }

    public class ThongBaoRaSoatSearch
    {
        public string SoThongBao { get; set; }
        public string NgayThongBao { get; set; }
    }
}
