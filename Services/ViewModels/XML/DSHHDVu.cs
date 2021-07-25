using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Services.ViewModels.XML
{
    /// <summary>
    /// Chứa danh sách hàng hóa dịch vụ
    /// Không bắt buộc trong trường hợp là hóa đơn điều chỉnh cho hóa đơn đã lập có sai sót
    /// </summary>
    public class DSHHDVu
    {
        public List<HHDVu> HHDVu { set; get; }
    }
    // <summary>
    /// Chưa chi tiết 01 dòng hàng dịch vụ (Thẻ này có thể lặp lại nhiều lần tương ứng với số lượng hàng hóa,
    /// dịch vụ
    /// </summary>
    public class HHDVu
    {
        /// <summary>
        /// Tính chất
        /// 1 - Hàng hóa, dịch vụ
        /// 2 - Khuyến mại
        /// 3 - Chiết khấu thương mại (Trong trường hợp muốn thể hiện thông tin chiết khấu theo dòng)
        /// 4 - Ghi chú, diễn giải
        /// </summary>
        public int? TChat { set; get; }
        [XmlIgnore]
        public bool TChatSpecified { get { return this.TChat != null; } }
        /// <summary>
        /// Tính chất điều chỉnh
        /// 1 - Tăng
        /// 2 - Giảm
        /// </summary>
        public int? TCDChinh { set; get; }
        [XmlIgnore]
        public bool TCDChinhSpecified { get { return this.TCDChinh != null; } }
        /// <summary>
        /// Số thứ tự
        /// </summary>
        public int? STT { set; get; }
        [XmlIgnore]
        public bool STTSpecified { get { return this.STT != null; } }
        /// <summary>
        /// Mã hàng hóa, dịch vụ
        /// </summary>
        [MaxLength(50)]
        public string MHHDVu { set; get; }

        /// <summary>
        /// Tên hàng hóa, dịch vụ
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string THHDVu { set; get; }

        /// <summary>
        /// Đơn vị tính
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string DVTinh { set; get; }

        /// <summary>
        /// Số lượng
        /// </summary>
        [Required]
        public decimal SLuong { set; get; }

        /// <summary>
        /// Đơn giá
        /// </summary>
        [Required]
        public decimal DGia { set; get; }

        /// <summary>
        /// Tỉ lệ % chiết khấu (Trong trường hợp muốn thể hiện thông tin chiết khấu theo cột cho từng hàng hóa, dịch vụ)
        /// </summary>
        public decimal? TLCKhau { set; get; }
        [XmlIgnore]
        public bool TLCKhauSpecified { get { return this.TLCKhau != null; } }
        /// <summary>
        /// Số tiền chiết khấu (Trong trường hợp muốn thể hiện thông tin chiết khấu theo từng cột cho từng hàng hóa, dịch vụ)
        /// </summary>
        [Required]
        public decimal? STCKhau { set; get; }
        [XmlIgnore]
        public bool STCKhauSpecified { get { return this.STCKhau != null; } }
        /// <summary>
        /// Thành tiền (Thành tiền chưa có thuế GTGT)
        /// </summary>
        [Required]
        public decimal ThTien { set; get; }

        /// <summary>
        /// Thuế suất (Thuế suất thuế GTGT)
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string TSuat { set; get; }

        public TTKhac TTKhac { set; get; }
    }
}
