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
        public DateTime NgayLap { get; set; }
        public List<BaoCaoTinhHinhSuDungHoaDonChiTiet> ChiTiets { get; set; }
    }
}
