using Services.ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._3;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._8
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
