using Services.ViewModels.BaoCao;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helper.Params.BaoCao
{
    public class BaoCaoParams
    {
        public DateTime? TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }
        public List<SoLuongHoaDonDaPhatHanhViewModel> ListSoLuongHoaDonDaPhatHanhs { get; set; }
        public string FilePath { get; set; }
    }
}
