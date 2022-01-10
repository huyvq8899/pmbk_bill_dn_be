using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helper
{
    public class LyDoThayTheModel
    {
        public int? HinhThucHoaDonCanThayThe { get; set; }
        public string MauSo { get; set; }
        public string KyHieu { get; set; }
        public string SoHoaDon { get; set; }
        public DateTime NgayHoaDon { get; set; }
        public DateTime? NgayXoaBo { get; set; }
        public int? LoaiHoaDon { get; set; }
        public bool? IsLapVanBanThoaThuan { get; set; }
        public bool? IsSystem { get; set; }

        public override string ToString()
        {
            string day = NgayHoaDon.ToString("dd");
            string month = NgayHoaDon.ToString("MM");
            string year = NgayHoaDon.ToString("yyyy");

            return $"Thay thế cho hóa đơn Mẫu số {MauSo} ký hiệu {KyHieu} số {SoHoaDon} ngày {day} tháng {month} năm {year}";
        }
    }
}
