using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._3
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
        public string PBan { get; set; } = "2.0.0";

        /// <summary>
        /// <para>Mẫu số (Mẫu số thông báo)</para>
        /// <para>Độ dài tối đa: 15</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự (Chi tiết tại Phụ lục VIII kèm theo Quy định này)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(15)]
        public string MSo { get; set; }

        /// <summary>
        /// <para>Tên (Tên thông báo)</para>
        /// <para>Độ dài tối đa: 255</para>
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
        /// <para>Độ dài tối đa: 50</para>
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
        [Required]
        public string NTBao { get; set; }

        /// <summary>
        /// <para>Mã số thuế</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(14)]
        public string MST { get; set; }

        /// <summary>
        /// <para>Tên NNT</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(400)]
        public string TNNT { get; set; }

        /// <summary>
        /// <para>Tên tờ khai (Tên tờ khai NNT gửi tới CQT)</para>
        /// <para>Độ dài tối đa: 100</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string TTKhai { get; set; }

        /// <summary>
        /// <para>Mã giao dịch điện tử</para>
        /// <para>Độ dài tối đa: 46</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(46)]
        public string MGDDTu { get; set; }

        /// <summary>
        /// <para>Thời gian gửi (Thời gian NNT gửi tờ khai tới CQT)</para>
        /// <para>Kiểu dữ liệu: Ngày giờ</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public string TGGui { get; set; }

        /// <summary>
        /// <para>Trường hợp (Trường hợp tiếp nhận/không tiếp nhận của CQT)</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số (Chi tiết tại Phụ lục XII kèm theo Quy định này)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(1)]
        public THop THop { get; set; }

        /// <summary>
        /// <para>Thời gian nhận (Thời gian CQT tiếp nhận)</para>
        /// <para>Kiểu dữ liệu: Ngày giờ</para>
        /// <para>Bắt buộc (Trừ trường hợp Chỉ tiêu THop có giá trị là 2 hoặc 4)</para>
        /// </summary>
        public string TGNhan { get; set; }

        public List<LDo> DSLDKCNhan { get; set; }
    }
}
