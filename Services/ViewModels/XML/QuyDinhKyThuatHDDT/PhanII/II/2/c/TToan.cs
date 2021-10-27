using Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.c
{
    public partial class TToan
    {
        /// <summary>
        /// <para>Tổng tiền chiết khấu thương mại</para>
        /// <para>Độ dài tối đa: 19,4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Không bắt buộc (Trừ trường hợp quy định tại điểm đ, khoản 6, Điều 10 Nghị định số 123/2020/NĐ-CP)</para>
        /// </summary>
        [Column(TypeName = "decimal(19, 4)")]
        public decimal? TTCKTMai { get; set; }

        /// <summary>
        /// <para>Tổng tiền thanh toán bằng số</para>
        /// <para>Độ dài tối đa: 19,4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(19, 4)")]
        public decimal TgTTTBSo { get; set; }

        /// <summary>
        /// <para>Tổng tiền thanh toán bằng chữ</para>
        /// <para>Độ dài tối đa: 255</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(19, 4)")]
        public decimal TgTTTBChu { get; set; }

        public List<LPhi> DSLPhi { get; set; }

        public TTKhac TTKhac { get; set; }
    }
}
