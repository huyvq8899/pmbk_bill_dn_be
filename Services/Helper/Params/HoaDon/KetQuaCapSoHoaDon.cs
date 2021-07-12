using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helper.Params.HoaDon
{
    public class KetQuaCapSoHoaDon
    {
        public int? LoiTrangThaiPhatHanh { get; set; }
        public string SoHoaDon { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
