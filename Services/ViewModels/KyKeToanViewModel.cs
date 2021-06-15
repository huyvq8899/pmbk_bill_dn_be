using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels
{
    public class KyKeToanViewModel: ThongTinChungViewModel
    {
        public KyKeToanViewModel()
        {
            this.Status = true;
        }
        public string KyKeToanId { get; set; }
        public DateTime? NgayChungTu { get; set; } // ngày của các phiếu phải sau ngày này
        public DateTime? DenNgay { get; set; } // đến ngày
        public string GhiChu { get; set; }

        public string NgayChungTuString { get; set; } // ngày của các phiếu phải sau ngày này
        public string DenNgayString { get; set; } // đến ngày
        public bool? DaKhoaSo { get; set; }
        public bool? DaKhoaNhapSoDuBanDau { get; set; }
        public int? LoaiThongTu { get; set; } // 1 TT133, 2 TT200 ..
    }
}
