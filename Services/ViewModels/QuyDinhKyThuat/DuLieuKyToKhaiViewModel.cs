using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels.QuyDinhKyThuat
{
    public class DuLieuKyToKhaiViewModel
    {
        public string Id { get; set; }
        public string IdToKhai { get; set; }
        public string FileXMLDaKy { get; set; }
        public string Content { get; set; }
        public DateTime NgayKy { get; set; }
        public string MST { get; set; }
        public string Seri { get; set; }
    }
}
