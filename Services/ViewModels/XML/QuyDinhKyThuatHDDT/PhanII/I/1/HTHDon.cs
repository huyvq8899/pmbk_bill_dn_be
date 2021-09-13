using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;

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
        public ADung CMa { get; set; }

        /// <summary>
        /// <para>Không có mã (Hình thức hóa đơn không có mã của CQT)</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số(0: không áp dụng, 1: áp dụng)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public ADung KCMa { get; set; }
    }
}
