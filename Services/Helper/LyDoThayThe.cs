﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helper
{
    public class LyDoThayThe
    {
        public int? HinhThucHoaDonCanThayThe { get; set; }
        public string MauSo { get; set; }
        public string KyHieu { get; set; }
        public string SoHoaDon { get; set; }
        public DateTime NgayHoaDon { get; set; }

        public override string ToString()
        {
            string day = NgayHoaDon.ToString("dd");
            string month = NgayHoaDon.ToString("MM");
            string year = NgayHoaDon.ToString("yyyy");

            return $"Hóa đơn này thay thế hóa đơn số {SoHoaDon}, mẫu số {MauSo}, ký hiệu {KyHieu}, gửi ngày {day} tháng {month} năm {year}";
        }
    }
}