using DLL.Entity.DanhMuc;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Entity.QuanLyHoaDon
{
    public class ThongDiepGuiCQT
    {
        public string Id { get; set; }
        public string MaThongDiep { get; set; }
        public DateTime NgayGui { get; set; }
        public DateTime NgayLap { get; set; }
        public string FileDinhKem { get; set; }
        public string NguoiNopThue { get; set; }
        public string DiaDanh { get; set; }
        public string FileXMLDaKy { get; set; }
        public string ThongBaoHoaDonRaSoatId { get; set; }
        public bool? DaKyGuiCQT { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string ModifyBy { get; set; }
    }
}
