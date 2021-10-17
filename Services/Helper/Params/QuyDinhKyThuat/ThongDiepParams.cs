using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helper.Params.QuyDinhKyThuat
{
    public class ThongDiepParams
    {
        public string FileUrl { get; set; }
        public byte[] FileByte { get; set; }
        public Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._8.TDiep ThongDiepKhongUyNhiem { get; set; }
        public Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._9.TDiep ThongDiepUyNhiem { get; set; }

    }
}
