using Services.ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a;
using System.Collections.Generic;
using TTChung = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.e.TTChung;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.f
{
    public partial class DLHDon : SigningArea
    {
        public TTChung TTChung { get; set; }
        public NDHDon NDHDon { get; set; }
        public List<TTin> TTKhac { get; set; }
    }
}
