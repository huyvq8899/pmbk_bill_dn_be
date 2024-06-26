﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels.BaoCao
{
    public class TruongDuLieuViewModel
    {
        public string Id { get; set; }
        public int STT { get; set; }
        public string MaTruong { get; set; }
        public string TenTruong { get; set; }
        public string TenHienThi { get; set; }
        public bool Status { get; set; }
        public bool Default { get; set; }
        public bool HienThiKhiCongGop { get; set; }
        public string NghiepVuId { get; set; }
        public int Size { get; set; }
        public string Align { get; set; }
        public int DefaultSTT { get; set; }
        public bool DinhDangSo { get; set; }
        public NghiepVuViewModel NghiepVu { get; set; }
    }
}
