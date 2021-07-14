using Services.ViewModels.QuanLyHoaDonDienTu;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helper.Params.HoaDon
{
    public class ParamXoaBoHoaDon
    {
        public HoaDonDienTuViewModel HoaDon { get; set; }
        public int OptionalSend { get; set; } = 1;
    }
}
