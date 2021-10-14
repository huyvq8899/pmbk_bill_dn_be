using Services.ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities;
using System.Collections.Generic;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._7
{
    public partial class TDiep<T> where T : class
    {
        public TTChungThongDiep TTChung { get; set; }
        public DLieu<T> DLieu { get; set; }
    }

    public partial class DLieu<T> where T : class
    {
        public List<T> HDon { get; set; }
    }
}
