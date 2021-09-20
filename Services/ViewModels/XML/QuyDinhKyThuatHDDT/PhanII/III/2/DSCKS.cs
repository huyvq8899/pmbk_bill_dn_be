using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.III._2
{
    public partial class DSCKS
    {
        public CQT CQT { get; set; }
        public CCKSKhac CCKSKhac { get; set; }
    }

    public partial class CQT
    {
        /// <summary>
        /// <para>Hình thức (Hình thức, chức danh của chữ k‎ý)</para>
        /// <para>Độ dài tối đa: 100</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(100)]
        public string HThuc { get; set; }
    }

    public partial class CCKSKhac
    {
    }
}
