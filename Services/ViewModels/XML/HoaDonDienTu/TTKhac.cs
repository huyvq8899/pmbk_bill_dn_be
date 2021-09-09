using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels.XML.HoaDonDienTu
{
    public partial class TTKhac
    {
        public List<TTin> TTin { set; get; }
    }

    public partial class TTin
    {
        public string TTruong { set; get; }

        public string KDLieu { set; get; }

        public string DLieu { set; get; }
    }
}
