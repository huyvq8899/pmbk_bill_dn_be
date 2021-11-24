using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h
{
    public partial class DSHHDVu
    {
        public List<HHDVu> HHDVu { get; set; }
    }

    public partial class HHDVu
    {
        /// <summary>
        /// <para>Tính chất</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số (Chi tiết tại Phụ lục IV kèm theo Quy định này)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(1)]
        public TChat TChat { get; set; }

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
        /// <para>Không bắt buộc (Trừ trường hợp quy định tại điểm a, khoản 6, Điều 10 Nghị định số 123/2020/NĐ-CP)</para>
        /// </summary>
        [MaxLength(50)]
        public string MHHDVu { get; set; }

        /// <summary>
        /// <para>Tên hàng hóa, dịch vụ</para>
        /// <para>Độ dài tối đa: 500</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string THHDVu { get; set; }

        /// <summary>
        /// <para>Đơn vị tính</para>
        /// <para>Độ dài tối đa: 50</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc (Đối với trường hợp tem, vé, thẻ điện tử có sẵn mệnh giá)</para>
        /// </summary>
        [MaxLength(50)]
        public string DVTinh { get; set; }

        /// <summary>
        /// <para>Số lượng</para>
        /// <para>Độ dài tối đa: 19,4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Không bắt buộc (Đối với trường hợp tem, vé, thẻ điện tử có sẵn mệnh giá)</para>
        /// </summary>
        [Column(TypeName = "decimal(19, 4)")]
        public decimal? SLuong { get; set; }

        /// <summary>
        /// <para>Đơn giá</para>
        /// <para>Độ dài tối đa: 19,4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Không bắt buộc (Đối với trường hợp tem, vé, thẻ điện tử có sẵn mệnh giá)</para>
        /// </summary>
        [Column(TypeName = "decimal(19, 4)")]
        public decimal? DGia { get; set; }

        /// <summary>
        /// <para>Tỷ lệ % chiết khấu (Trong trường hợp muốn thể hiện thông tin chiết khấu theo cột cho từng hàng hóa, dịch vụ)</para>
        /// <para>Độ dài tối đa: 6,4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Không Bắt buộc</para>
        /// </summary>
        [Column(TypeName = "decimal(6, 4)")]
        public decimal? TLCKhau { get; set; }

        /// <summary>
        /// <para>Số tiền chiết khấu (Trong trường hợp muốn thể hiện thông tin chiết khấu theo cột cho từng hàng hóa, dịch vụ)</para>
        /// <para>Độ dài tối đa: 19,4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Không Bắt buộc</para>
        /// </summary>
        [Column(TypeName = "decimal(19, 4)")]
        public decimal? STCKhau { get; set; }

        /// <summary>
        /// <para>Thành tiền (thành tiền chưa có thuế GTGT đối với hóa đơn khác thuộc loại hóa đơn GTGT, thành tiền đối với hóa đơn khác thuộc loại hóa đơn bán hàng)</para>
        /// <para>Độ dài tối đa: 19,4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(19, 4)")]
        public decimal? ThTien { get; set; }

        /// <summary>
        /// <para>Thuế suất (Thuế suất thuế GTGT đối với hóa đơn khác thuộc loại hóa đơn GTGT, đối với hóa đơn khác thuộc loại hóa đơn bán hàng không có thẻ này)</para>
        /// <para>Độ dài tối đa: 10</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự (Chi tiết tại Phụ lục V kèm theo Quy định này)</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(10)]
        public string TSuat { get; set; }

        public List<TTin> TTKhac { get; set; }
    }
}
