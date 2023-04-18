using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.IV.IV
{
    public partial class DSCKS
    {
        /// <summary>
        /// chứa thông tin chữ ký số tổ chức trả thu nhập 
        /// </summary>
        public string TCTTNhap { get; set; }
        /// <summary>
        /// chứa các chữ ký số khác (nếu có).
        /// </summary>
        public string CCKSKhac { get; set; }
    }
}
