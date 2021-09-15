﻿namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._5
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
        public string HThuc { get; set; }
    }

    public partial class CCKSKhac
    {
    }
}