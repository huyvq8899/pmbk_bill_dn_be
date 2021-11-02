using DLL.Entity.DanhMuc;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Entity.QuanLyHoaDon
{
    public class ThongBaoHoaDonRaSoat
    {
        public string Id { get; set; }
        public string SoThongBaoCuaCQT { get; set; }
        public DateTime NgayThongBao { get; set; }

        public string TenCQTCapTren { get; set; }
        public string TenCQTRaThongBao { get; set; }

        public string TenNguoiNopThue { get; set; }

        public string MaSoThue { get; set; }

        public byte ThoiHan { get; set; }

        public byte Lan { get; set; }

        public string HinhThuc { get; set; }

        public string ChucDanh { get; set; }

        public string FileDinhKem { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string ModifyBy { get; set; }
    }
}
