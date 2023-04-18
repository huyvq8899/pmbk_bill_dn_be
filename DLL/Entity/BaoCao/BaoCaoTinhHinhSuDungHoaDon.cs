using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Entity.BaoCao
{
    public class BaoCaoTinhHinhSuDungHoaDon
    {
        public string BaoCaoTinhHinhSuDungHoaDonId { get; set; }
        public int Nam { get; set; }
        public int? Thang { get; set; }
        public int? Quy { get; set; }
        public string DienGiai { get; set; }
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
        public DateTime NgayLap { get; set; }
        public string NguoiLapId { get; set; }
        public string TenNguoiLap { get; set; }
        public string TenNguoiDaiDienPhapLuat { get; set; }
        public List<BaoCaoTinhHinhSuDungHoaDonChiTiet> ChiTiets { get; set; }
    }
}
