using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Attributes;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1
{
    public partial class TTChung
    {
        [Required]
        [MaxLength(6)]
        [Display(Name = "Phiên bản XML (Trong Quy định này có giá trị là 2.0.0)")]
        [CustomDataType(CustomDataType.String)]
        public string PBan { get; set; }

        [Required]
        [MaxLength(15)]
        [Display(Name = "Mẫu số (Mẫu số tờ khai)")]
        [CustomDataType(CustomDataType.String)]
        public string MSo { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Tên (Tên tờ khai)")]
        [CustomDataType(CustomDataType.String)]
        public string Ten { get; set; }

        [Required]
        [MaxLength(1)]
        [Display(Name = "Hình thức (Hình thức đăng ký/thay đổi thông tin sử dụng hóa đơn điện tử)")]
        [CustomDataType(CustomDataType.Number)]
        public HThuc HThuc { get; set; }

        [Required]
        [MaxLength(400)]
        [Display(Name = "Tên NNT")]
        [CustomDataType(CustomDataType.String)]
        public string TNNT { get; set; }

        [Required]
        [MaxLength(14)]
        [Display(Name = "Mã số thuế")]
        [CustomDataType(CustomDataType.String)]
        public string MST { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "CQT quản lý")]
        [CustomDataType(CustomDataType.String)]
        public string CQTQLy { get; set; }

        [Required]
        [MaxLength(5)]
        [Display(Name = "Mã CQT quản lý")]
        [CustomDataType(CustomDataType.String)]
        public string MCQTQLy { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Người liên hệ")]
        [CustomDataType(CustomDataType.String)]
        public string NLHe { get; set; }

        [Required]
        [MaxLength(400)]
        [Display(Name = "Địa chỉ liên hệ")]
        [CustomDataType(CustomDataType.String)]
        public string DCLHe { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Địa chỉ thư điện tử")]
        [CustomDataType(CustomDataType.String)]
        public string DCTDTu { get; set; }

        [Required]
        [MaxLength(20)]
        [Display(Name = "Điện thoại liên hệ")]
        [CustomDataType(CustomDataType.String)]
        public string DTLHe { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Địa danh")]
        [CustomDataType(CustomDataType.String)]
        public string DDanh { get; set; }

        [Required]
        [Display(Name = "Ngày lập")]
        [CustomDataType(CustomDataType.Date)]
        public string NLap { get; set; }
    }
}
