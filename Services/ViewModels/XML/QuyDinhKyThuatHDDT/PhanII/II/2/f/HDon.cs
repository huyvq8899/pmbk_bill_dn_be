using Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.f
{
    public partial class HDon
    {
        public DLHDon DLHDon { get; set; }

        /// <summary>
        /// * chứa dữ liệu QR Code phục vụ tra cứu, thanh toán hóa đơn điện tử (Nếu có). 
        /// Chi tiết định dạng của thẻ được mô tả tại Khoản 7, Mục IV, Phần I
        /// <para>Độ dài tối đa: 512</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public string DLQRCode { get; set; }

        public DSCKS DSCKS { get; set; }

        /// <summary>
        /// * Với hóa đơn điện tử có mã, nếu đủ điều kiện cấp mã, 
        /// hệ thống của cơ quan thuế trả về chỉ tiêu Mã của cơ quan thuế trên hóa đơn điện tử 
        /// <para>Mã của cơ quan thuế (Mã của cơ quan thuế trên hóa đơn điện tử)</para>
        /// <para>Độ dài tối đa: 34</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        // public string MCCQT { get; set; }
    }
}
