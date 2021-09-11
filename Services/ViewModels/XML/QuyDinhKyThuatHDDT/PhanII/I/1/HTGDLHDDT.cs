using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1
{
    public partial class HTGDLHDDT
    {
        /// <summary>
        /// <para>NNT địa bàn khó khăn (Doanh nghiệp nhỏ và vừa, hợp tác xã, hộ, cá nhân kinh doanh tại địa bàn có điều kiện kinh tế xã hội khó khăn, địa bàn có điều kiện kinh tế xã hội đặc biệt khó khăn)</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số(0: không áp dụng, 1: áp dụng)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public ADung NNTDBKKhan { get; set; }

        /// <summary>
        /// <para>NNT khác theo đề nghị UBND (Doanh nghiệp nhỏ và vừa khác theo đề nghị của Ủy ban nhân dân tỉnh, thành phố trực thuộc trung ương gửi Bộ Tài chính trừ doanh nghiệp hoạt động tại các khu kinh tế, khu công nghiệp, khu công nghệ cao)</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số(0: không áp dụng, 1: áp dụng)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public ADung NNTKTDNUBND { get; set; }

        /// <summary>
        /// <para>Chuyển dữ liệu trực tiếp đến CQT (Chuyển dữ liệu hóa đơn điện tử trực tiếp đến cơ quan thuế (điểm b1, khoản 3, Điều 22 của Nghị định))</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số(0: không áp dụng, 1: áp dụng)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public ADung CDLTTDCQT { get; set; }

        /// <summary>
        /// <para>Chuyển dữ liệu qua T-VAN (Thông qua tổ chức cung cấp dịch vụ hóa đơn điện tử (điểm b2, khoản 3, Điều 22 của Nghị định))</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số(0: không áp dụng, 1: áp dụng)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public ADung CDLQTVAN { get; set; }
    }
}
