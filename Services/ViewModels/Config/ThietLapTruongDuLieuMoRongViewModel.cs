﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels.Config
{
    public class ThietLapTruongDuLieuMoRongViewModel
    {
        public int STT { get; set; }
        public string Id { get; set; }
        public string TenTruong { get; set; }
        public string TenTruongHienThi { get; set; }
        public string GhiChu { get; set; }
        public bool HienThi { get; set; }
        public int LoaiHoaDon { get; set; }
    }
}