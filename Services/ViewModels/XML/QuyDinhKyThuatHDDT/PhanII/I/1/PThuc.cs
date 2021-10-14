using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1
{
    public partial class PThuc
    {
        /// <summary>
        /// <para>Chuyển đầy đủ (Chuyển đầy đủ nội dung từng hóa đơn)</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số(0: không áp dụng, 1: áp dụng)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public int CDDu { get; set; }

        /// <summary>
        /// <para>Chuyển bảng tổng hợp (Chuyển theo bảng tổng hợp dữ liệu hóa đơn điện tử (điểm a1, khoản 3, Điều 22 của Nghị định))</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số(0: không áp dụng, 1: áp dụng)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public int CBTHop { get; set; }
    }
}
