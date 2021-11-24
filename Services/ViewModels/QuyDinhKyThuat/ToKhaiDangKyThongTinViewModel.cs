using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels.QuyDinhKyThuat
{
    public class ToKhaiDangKyThongTinViewModel : ThongTinChungViewModel
    {
        public string Id { get; set; }
        public string PPTinh { get; set; }
        public bool IsThemMoi { get; set; }
        public bool NhanUyNhiem { get; set; }
        public int? LoaiUyNhiem { get; set; }
        public string FileXMLChuaKy { get; set; }
        public XML.QuyDinhKyThuatHDDT.PhanII.I._1.TKhai ToKhaiKhongUyNhiem { get; set; }
        public XML.QuyDinhKyThuatHDDT.PhanII.I._2.TKhai ToKhaiUyNhiem { get; set; }
        public bool SignedStatus { get; set; }
        public DateTime? NgayTao { get; set; }
        public DateTime? NgayKy { get; set; }
        public DateTime? NgayGui { get; set; }
        public string TrangThaiGui { get; set; }
        public string TrangThaiTiepNhan { get; set; }
        public List<DuLieuKyToKhaiViewModel> DuLieuKys { get; set; }
    }
}
