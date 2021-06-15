using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Entity
{
    public class KyKeToan : ThongTinChung
    {
        public string KyKeToanId { get; set; }
        public DateTime? NgayChungTu { get; set; } // ngày của các phiếu phải sau ngày này // từ ngày
        public DateTime? DenNgay { get; set; } // đến ngày
        public string GhiChu { get; set; }
        public bool? DaKhoaSo { get; set; }
        public bool? DaKhoaNhapSoDuBanDau { get; set; }
        public int? LoaiThongTu { get; set; } // 1 TT133, 2 TT200 ..
    }
}
