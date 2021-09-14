using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._6
{
    public partial class DLTBao
    {
        /// <summary>
        /// <para>Phiên bản XML (Trong Quy định này có giá trị là 2.0.0)</para>
        /// <para>Độ dài tối đa: 6</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(6)]
        public string PBan { get; set; }

        /// <summary>
        /// <para>Mẫu số (Mẫu số thông báo)</para>
        /// <para>Độ dài tối đa: 15</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(15)]
        public string MSo { get; set; }

        /// <summary>
        /// <para>Tên (Tên thông báo)"</para>
        /// <para>Độ dài tối đa: 6</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Ten { get; set; }

        /// <summary>
        /// <para>Số (Số thông báo)</para>
        /// <para>Độ dài tối đa: 30</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(30)]
        public string So { get; set; }

        /// <summary>
        /// <para>Địa danh</para>
        /// <para>Độ dài tối đa: 6</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        
        [Required]
        [MaxLength(50)]
        public string DDanh { get; set; }

        /// <summary>
        /// <para>Ngày thông báo</para>
        /// <para>Kiểu dữ liệu: Ngày</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        /// 
        [Required]
        public DateTime NTBao { get; set; }

        /// <summary>
        /// <para>Tên cơ quan thuế cấp trên</para>
        /// <para>Độ dài tối đa: 100</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(100)]    
        public string TCQTCTren { get; set; }

        /// <summary>
        /// <para>Tên cơ quan thuế (Tên cơ quan thuế ra thông báo)</para>
        /// <para>Độ dài tối đa: 100</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string TCQT { get; set; }

        /// <summary>
        /// <para>Tên người nộp thuế</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(400)]
        public string TNNT { get; set; }

        /// <summary>
        /// <para>Mã số thuế</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(14)]
        public string MST { get; set; }

        /// <summary>
        /// <para>Trường hợp</para>
        /// <para>Độ dài tối đa: 1</para>111
        /// <para>Kiểu dữ liệu: Số (1: Hết thời gian sử dụng hóa đơn có mã miễn phí, 2: Không còn thuộc trường hợp sử dụng hóa đơn điện tử không có mã)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(1)]
        public THopHetHanHDDT THop { get; set; }

        /// <summary>
        /// <para>Ngày yêu cầu chuyển đổi</para>
        /// <para>Độ dài tối đa: 6</para>
        /// <para>Kiểu dữ liệu: Ngày</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public DateTime NYCCDoi { get; set; }
    }
}
