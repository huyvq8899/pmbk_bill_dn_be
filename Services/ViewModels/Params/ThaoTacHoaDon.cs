using Services.ViewModels.QuanLyHoaDonDienTu;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels.Params
{
    public class CapPhatSoHoaDonParam
    {
        public HoaDonDienTuViewModel Model { get; set; }
        public string SoHoaDon { get; set; }
    }

    public class CapPhatSoHoaDonHangLoatParam
    {
        public List<HoaDonDienTuViewModel> Models { get; set; }
        public List<string> SoHoaDons { get; set; }
    }
}
