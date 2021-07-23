using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Services.ViewModels.XML
{
    /// <summary>
    /// Chứa thông tin thanh toán của hóa đơn (Không bắt buộc trong trường hợp là hóa đơn điều chỉnh cho hóa đơn đã lập 
    /// có sai sót)
    /// </summary>
    public class TToan
    {
        //public THTTLTSuat THTTLTSuat { set; get; }
        public List<LTSuat> THTTLTSuat { set; get; }
        /// <summary>
        /// Tính chất điều chỉnh
        /// 1 - Tăng
        /// 2 - Giảm
        /// </summary>
        public int? TCDChinh { set; get; }
        [XmlIgnore]
        public bool TCDChinhSpecified { get { return this.TCDChinh != null; } }
        /// <summary>
        /// Tổng tiền chưa thuế (Tổng cộng thành tiền chưa có thuế GTGT)
        /// </summary>
        [Required]
        public decimal TgTCThue { set; get; }

        /// <summary>
        /// Tổng tiền thuế (Tổng cộng tiền thuế GTGT)
        /// </summary>
        [Required]
        public decimal TgTThue { set; get; }

        public DSLPhi DSLPhi { set; get; }

        /// <summary>
        /// Tính chất điều chỉnh chiết khấu thương mại
        /// </summary>
        public int? TCDCCKTMai { set; get; }
        [XmlIgnore]
        public bool TCDCCKTMaiSpecified { get { return this.TCDCCKTMai != null; } }
        /// <summary>
        /// Tổng tiền chiết khấu thương mại
        /// </summary>
        public decimal? TTCKTMai { set; get; }
        [XmlIgnore]
        public bool TTCKTMaiSpecified { get { return this.TTCKTMai != null; } }
        /// <summary>
        /// Tính chất điều chỉnh tổng tiền thanh toán
        /// </summary>
        public int? TCDCTTTToan { set; get; }
        [XmlIgnore]
        public bool TCDCTTTToanSpecified { get { return this.TCDCTTTToan != null; } }
        /// <summary>
        /// Tổng tiền thanh toán bằng số
        /// </summary>
        [Required]
        public decimal TgTTTBSo { set; get; }

        /// <summary>
        /// Tổng tiền thanh toán bằng chữ
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string TgTTTBChu { set; get; }

        public TTKhac TTKhac { set; get; }
    }

    /// <summary>
    /// Chứa thông tin tổng hợp theo từng loại thuế suất (Không bắt buộc trong trường hợp là hóa đơn điều chỉnh cho hóa đơn đã lập có sai sót)
    /// </summary>
    public class THTTLTSuat
    {
        public List<LTSuat> LTSuat { set; get; }
    }

    /// <summary>
    /// Chứa chi tiết thông tin tổng hợp của mỗi loại thuế suất
    /// </summary>
    public class LTSuat
    {
        /// <summary>
        /// Tính chất điều chỉnh
        /// 1 - Tăng
        /// 2 - Giảm
        /// </summary>
        public int? TCDChinh { set; get; }
        [XmlIgnore]
        public bool TCDChinhSpecified { get { return this.TCDChinh != null; } }
        /// <summary>
        /// Thuế suất (Thuế suất thuế GTGT)
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string TSuat { set; get; }

        /// <summary>
        /// Thành tiền (Thành tiền chưa có thuế GTGT)
        /// </summary>
        [Required]
        public decimal ThTien { set; get; }

        /// <summary>
        /// Tiền thuế (Tiền thuế GTGT)
        /// </summary>
        [Required]
        public decimal TThue { set; get; }
    }

    /// <summary>
    /// Chứa danh sách các loại tiền phí, lệ phí (Nếu có)
    /// </summary>
    public class DSLPhi
    {
        public List<LPhi> LPhi { set; get; }
    }

    /// <summary>
    /// Chứa chi tiết từng loại tiền phí, lệ phí. Thẻ này có thể lặp lại nhiều lần tương ứng với số loại phí,
    /// lệ phí
    /// </summary>
    public class LPhi
    {

        /// <summary>
        /// Tính chất điều chỉnh
        /// 1 - Tăng
        /// 2 - Giảm
        /// </summary>
        public int TCDChinh { set; get; }

        /// <summary>
        /// Tên loại phí
        /// </summary>
        [MaxLength(100)]
        public string TLPhi { set; get; }

        /// <summary>
        /// Tiền phí
        /// </summary>
        public decimal TPhi { set; get; }
    }
}
