using DLL.Enums;
using Services.ViewModels.QuanLyHoaDonDienTu;
using System.Collections.Generic;

namespace Services.ViewModels.QuyDinhKyThuat
{
    public class DuLieuGuiHDDTViewModel : ThongTinChungViewModel
    {
        public string DuLieuGuiHDDTId { get; set; }

        // TTChung
        public string HoaDonDienTuId { get; set; }
        public int Count { get; set; }
        ///
        public HoaDonDienTuViewModel HoaDonDienTu { get; set; }
        public List<DuLieuGuiHDDTChiTietViewModel> DuLieuGuiHDDTChiTiets { get; set; }
    }
}
