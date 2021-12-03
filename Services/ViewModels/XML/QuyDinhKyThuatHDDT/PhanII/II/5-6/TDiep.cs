
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities;
using HDonGTGT = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.HDon;
using HDonBH = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.b.HDon;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6
{
    //------------------ Hóa đơn GTGT ----------------------
    public partial class TDiep
    {
        public TTChungThongDiep TTChung { get; set; }
        public DLieu DLieu { get; set; }
    }

    public partial class DLieu
    {
        public HDonGTGT HDon { get; set; }
    }

    //------------------ Hóa đơn bán hàng ----------------------
    public partial class TDiep2
    {
        public TTChungThongDiep TTChung { get; set; }
        public DLieu2 DLieu { get; set; }
    }

    public partial class DLieu2
    {
        public HDonBH HDon { get; set; }
    }
}
