using Services.ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._4;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._9
{
    public partial class TDiep
    {
        public TTChungThongDiep TTChung { get; set; }
        public DLieu DLieu { get; set; }
    }

    public partial class DLieu
    {
        public TBao TBao { get; set; }
    }
}
