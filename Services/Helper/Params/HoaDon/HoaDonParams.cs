﻿using ManagementServices.Helper;
using Services.ViewModels.QuanLyHoaDonDienTu;

namespace Services.Helper.Params.HoaDon
{
    public class HoaDonParams : PagingParams
    {
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
    }
}