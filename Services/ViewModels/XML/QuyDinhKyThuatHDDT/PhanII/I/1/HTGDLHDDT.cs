using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Attributes;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1
{
    public partial class HTGDLHDDT
    {
        [Required]
        [MaxLength(1)]
        [Display(Name = "NNT địa bàn khó khăn (Doanh nghiệp nhỏ và vừa, hợp tác xã, hộ, cá nhân kinh doanh tại địa bàn có điều kiện kinh tế xã hội khó khăn, địa bàn có điều kiện kinh tế xã hội đặc biệt khó khăn)")]
        [CustomDataType(CustomDataType.Number)]
        public ADung NNTDBKKhan { get; set; }

        [Required]
        [MaxLength(1)]
        [Display(Name = "NNT khác theo đề nghị UBND (Doanh nghiệp nhỏ và vừa khác theo đề nghị của Ủy ban nhân dân tỉnh, thành phố trực thuộc trung ương gửi Bộ Tài chính trừ doanh nghiệp hoạt động tại các khu kinh tế, khu công nghiệp, khu công nghệ cao)")]
        [CustomDataType(CustomDataType.Number)]
        public ADung NNTKTDNUBND { get; set; }

        [Required]
        [MaxLength(1)]
        [Display(Name = "Chuyển dữ liệu trực tiếp đến CQT (Chuyển dữ liệu hóa đơn điện tử trực tiếp đến cơ quan thuế (điểm b1, khoản 3, Điều 22 của Nghị định))")]
        [CustomDataType(CustomDataType.Number)]
        public ADung CDLTTDCQT { get; set; }

        [Required]
        [MaxLength(1)]
        [Display(Name = "Chuyển dữ liệu qua T-VAN (Thông qua tổ chức cung cấp dịch vụ hóa đơn điện tử (điểm b2, khoản 3, Điều 22 của Nghị định))")]
        [CustomDataType(CustomDataType.Number)]
        public ADung CDLQTVAN { get; set; }
    }
}
