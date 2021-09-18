using Services.ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.VI._1
{
    public partial class TDiep
    {
        public TTChungThongDiep TTChung { get; set; }
        public DLieu DLieu { get; set; }
    }

    public partial class DLieu
    {
        public List<HDon> HDon { get; set; }
    }
}
