using System;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._7
{
    public partial class TTChung
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
        /// <para>Mẫu số (Mẫu số đề nghị)</para>
        /// <para>Độ dài tối đa: 15</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(15)]
        public string MSo { get; set; }

        /// <summary>
        /// <para>Tên (Tên đề nghị)</para>
        /// <para>Độ dài tối đa: 100</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Ten { get; set; }

        /// <summary>
        /// <para>Tên cơ quan thuế (Tên cơ quan thuế cấp hóa đơn)</para>
        /// <para>Độ dài tối đa: 100</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string TCQT { get; set; }

        /// <summary>
        /// <para>Mã CQT (Mã cơ quan thuế cấp hóa đơn)</para>
        /// <para>Độ dài tối đa: 5</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(5)]
        public string MCQT { get; set; }

        /// <summary>
        /// <para>Tên tổ chức, cá nhân</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(400)]
        public string TTCCNhan { get; set; }

        /// <summary>
        /// <para>Địa chỉ liên hệ</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(400)]
        public string DCLHe { get; set; }

        /// <summary>
        /// <para>Địa chỉ thư điện tử</para>
        /// <para>Độ dài tối đa: 50</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [MaxLength(50)]
        public string DCTDTu { get; set; }

        /// <summary>
        /// <para>Điện thoại liên hệ</para>
        /// <para>Độ dài tối đa: 20</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string DTLHe { get; set; }

        /// <summary>
        /// <para>Số quyết định thành lập (Số quyết định thành lập tổ chức)</para>
        /// <para>Độ dài tối đa: 50</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(50)]
        public string SQDTLap { get; set; }

        /// <summary>
        /// <para>Ngày cấp quyết định thành lập (Ngày cấp quyết định thành lập tổ chức)</para>
        /// <para>Kiểu dữ liệu: Ngày</para>
        /// <para> Không bắt buộc</para>
        /// </summary>
        public string NCQDTLap { get; set; }

        /// <summary>
        /// <para>Cơ quan cấp quyết định thành lập (Cơ quan cấp quyết định thành lập tổ chức)</para>
        /// <para>Độ dài tối đa: 200</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(200)]
        public string CQCQDTLap { get; set; }

        /// <summary>
        /// <para>Mã số thuế</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(14)]
        public string MST { get; set; }

        /// <summary>
        /// <para>Tên người nhận hóa đơn</para>
        /// <para>Độ dài tối đa: 100</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string TNNHDon { get; set; }

        /// <summary>
        /// <para>CMND (Số CMND /CCCD/Hộ chiếu người đi nhận hóa đơn người đi nhận hóa đơn)</para>
        /// <para>Độ dài tối đa: 20</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string CMND { get; set; }

        /// <summary>
        /// <para>Ngày cấp CMND (Ngày cấp CMND /CCCD/Hộ chiếu người đi nhận hóa đơn người đi nhận hóa đơn)</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para>Kiểu dữ liệu: Ngày</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public string NgCCMND { get; set; }

        /// <summary>
        /// <para>Nơi cấp CMND (Nơi cấp CMND /CCCD/Hộ chiếu người đi nhận hóa đơn người đi nhận hóa đơn)</para>
        /// <para>Độ dài tối đa: 100</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string NCCMND { get; set; }

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
        /// <para>Ngày lập</para>
        /// <para>Kiểu dữ liệu: Ngày</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public string NLap { get; set; }
    }
}
