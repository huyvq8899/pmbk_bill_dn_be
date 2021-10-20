using DLL.Entity.DanhMuc;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Entity.QuanLyHoaDon
{
    public class ThongDiepChiTietGuiCQT
    {
        public string Id { get; set; }
        public string ThongDiepGuiCQTId { get; set; }
        public string HoaDonDienTuId { get; set; }
        public string MaCQTCap { get; set; }
        public string MauHoaDon { get; set; }
        public string KyHieuHoaDon { get; set; }
        public string SoHoaDon { get; set; }
        public DateTime? NgayLapHoaDon { get; set; }
        public byte LoaiApDungHoaDon { get; set; }
        public byte PhanLoaiHDSaiSot { get; set; }
        public string LyDo { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string ModifyBy { get; set; }
    }
}
