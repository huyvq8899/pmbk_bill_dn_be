using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.III._1
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
        /// <para>Kiểu dữ liệu: Chuỗi ký tự (Chi tiết tại Phụ lục VIII kèm theo Quy định này)(Chú thích: MSoThongBao.cs)</para>
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
        /// <para>Loại (Loại thông báo)</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Số (1: Thông báo hủy/giải trình của NNT, 2: Thông báo hủy/giải trình của NNT theo thông báo của CQT)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public Loai Loai { get; set; }

        /// <summary>
        /// <para>Số (Số thông báo của CQT)</para>
        /// <para>Độ dài tối đa: 30</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para> 
        /// <para>Bắt buộc (Đối với Loại=2: Thông báo hủy/giải trình của NNT theo thông báo của CQT)</para>
        /// </summary>
        public string So { get; set; }

        /// <summary>
        /// <para>Ngày thông báo của CQT</para>
        /// <para>Kiểu dữ liệu: Ngày</para>
        /// <para>Bắt buộc (Đối với Loại=2: Thông báo hủy/giải trình của NNT theo thông báo của CQT)</para>
        /// </summary>
        public string NTBCCQT { get; set; }

        /// <summary>
        /// <para>Mã CQT (Mã cơ quan thuế hóa đơn)</para>
        /// <para>Độ dài tối đa: 5</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string MCQT { get; set; }

        /// <summary>
        /// <para>Tên cơ quan thuế</para>
        /// <para>Độ dài tối đa: 100</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string TCQT { get; set; }

        /// <summary>
        /// <para>Tên NNT</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string TNNT { get; set; }

        /// <summary>
        /// <para>Mã số thuế</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Trừ trường hợp là đơn vị bán tài sản công)</para>
        /// </summary>
        public string MST { get; set; }

        /// <summary>
        /// <para>Mã đơn vị quan hệ ngân sách (Mã số đơn vị có quan hệ với ngân sách của đơn vị bán tài sản công)</para>
        /// <para>Độ dài tối đa: 7</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Đối với đơn vị bán tài sản công không có Mã số thuế)</para>
        /// </summary>
        public string MDVQHNSach { get; set; }

        /// <summary>
        /// <para>Địa danh</para>
        /// <para>Độ dài tối đa: 50</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string DDanh { get; set; }

        /// <summary>
        /// <para>Ngày thông báo</para>
        /// <para>Kiểu dữ liệu: Ngày</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string NTBao { get; set; }
        
        public DSHDon DSHDon { get; set; }
    }
}
