using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels.QuyDinhKyThuat
{
    public class ToKhaiDangKyThongTinViewModel
    {
        public string Id { get; set; }
        public string FileXMLChuaKy { get; set; }
        public byte[] ContentXMLChuaKy { get; set; }
        public bool SignedStatus { get; set; }
        public List<DuLieuKyToKhaiViewModel> DuLieuKys { get; set; }
        public List<TrangThaiGuiToKhaiViewModel> TrangThaiGuiToKhais { get; set; }
    }
}
