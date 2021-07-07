using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels.Params
{
    public class BanMauHoaDon
    {
        public string TenBanMau { get; set; }
        public List<ChiTietMauHoaDon> ChiTiets { get; set; }
    }

    public class ChiTietMauHoaDon
    {
        public string Code { get; set; }
        public string File { get; set; }
        public int LoaiHoaDon { get; set; }
        public int LoaiMau { get; set; }
        public int LoaiThueGTGT { get; set; }
        public int LoaiNgonNgu { get; set; }
        public int LoaiKhoGiay { get; set; }
    }
}
