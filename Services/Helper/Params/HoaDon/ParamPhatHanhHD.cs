using DLL.Enums;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Services.Helper.Params.HoaDon
{
    public class ParamPhatHanhHD
    {
        public ParamPhatHanhHD()
        {
            SkipCheckHetHieuLucTrongKhoang = false;
            SkipChecNgayKyLonHonNgayHoaDon = false;
        }

        public int? Type { get; set; }
        public bool TuDongGuiMail { get; set; } = false;
        public string NguoiNhanHD { get; set; }
        public string EmailNguoiNhan { get; set; }
        public string SoDienThoaiNguoiNhan { get; set; }
        public string HoaDonDienTuId { get; set; }
        public HoaDonDienTuViewModel HoaDon { get; set; }
        public string DataPDF { set; get; }
        public string DataXML { set; get; }

        public bool IsBuyerSigned { get; set; } = false;

        public TTChungThongDiep TTChungThongDiep { get; set; }
        public bool? SkipCheckHetHieuLucTrongKhoang { get; set; }
        public bool? SkipChecNgayKyLonHonNgayHoaDon { get; set; }
        public TTNKy NBan { set; get; }
        public int? TrangThaiQuyTrinh { get; set; }
    }

    public class TTNKy
    {
        public string Ten { set; get; }

        public string SDThoai { set; get; }

        public string DChi { set; get; }

        public string TenP1 { set; get; }

        public string TenP2 { set; get; }
    }

    public class NBan
    {
        [Required]
        [MaxLength(400)]
        public string Ten { set; get; }

        /// <summary>
        /// Mã số thuế
        /// </summary>
        [Required]
        [MaxLength(14)]
        public string MST { set; get; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        [Required]
        [MaxLength(400)]
        public string DChi { set; get; }
        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string SDThoai { set; get; }
        /// <summary>
        /// Địa chỉ thư điện tử
        /// </summary>
        [MaxLength(50)]
        public string DCTDTu { set; get; }

        /// <summary>
        /// Sổ tài khoản ngân hàng
        /// </summary>
        public string STKNHang { set; get; }

        /// <summary>
        /// Tên ngân hàng
        /// </summary>
        [MaxLength(400)]
        public string TNHang { set; get; }

        /// <summary>
        /// Fax
        /// </summary>
        [MaxLength(20)]
        public string Fax { set; get; }

        /// <summary>
        /// Website
        /// </summary>
        [MaxLength(50)]
        public string Website { set; get; }

        public TTKhac TTKhac { set; get; }
    }

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

    public enum LOAI_PHAN_HOI
    {
        SIGN_INVOICE = 1000,
        SIGN_INVOICE_XML = 1001,
        SIGN_RECORD = 1002,

        REP_SIGN_SUC = 2000,
        REP_SIGN_ERR = 2001,
    }
}
