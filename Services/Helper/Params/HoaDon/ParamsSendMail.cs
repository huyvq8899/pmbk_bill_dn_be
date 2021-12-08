using Services.ViewModels.QuanLyHoaDonDienTu;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helper.Params.HoaDon
{
    public class ParamsSendMail
    {
        public HoaDonDienTuViewModel HoaDon { get; set; }
        public string ToMail { get; set; }
        public string TenNguoiNhan { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string SoDienThoaiNguoiNhan { get; set; }
        public int? LoaiEmail { get; set; }
        public string Link { get; set; }
        public string LinkTraCuu { get; set; }
        public string BienBanDieuChinhId { get; set; }
    }

    public class ParamsSendMailThongTinHoaDon
    {
        public string ThongTinHoaDonId { get; set; }
        public string ToMail { get; set; }
        public string TenNguoiNhan { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string SoDienThoaiNguoiNhan { get; set; }
        public int? LoaiEmail { get; set; }
        public string Link { get; set; }
    }
}
