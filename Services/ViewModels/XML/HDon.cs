using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels.XML
{
    public class HDon
    {
        public DLHDon DLHDon { set; get; }

        public DSCKS DSCKS { set; get; }
    }

    public class BBHuy
    {
        public TTBienBan DLTTBienBan { get; set; }
        public DLHDon DLHDon { get; set; }
        public DSCKS DSCKS { set; get; }

    }
}
