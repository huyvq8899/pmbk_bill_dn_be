using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels.XML.HoaDonDienTu
{
    public class TTKhac
    {
        public List<TTin> TTin { set; get; }
    }

    public class TTin
    {
        public string TTruong { set; get; }

        public string KDLieu { set; get; }

        public string DLieu { set; get; }
    }
}
