using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a;
using System.Collections.Generic;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.f
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
        public TChat TChat { get; set; }

        /// <summary>
        /// <para>Số thứ tự</para>
        /// <para>Độ dài tối đa: 4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public int? STT { get; set; }

        /// <summary>
        /// <para>Mã hàng hóa, dịch vụ</para>
        /// <para>Độ dài tối đa: 50</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc (Trừ trường hợp quy định tại điểm a, khoản 6, Điều 10 Nghị định số 123/2020/NĐ-CP)</para>
        /// </summary>
        public string MHHDVu { get; set; }

        /// <summary>
        /// <para>Tên hàng hóa, dịch vụ</para>
        /// <para>Độ dài tối đa: 500</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string THHDVu { get; set; }

        /// <summary>
        /// <para>Đơn vị tính</para>
        /// <para>Độ dài tối đa: 50</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (trừ trường hợp TChat có giá trị là “2-Khuyến mại, 4-Ghi chú, diễn giải”)</para>
        /// </summary>
        public string DVTinh { get; set; }

        /// <summary>
        /// <para>Số lượng</para>
        /// <para>Độ dài tối đa: 19,4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc (trừ trường hợp TChat có giá trị là “2-Khuyến mại, 4-Ghi chú, diễn giải”)</para>
        /// </summary>
        public decimal? SLuong { get; set; }

        /// <summary>
        /// <para>Đơn giá</para>
        /// <para>Độ dài tối đa: 19,4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public decimal? DGia { get; set; }

        /// <summary>
        /// <para>Thành tiền (Thành tiền chưa có thuế GTGT)</para>
        /// <para>Độ dài tối đa: 19,4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public decimal? ThTien { get; set; }

        public TTKhac TTKhac { get; set; }
    }
}
