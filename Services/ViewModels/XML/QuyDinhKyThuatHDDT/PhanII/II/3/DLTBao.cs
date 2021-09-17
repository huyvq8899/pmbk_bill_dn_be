using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._3
{
    public partial class DLTBao
    {
        /// <summary>
        /// <para>Phiên bản XML (Trong Quy định này có giá trị là 2.0.0)</para>
        /// <para>Độ dài tối đa: 6</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string PBan { get; set; }

        /// <summary>
        /// <para>Mẫu số (Mẫu số thông báo)</para>
        /// <para>Độ dài tối đa: 15</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự (Chi tiết tại Phụ lục VIII kèm theo Quy định này)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string MSo { get; set; }

        /// <summary>
        /// <para>Tên (Tên thông báo)</para>
        /// <para>Độ dài tối đa: 255</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string Ten { get; set; }

        /// <summary>
        /// <para>Số (Số thông báo)</para>
        /// <para>Độ dài tối đa: 30</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string So { get; set; }

        /// <summary>
        /// <para>Địa danh</para>
        /// <para>Độ dài tối đa: 50</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string DDanh { get; set; }

        /// <summary>
        /// <para>Ngày thông báo</para>
        /// <para>Kiểu dữ liệu: Ngày</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string NTBao { get; set; }

        /// <summary>
        /// <para>Mã số thuế</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (trừ trường hợp là đơn vị bán tài sản công)</para>
        /// </summary>
        public string MST { get; set; }

        /// <summary>
        /// <para>Mã đơn vị quan hệ ngân sách (Mã số đơn vị có quan hệ với ngân sách của đơn vị bán tài sản công)</para>
        /// <para>Độ dài tối đa: 7</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc nếu là đơn vị bán tài sản công không có Mã số thuế</para>
        /// </summary>
        public string MDVQHNSach { get; set; }

        /// <summary>
        /// <para>Tên NNT</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string TNNT { get; set; }

        /// <summary>
        /// <para>Thời gian gửi (Thời gian NNT gửi tới CQT)</para>
        /// <para>Kiểu dữ liệu: Ngày giờ</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public string TGGui { get; set; }

        /// <summary>
        /// <para>Loại thông báo</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số (Chi tiết tại Phụ lục XIII kèm theo Quy định này)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public LTBao LTBao { get; set; }

        /// <summary>
        /// <para>Căn cứ (Tên loại thông điệp nhận) </para>
        /// <para>Độ dài tối đa: 255</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string CCu { get; set; }

        /// <summary>
        /// <para>Mã giao dịch điện tử</para>
        /// <para>Độ dài tối đa: 46</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public string MGDDTu { get; set; }

        /// <summary>
        /// <para>Số lượng (Số lượng dữ liệu trong gói)</para>
        /// <para>Độ dài tối đa: 7</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public string SLuong { get; set; }

        public LCMa LCMa { get; set; }

        public LHDKMa LHDKMa { get; set; }

        public LBTHKXDau LBTHKXDau { get; set; }

        public LBTHXDau LBTHXDau { get; set; }

        public KHLKhac KHLKhac { get; set; }
    }
}
