using Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.d
{
    public partial class TToan
    {
        /// <summary>
        /// <para>Tổng tiền thanh toán bằng số</para>
        /// <para>Độ dài tối đa: 19,4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public decimal TgTTTBSo { get; set; }

        /// <summary>
        /// <para>Tổng tiền thanh toán bằng chữ</para>
        /// <para>Độ dài tối đa: 255</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public decimal TgTTTBChu { get; set; }

        public DSLPhi DSLPhi { get; set; }

        public TTKhac TTKhac { get; set; }
    }
}
