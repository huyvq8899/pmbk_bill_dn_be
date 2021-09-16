using Services.ViewModels.QuanLyHoaDonDienTu;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helper.Params.HoaDon
{
    public class TaiHoaDonLoiParams
    {
        public List<HoaDonDienTuViewModel> ListError { get; set; }
        public int Action { get; set; }
    }
}
