using DLL.Enums;
using Services.Helper.LogHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.DanhMuc
{
    public class MauHoaDonViewModel : ThongTinChungViewModel
    {
        [IgnoreLogging]
        public string MauHoaDonId { get; set; }
        [Display(Name = "Tên")]
        public string Ten { get; set; }
        [IgnoreLogging]
        public int? SoThuTu { get; set; }
        [IgnoreLogging]
        public string MauSo { get; set; }
        [IgnoreLogging]
        public string KyHieu { get; set; }
        [IgnoreLogging]
        public string TenBoMau { get; set; }
        [Display(Name = "Ngày ký")]
        public DateTime? NgayKy { get; set; }
        [IgnoreLogging]
        public QuyDinhApDung QuyDinhApDung { get; set; }
        [IgnoreLogging]
        public UyNhiemLapHoaDon UyNhiemLapHoaDon { get; set; }
        [IgnoreLogging]
        public HinhThucHoaDon HinhThucHoaDon { get; set; }
        [IgnoreLogging]
        public LoaiHoaDon LoaiHoaDon { get; set; }
        [IgnoreLogging]
        public LoaiMauHoaDon LoaiMauHoaDon { get; set; }
        [IgnoreLogging]
        public LoaiThueGTGT LoaiThueGTGT { get; set; }
        [IgnoreLogging]
        public LoaiNgonNgu LoaiNgonNgu { get; set; }
        [IgnoreLogging]
        public LoaiKhoGiay LoaiKhoGiay { get; set; }

        [IgnoreLogging]
        public string TenLoaiHoaDon { get; set; }
        [IgnoreLogging]
        public string TenHinhThucHoaDon { get; set; }
        [IgnoreLogging]
        public string TenUyNhiemLapHoaDon { get; set; }
        [IgnoreLogging]
        public string TenLoaiMau { get; set; }
        [IgnoreLogging]
        public string NgayCapNhatFilter { get; set; }
        ///////////////  property temp  //////////////////////////
        [IgnoreLogging]
        public string TenLoaiThueGTGT { get; set; }
        ///////////////  property temp  //////////////////////////
        [IgnoreLogging]
        public string WebRootPath { get; set; }
        [IgnoreLogging]
        public string DatabaseName { get; set; }
        [IgnoreLogging]
        public string LoaiNghiepVu { get; set; }
        [IgnoreLogging]
        public string FilePath { get; set; }
        /////////////////////////////////////////
        [IgnoreLogging]
        public string TenLoaiHoaDonFull { get; set; }

        [IgnoreLogging]
        public List<string> KyHieus { get; set; }
        [IgnoreLogging]
        public List<string> MauHoaDonIds { get; set; }
        [IgnoreLogging]
        public List<ThongTinChiTietKetQuaHuy> ThongTinChiTiets { get; set; }
        [Special]
        public List<MauHoaDonThietLapMacDinhViewModel> MauHoaDonThietLapMacDinhs { get; set; }
        [Special]
        public List<MauHoaDonTuyChinhChiTietViewModel> MauHoaDonTuyChinhChiTiets { get; set; }
        [IgnoreLogging]
        public List<MauHoaDonFileViewModel> MauHoaDonFiles { get; set; }

        [IgnoreLogging]
        public bool? Actived { get; set; }
        [IgnoreLogging]
        public bool? Disabled { get; set; }
    }

    public class ThongTinChiTietKetQuaHuy
    {
        public string KyHieu { get; set; }
        public int? TuSo { get; set; }
    }
}
