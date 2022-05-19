using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helper.Params.QuyDinhKyThuat
{
    public class GuiNhanToKhaiParams
    {
        public string FileXml { get; set; }
        public string Id { get; set; }
        public string MaThongDiep { get; set; }
        public string MST { get; set; }
        public string EncodedContent { get; set; }
        public UserViewModel ActionUser { get; set; }
    }
}
