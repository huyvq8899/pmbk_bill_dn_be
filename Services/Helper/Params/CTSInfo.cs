using Services.ViewModels.XML.HoaDonDienTu;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helper.Params
{
    public class CTSInfo
    {
        public string SubjectName { get; set;}
        public string IssuerName { get; set; }
        public bool IsVerified { get; set; }
        public DateTime NotBefore { get; set; }
        public DateTime NotAfter { get; set; }
        public DateTime ThoiGianKy { get; set; }
        public NBan NBan { get; set; }
        public NMua NMua { get; set; }
    }
}
