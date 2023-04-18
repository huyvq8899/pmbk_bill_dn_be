using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1
{
    public partial class HTHDon
    {
        /// <summary>
        /// <para>Có mã (Hình thức hóa đơn có mã của CQT)</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số(0: không áp dụng, 1: áp dụng)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public int CMa { get; set; }

        /// <summary>
        /// <para>Có mã từ máy tính tiền (Hình thức hóa đơn có mã khởi tạo từ máy tính tiền)</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số (1: áp dụng)</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public int CMTMTTien { get; set; }

        /// <summary>
        /// <para>Không có mã (Hình thức hóa đơn không có mã của CQT)</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số(0: không áp dụng, 1: áp dụng)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public int KCMa { get; set; }
    }
}
