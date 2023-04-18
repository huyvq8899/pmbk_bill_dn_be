using DLL.Enums;
using System.Collections.Generic;

namespace Services.ViewModels.QuanLy
{
    public class HinhThucVaLoaiHoaDonNgungSuDung
    {
        public HinhThucHoaDon HinhThucHoaDon { get; set; }
        public List<LoaiHoaDon> LoaiHoaDons { get; set; }
    }
}
