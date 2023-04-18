using Services.ViewModels.DanhMuc;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System;
using System.Collections.Generic;

namespace Services.ViewModels.QuanLyHoaDonDienTu
{
    public class ThongBaoHoaDonRaSoatViewModel
    {
        public string Id { get; set; }
        public string MaThongDiep { get; set; }
        public string SoThongBaoCuaCQT { get; set; }
        public DateTime NgayThongBao { get; set; }

        public string TenCQTCapTren { get; set; }
        public string TenCQTRaThongBao { get; set; }

        public string TenNguoiNopThue { get; set; }

        public string MaSoThue { get; set; }

        public DateTime NgayThoiHan { get; set; }

        public byte ThoiHan { get; set; }

        public byte Lan { get; set; }

        public bool? TinhTrang { get; set; }

        public string HinhThuc { get; set; }

        public string ChucDanh { get; set; }

        public string FileDinhKem { get; set; }
        public string FileUploadPath { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string ModifyBy { get; set; }
        public int LoaiSaiSot { get; set; }
        public int HinhThucHoaDon { get; set; }
    }

    public class ThongBaoChiTietHoaDonRaSoatViewModel
    {
        public string Id { get; set; }
        public string ThongBaoHoaDonRaSoatId { get; set; }

        public string MaCQTCap { get; set; }
        public string MauHoaDon { get; set; }
        public string KyHieuHoaDon { get; set; }

        public string SoHoaDon { get; set; }

        public DateTime? NgayLapHoaDon { get; set; }

        public byte LoaiApDungHD { get; set; }

        public string LyDoRaSoat { get; set; }

        public bool? DaGuiThongBao { get; set; }

        public string LyDo { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string ModifyBy { get; set; }
    }
}
