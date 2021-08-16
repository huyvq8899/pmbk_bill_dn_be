using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Services.ViewModels.XML.HoaDonDienTu
{
    public class TTChung
    {
        /// <summary>
        /// Phiên bản XML. Trong khuyến nghị này có giá trị là 1.1.0
        /// </summary>
        [Required]
        [MaxLength(6)]
        public string PBan { set; get; } = "1.1.0";

        /// <summary>
        /// Tên hóa đơn
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string THDon { set; get; }

        /// <summary>
        /// Ký hiệu mẫu số hóa đơn
        /// </summary>
        [Required]
        [MaxLength(11)]
        public string KHMSHDon { set; get; }

        /// <summary>
        /// Ký hiệu hóa đơn
        /// </summary>
        [Required]
        [MaxLength(6)]
        public string KHHDon { set; get; }

        /// <summary>
        /// Số hóa đơn
        /// </summary>
        [Required]
        [MaxLength(7)]
        public string SHDon { set; get; }

        /// <summary>
        /// Ngày lập
        /// </summary>
        [Required]
        public string NLap { set; get; }

        /// <summary>
        /// Đơn vị tiền tệ
        /// </summary>
        [Required]
        [MaxLength(3)]
        public string DVTTe { set; get; }

        /// <summary>
        /// Đơn vị tiền tệ. (Không bắt buộc nếu đơn vị tiền tệ là VNĐ)
        /// </summary>
        public decimal TGia { set; get; }

        /// <summary>
        /// Thông tin nhà cung cấp (Thông tin đơn vị cung cấp hóa đơn điện tử)
        /// </summary>
        [MaxLength(14)]
        public string TTNCC { set; get; }

        /// <summary>
        /// Đường dẫn tra cứu (Đường dẫn tra cứu hóa đơn)
        /// </summary>
        [MaxLength(150)]
        public string DDTCuu { set; get; }

        /// <summary>
        /// Mã tra cứu
        /// </summary>
        [MaxLength(50)]
        public string MTCuu { set; get; }

        /// <summary>
        /// Hính thức thanh toán
        /// </summary>
        [MaxLength(1)]
        public int? HTTToan { set; get; }

        /// <summary>
        /// Tên hình thức thanh toán khác
        /// </summary>
        [MaxLength(50)]
        public string THTTTKhac { set; get; }

        /// <summary>
        /// Chứa thông tin hóa đơn liên quan trong trường hợp là hóa đơn điều chỉnh hoặc thay thế
        /// </summary>
        public TTHDLQuan TTHDLQuan { set; get; }
    }

    public class TTHDLQuan
    {
        /// <summary>
        /// Tính chất hóa đơn
        /// 1 - Thay thế
        /// 2 - Điều chỉnh
        /// </summary>
        [Required]
        [MaxLength(1)]
        public string TCHDon { set; get; }

        /// <summary>
        /// Loại hóa đơn liên quan (Loại áp dụng hóa đơn của HĐ có liên quan)
        /// </summary>
        [Required]
        [MaxLength(1)]
        public string LHDCLQuan { set; get; }

        /// <summary>
        /// Ký hiệu mẫu số hóa đơn có liên quan (Ký hiệu mẫu số hóa đơn bị thay thế/điều chỉnh)
        /// </summary>
        [Required]
        [MaxLength(11)]
        public string KHMSHDCLQuan { set; get; }

        /// <summary>
        /// Ký hiệu hóa đơn có liên quan (Ký hiệu hóa đơn bị thay thế/điều chỉnh)
        /// </summary>
        [Required]
        [MaxLength(8)]
        public string KHHDCLQuan { set; get; }

        /// <summary>
        /// Số hóa đơn có liên quan (Số hóa đơn bị thay thế/điều chỉnh)
        /// </summary>
        [Required]
        [MaxLength(7)]
        public string SHDCLQuan { set; get; }

        [MaxLength(255)]
        public string GChu { set; get; }

        public TTKhac TTKhac { set; get; }
    }

    public class TTBienBan
    {
        /// <summary>
        /// Phiên bản XML. Trong khuyến nghị này có giá trị là 1.1.0
        /// </summary>
        [Required]
        [MaxLength(6)]
        public string PBan { set; get; } = "1.1.0";
        public DateTime? NgayBienBan { get; set; }
        public string ThongTu { get; set; }
        public string SoBienBan { get; set; }
        public string TenKhachHang { get; set; }
        public string MaSoThue { get; set; }
        public string SoDienThoai { get; set; }
        public string DaiDien { get; set; }
        public string DiaChi { get; set; }
        public string ChucVu { get; set; }
        public string TenCongTyBenA { get; set; }
        public string DiaChiBenA { get; set; }
        public string MaSoThueBenA { get; set; }
        public string SoDienThoaiBenA { get; set; }
        public string DaiDienBenA { get; set; }
        public string ChucVuBenA { get; set; }
    }
}
