using Services.ViewModels.QuyDinhKyThuat;
using System;
using System.Collections.Generic;
using System.Text;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.IV._1;
using Services.ViewModels.QuanLyHoaDonDienTu;

namespace Services.Helper.Params.QuyDinhKyThuat
{
    public class BangTongHopParams
    {
        public string TuNgay { get; set; }
        public string DenNgay { get; set; }
        public int LoaiHangHoa { get; set; }
    }

    public class BangTongHopDuLieuParams
    {
        public ThongDiepChungViewModel TTChung1 { get; set; }
        public TTChung TTChung2 { get; set; }
        public List<TongHopDuLieuHoaDonGuiCQTViewModel> DuLieu { get; set; }
        public string ThongDiepChungId { get; set; }
    }

    public class BangTongHopParams2
    {
        public int NamDuLieu { get; set; }
        public int? ThangDuLieu { get; set; }
        public int? QuyDuLieu { get; set; }
    }
}
