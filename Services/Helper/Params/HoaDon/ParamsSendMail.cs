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
    }
}
