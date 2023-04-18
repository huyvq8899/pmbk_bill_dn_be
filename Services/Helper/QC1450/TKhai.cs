using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Services.Helper.QC1450
{
    public class TKhai
    {
        [XmlElement("DLTKhai")]
        public DLTKhai DLTKhai { set; get; }
    }

    public class DLTKhai
    {
        [XmlElement("TTChung")]
        public DLTKhTTChung TTChung { set; get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DLTKhTTChung
    {
        [Required]
        [XmlElement]
        [MaxLength(6)]
        public string PBan { get; set; } = "2.0.0";

        /// <summary>
        /// Mẫu số (mẫu số tờ khai)
        /// </summary>
        [Required]
        [XmlElement]
        [MaxLength(15)]
        public string MSo { get; set; }

        /// <summary>
        /// Tên (tên tờ khai)
        /// </summary>
        [Required]
        [XmlElement]
        [MaxLength(100)]
        public string Ten { get; set; }

        /// <summary>
        /// Hình thức (Hình thức đăng ký/thay đổi thông tin sử dụng hóa đơn điện tử)
        /// </summary>
        [Required]
        [XmlElement]
        [MaxLength(1)]
        public string HThuc { get; set; }

        /// <summary>
        /// Tên NNT
        /// </summary>
        [Required]
        [XmlElement]
        [MaxLength(400)]
        public string TNNT { get; set; }

        /// <summary>
        /// Mã số thuế
        /// </summary>
        [Required]
        [XmlElement]
        [MaxLength(14)]
        public string MST { get; set; }

        /// <summary>
        /// CQT quản lý
        /// </summary>
        [Required]
        [XmlElement]
        [MaxLength(100)]
        public string CQTQLy { get; set; }

        /// <summary>
        /// Mã CQT quản lý
        /// </summary>
        [Required]
        [XmlElement]
        [MaxLength(5)]
        public string MCQTQLy { get; set; }

        /// <summary>
        /// Người liên hệ
        /// </summary>
        [Required]
        [XmlElement]
        [MaxLength(50)]
        public string NLHe { get; set; }

        /// <summary>
        /// Địa chỉ liên hệ
        /// </summary>
        [Required]
        [XmlElement]
        [MaxLength(400)]
        public string DCLHe { get; set; }

        /// <summary>
        /// Địa chỉ thư điện tử
        /// </summary>
        [Required]
        [XmlElement]
        [MaxLength(50)]
        public string DCTDTu { get; set; }

        /// <summary>
        /// Điện thoại liên hệ
        /// </summary>
        [Required]
        [XmlElement]
        [MaxLength(20)]
        public string DTLHe { get; set; }

        /// <summary>
        /// Địa danh
        /// </summary>
        [Required]
        [XmlElement]
        [MaxLength(50)]
        public string DDanh { get; set; }

        /// <summary>
        /// Ngày lập
        /// </summary>
        [Required]
        [XmlElement]
        public DateTime NLap { get; set; }
    }
}
