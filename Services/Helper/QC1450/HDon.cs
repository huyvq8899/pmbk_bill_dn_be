using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Services.Helper.QC1450
{
    public class HDon
    {
        [XmlElement("DLHDon")]
        public DLHDon DLHDon { set; get; }
    }

    /// <summary>
    /// Dữ liệu hóa đơn
    /// </summary>
    public class DLHDon
    {
        [XmlElement("TTChung")]
        public DLHDTTChung TTChung { set; get; }

        [XmlElement("NDHDon")]
        public NDHDon NDHDon { set; get; }
    }

    /// <summary>
    /// Thông tin chung hóa đơn
    /// </summary>
    public class DLHDTTChung
    {
        [Required]
        [XmlElement]
        [MaxLength(6)]
        public string PBan { get; set; } = "2.0.0";

        /// <summary>
        /// Tên hóa đơn
        /// </summary>
        [XmlElement]
        [MaxLength(100)]
        public string THDon { get; set; }

        /// <summary>
        /// Ký hiệu mẫu số hóa đơn 
        /// </summary>
        [XmlElement]
        [MaxLength(1)]
        public string KHMSHDon { get; set; }


        /// <summary>
        /// Ký hiệu hóa đơn 
        /// </summary>
        [XmlElement]
        [MaxLength(6)]
        public string KHHDon { get; set; }

        /// <summary>
        /// Số hóa đơn
        /// </summary>
        [XmlElement]
        public long SHDon { get; set; }

        /// <summary>
        /// Mã hồ sơ
        /// </summary>
        [XmlElement]
        public long MHSo { get; set; }

        /// <summary>
        /// Ngày lập 
        /// </summary>
        [XmlElement]
        public DateTime NLap { get; set; }

        /// <summary>
        /// Số bảng kê (Số của bảng kê các loại hàng hóa, dịch vụ đã bán kèm theo hóa đơn)
        /// </summary>
        [XmlElement]
        [MaxLength(50)]
        public string SBKe { get; set; }

        /// <summary>
        /// Ngày bảng kê (Ngày của bảng kê các loại hàng hóa, dịch vụ đã bán kèm theo hóa đơn)
        /// </summary>
        [XmlElement]
        public DateTime NBKe { get; set; }

        /// <summary>
        /// Đơn vị tiền tệ
        /// </summary>
        [XmlElement]
        [MaxLength(3)]
        public string DVTTe { get; set; }

        /// <summary>
        /// Đơn vị tiền tệ
        /// </summary>
        [XmlElement]
        [Column(TypeName = "decimal(7,2)")]
        public float TGia { get; set; }

        /// <summary>
        /// Hình thức thanh toán 
        /// </summary>
        [XmlElement]
        [MaxLength(50)]
        public string HTTToan { set; get; }

        /// <summary>
        /// Mã số thuế tổ chức cung cấp giải pháp hóa đơn điện tử
        /// </summary>
        [XmlElement]
        [MaxLength(14)]
        public string MSTTCGP { set; get; }

        /// <summary>
        /// Mã số thuế đơn vị nhận ủy nhiệm lập hóa đơn
        /// </summary>
        [XmlElement]
        [MaxLength(14)]
        public string MSTDVNUNLHDon { set; get; }

        /// <summary>
        /// Tên đơn vị nhận ủy nhiệm lập hóa đơn
        /// </summary>
        [XmlElement]
        [MaxLength(400)]
        public string TDVNUNLHDon { set; get; }

        /// <summary>
        /// Địa chỉ đơn vị nhận ủy nhiệm lập hóa đơn
        /// </summary>
        [XmlElement]
        [MaxLength(400)]
        public string DCDVNUNLHDon { set; get; }
    }

    /// <summary>
    /// Nội dung hóa đơn
    /// </summary>
    public class NDHDon
    {
        /// <summary>
        /// Người bán
        /// </summary>
        [XmlElement("NBan")]
        public TTKHang NBan { set; get; }

        /// <summary>
        /// Người mua
        /// </summary>
        [XmlElement("NMua")]
        public TTKHang NMua { set; get; }
    }
}
