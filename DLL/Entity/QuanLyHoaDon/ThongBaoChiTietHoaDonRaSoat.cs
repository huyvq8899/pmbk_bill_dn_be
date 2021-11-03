using DLL.Entity.DanhMuc;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Entity.QuanLyHoaDon
{
    public class ThongBaoChiTietHoaDonRaSoat
    {
        public string Id { get; set; }
        public string ThongBaoHoaDonRaSoatId { get; set; }

        public string MauHoaDon { get; set; }
        public string KyHieuHoaDon { get; set; }

        public string SoHoaDon { get; set; }

        public DateTime? NgayLapHoaDon { get; set; }

        public byte LoaiApDungHD { get; set; }

        public string LyDoRaSoat { get; set; }

        public bool? DaGuiThongBao { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string ModifyBy { get; set; }
    }
}
