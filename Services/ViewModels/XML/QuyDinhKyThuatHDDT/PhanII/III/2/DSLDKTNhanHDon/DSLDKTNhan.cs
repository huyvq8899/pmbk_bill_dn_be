using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.III._2.DSLDKTNhanHDon
{
    public partial class DSLDKTNhan
    {
        public List<LDo> LDo { get; set; }
    }

    public partial class LDo
    {
        /// <summary>
        /// <para>Mã lỗi</para>
        /// <para>Độ dài tối đa: 4</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string MLoi { get; set; }

        /// <summary>
        /// <para>Mô tả (Lý do không tiếp nhận)</para>
        /// <para>Độ dài tối đa: 255</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string MTa { get; set; }
    }
}
