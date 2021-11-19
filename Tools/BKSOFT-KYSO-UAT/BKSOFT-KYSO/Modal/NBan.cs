using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKSOFT_KYSO.Modal
{
    /// <summary>
    /// Chứa tên, địa chỉ, MST của người bán
    /// </summary>
    public class NBan
    {
        [Required]
        //[MaxLength(400)]
        public string Ten { set; get; }

        /// <summary>
        /// Mã số thuế
        /// </summary>
        [Required]
        //[MaxLength(14)]
        public string MST { set; get; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        [Required]
        //[MaxLength(400)]
        public string DChi { set; get; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        //[MaxLength(20)]
        public string SDThoai { set; get; }

        /// <summary>
        /// Địa chỉ thư điện tử
        /// </summary>
        //[MaxLength(50)]
        public string DCTDTu { set; get; }

        /// <summary>
        /// Sổ tài khoản ngân hàng
        /// </summary>
        //[MaxLength(30)]
        public string STKNHang { set; get; }

        /// <summary>
        /// Tên ngân hàng
        /// </summary>
        //[MaxLength(400)]
        public string TNHang { set; get; }

        /// <summary>
        /// Fax
        /// </summary>
        //[MaxLength(20)]
        public string Fax { set; get; }

        /// <summary>
        /// Website
        /// </summary>
        //[MaxLength(50)]
        public string Website { set; get; }

        public string TenP1 { set; get; }

        public string TenP2 { set; get; }
    }
}
