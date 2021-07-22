using System;

namespace Services.Helper
{
    public class LyDoDieuChinh
    {
        public int? HinhThucHoaDonBiDieuChinh { get; set; }
        public string MauSo { get; set; }
        public string KyHieu { get; set; }
        public string SoHoaDon { get; set; }
        public DateTime NgayHoaDon { get; set; }
        public string LyDo { get; set; }

        public override string ToString()
        {
            string day = NgayHoaDon.ToString("dd");
            string month = NgayHoaDon.ToString("MM");
            string year = NgayHoaDon.ToString("yyyy");
            return $"Điều chỉnh cho hóa đơn số {SoHoaDon}, mẫu số {MauSo}, ký hiệu {KyHieu}, ngày {day} tháng {month} năm {year}";
        }
    }
}
