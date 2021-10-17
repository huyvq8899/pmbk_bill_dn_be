using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._3
{
    [XmlType(TypeName = "BTHop", Namespace = "BTHopLBTHXDau")]
    public partial class BTHopLBTHXDau
    {
        /// <summary>
        /// <para>Số thứ tự</para>
        /// <para>Độ dài tối đa: 4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(4)]
        public int? STT { get; set; }

        /// <summary>
        /// <para>Kỳ dữ liệu (Kỳ dữ liệu Bảng tổng hợp, Tờ khai dữ liệu)</para>
        /// <para>Độ dài tối đa: 10</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự (Chi tiết tại Phụ lục VII kèm theo Quy định này)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string KDLieu { get; set; }

        /// <summary>
        /// <para>Lần đầu</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số (1: lần đầu, 0: bổ sung)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(1)]
        public LDau LDau { get; set; }

        /// <summary>
        /// <para>Bổ sung lần thứ</para>
        /// <para>Độ dài tối đa: 3</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc nếu LDau = 0</para>
        /// </summary>
        [MaxLength(3)]
        public int? BSLThu { get; set; }

        /// <summary>
        /// <para>Số bảng tổng hợp dữ liệu (Số thứ tự bảng tổng hợp dữ liệu)</para>
        /// <para>Độ dài tối đa: 5</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc nếu là Bảng tổng hợp</para>
        /// </summary>
        [MaxLength(5)]
        public int? SBTHDLieu { get; set; }

        public DSLDTTChung DSLDTTChung { get; set; }

        public DSLMHang DSLMHang { get; set; }
    }

    public partial class DSLMHang
    {
        public List<MHang> MHang { get; set; }
    }

    public partial class MHang
    {
        /// <summary>
        /// <para>Số thứ tự</para>
        /// <para>Độ dài tối đa: 4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(4)]
        public int? STT { get; set; }

        /// <summary>
        /// <para>Mã hàng hóa, dịch vụ</para>
        /// <para>Độ dài tối đa: 50</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string MHHoa { get; set; }

        /// <summary>
        /// <para>Tên hàng hóa, dịch vụ (Mặt hàng)</para>
        /// <para>Độ dài tối đa: 500</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string THHDVu { get; set; }

        /// <summary>
        /// <para>Kỳ điều chỉnh</para>
        /// <para>Độ dài tối đa: 10</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự (Chi tiết tại Phụ lục VII kèm theo Quy định này)</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(10)]
        public string KDLieu { get; set; }

        public List<LDo> DSLDo { get; set; }
    }
}
