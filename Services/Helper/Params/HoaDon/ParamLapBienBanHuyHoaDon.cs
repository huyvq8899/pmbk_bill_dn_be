using Services.ViewModels.QuanLyHoaDonDienTu;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helper.Params.HoaDon
{
    public class ParamLapBienBanHuyHoaDon
    {
        public BienBanXoaBoViewModel Data { get; set; }
        public int OptionalSendData { get; set; } = 1;
    }
}
