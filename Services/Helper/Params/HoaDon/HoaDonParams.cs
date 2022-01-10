using ManagementServices.Helper;
using Services.ViewModels.QuanLyHoaDonDienTu;
using System.Collections.Generic;

namespace Services.Helper.Params.HoaDon
{
    public class HoaDonParams : PagingParams
    {
        public string KhachHangId { get; set; }
        public int? HinhThucHoaDon { get; set; }
        public int? UyNhiemLapHoaDon { get; set; }
        public int? LoaiHoaDon { get; set; }
        public int? TrangThaiHoaDonDienTu { get; set; }
        public int? TrangThaiPhatHanh { get; set; }
        public int? TrangThaiGuiHoaDon { get; set; }
        public int? TrangThaiChuyenDoi { get; set; }
        public int? TrangThaiXoaBo { get; set; }
        public int? TrangThaiBienBanXoaBo { get; set; }
        public HoaDonDienTuViewModel Filter { get; set; }
        public HoaDonThayTheSearch TimKiemTheo { get; set; }
        public string GiaTri { get; set; }
        public string HoaDonDienTuId { get; set; }
        public bool? IsChuyenDoi { get; set; }
    }
}
