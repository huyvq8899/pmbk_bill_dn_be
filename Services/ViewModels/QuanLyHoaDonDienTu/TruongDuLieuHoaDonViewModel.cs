using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels.QuanLyHoaDonDienTu
{
    public class TruongDuLieuHoaDonViewModel
    {
        public string Id { get; set; }
        public int STT { get; set; }
        public string MaTruong { get; set; }
        public string TenTruong { get; set; }
        public string TenTruongHienThi { get; set; }
        public string TenTruongData { get; set; }
        public string GhiChu { get; set; }
        public bool IsChiTiet { get; set; }
        public bool IsMoRong { get; set; }
        public bool IsLeft { get; set; }
        public int Left { get; set; } = 0;
        public bool Status { get; set; }
        public bool Default { get; set; }
        public int Size { get; set; }
        public string Align { get; set; }
        public int DefaultSTT { get; set; }
        public bool DinhDangSo { get; set; }
    }
}
