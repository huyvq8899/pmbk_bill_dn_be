using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Services.ViewModels.XML
{
    public class NMua
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
        /// Mã khách hàng
        /// </summary>
        [MaxLength(50)]
        public string MKHang { set; get; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        [MaxLength(20)]
        public string SDThoai { set; get; }

        /// <summary>
        /// Địa chỉ thư điện tử
        /// </summary>
        [MaxLength(50)]
        public string DCTDTu { set; get; }

        /// <summary>
        /// Họ và tên người mua hàng
        /// </summary>
        [MaxLength(100)]
        public string HVTNMHang { set; get; }

        /// <summary>
        /// Sổ tài khoản ngân hàng
        /// </summary>
        [MaxLength(30)]
        public string STKNHang { set; get; }

        /// <summary>
        /// Tên ngân hàng
        /// </summary>
        [MaxLength(400)]
        public string TNHang { set; get; }

        public TTKhac TTKhac { set; get; }
    }
}
