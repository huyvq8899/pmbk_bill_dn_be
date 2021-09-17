using Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._3.HDonDSHDon;
using System.Collections.Generic;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._3
{
    public partial class LHDKMa
    {
        public DSHDon DSHDon { get; set; }
    }

    public partial class DSHDon
    {
        public List<HDon> HDon { get; set; }
    }
}
