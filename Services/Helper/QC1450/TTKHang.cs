using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Services.Helper.QC1450
{
    public class TTKHang
    {
        /// <summary>
        /// Tên
        /// </summary>
        [Required]
        [XmlElement]
        [MaxLength(400)]
        public string Ten { get; set; }

        /// <summary>
        /// Mã số thuế
        /// </summary>
        [Required]
        [XmlElement]
        [MaxLength(14)]
        public string MST { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        [Required]
        [XmlElement]
        [MaxLength(400)]
        public string DChi { get; set; }

        /// <summary>
        /// Mã khách hàng
        /// </summary>
        [XmlElement]
        [MaxLength(50)]
        public string MKHang { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        [XmlElement]
        [MaxLength(20)]
        public string SDThoai { get; set; }

        /// <summary>
        /// Địa chỉ thư điện tử 
        /// </summary>
        [XmlElement]
        [MaxLength(50)]
        public string DCTDTu { get; set; }

        /// <summary>
        /// Họ và tên người mua hàng
        /// </summary>
        [XmlElement]
        [MaxLength(100)]
        public string HVTNMHang { get; set; }

        /// <summary>
        /// Số tài khoản ngân hàng
        /// </summary>
        [XmlElement]
        [MaxLength(30)]
        public string STKNHang { get; set; }

        /// <summary>
        /// Tên ngân hàng
        /// </summary>
        [XmlElement]
        [MaxLength(400)]
        public string TNHang { get; set; }

        /// <summary>
        /// Fax
        /// </summary>
        [XmlElement]
        [MaxLength(20)]
        public string Fax { get; set; }

        /// <summary>
        /// Fax
        /// </summary>
        [XmlElement]
        [MaxLength(100)]
        public string Website { get; set; }
    }
}
