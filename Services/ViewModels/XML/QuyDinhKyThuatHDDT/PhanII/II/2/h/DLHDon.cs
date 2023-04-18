using Services.ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities;
using System.Collections.Generic;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h
{
    public partial class DLHDon : SigningArea
    {
        public TTChung TTChung { get; set; }
        public NDHDon NDHDon { get; set; }
        public List<TTin> TTKhac { get; set; }
    }
}
