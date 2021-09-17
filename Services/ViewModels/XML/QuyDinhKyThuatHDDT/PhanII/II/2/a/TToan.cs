using System.Collections.Generic;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a
{
    public partial class TToan
    {
        /// <summary>
        /// <para>Tổng tiền chưa thuế (Tổng cộng thành tiền chưa có thuế GTGT)</para>
        /// <para>Độ dài tối đa: 19,4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public decimal TgTCThue { get; set; }

        /// <summary>
        /// <para>Tổng tiền thuế (Tổng cộng tiền thuế GTGT)</para>
        /// <para>Độ dài tối đa: 19,4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public decimal TgTThue { get; set; }

        /// <summary>
        /// <para>Tổng tiền chiết khấu thương mại</para>
        /// <para>Độ dài tối đa: 19,4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Không bắt buộc (Trừ trường hợp quy định tại điểm đ, khoản 6, Điều 10 Nghị định số 123/2020/NĐ-CP)</para>
        /// </summary>
        public decimal? TTCKTMai { get; set; }

        /// <summary>
        /// <para>Tổng tiền thanh toán bằng số</para>
        /// <para>Độ dài tối đa: 19,4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public decimal TgTTTBSo { get; set; }

        /// <summary>
        /// <para>Tổng tiền thanh toán bằng chữ</para>
        /// <para>Độ dài tối đa: 255</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public decimal TgTTTBChu { get; set; }

        public THTTLTSuat THTTLTSuat { get; set; }

        public DSLPhi DSLPhi { get; set; }

        public TTKhac TTKhac { get; set; }
    }

    public partial class THTTLTSuat
    {
        public List<LTSuat> LTSuat { get; set; }
    }

    public partial class LTSuat
    {
        /// <summary>
        /// <para>Thuế suất (Thuế suất thuế GTGT)</para>
        /// <para>Độ dài tối đa: 10</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự (Chi tiết tại Phụ lục V kèm theo Quy định này)</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại khoản 14, Điều 10 Nghị định số 123/2020/NĐ-CP và trường hợp TChat có giá trị là “2-Khuyến mại, 4-Ghi chú, diễn giải”)</para>
        /// </summary>
        public decimal? TSuat { get; set; }

        /// <summary>
        /// <para>Thành tiền (Thành tiền chưa có thuế GTGT)</para>
        /// <para>Độ dài tối đa: 19,4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public decimal ThTien { get; set; }

        /// <summary>
        /// <para>Tiền thuế (Tiền thuế GTGT)</para>
        /// <para>Độ dài tối đa: 19,4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại khoản 14, Điều 10 Nghị định số 123/2020/NĐ-CP)</para>
        /// </summary>
        public decimal? TThue { get; set; }
    }

    public partial class DSLPhi
    {
        public List<LPhi> LPhi { get; set; }
    }

    public partial class LPhi
    {
        /// <summary>
        /// <para>Tên loại phí</para>
        /// <para>Độ dài tối đa: 100</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public string TLPhi { get; set; }

        /// <summary>
        /// <para>Tiền phí</para>
        /// <para>Độ dài tối đa: 19,4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public decimal? TPhi { get; set; }
    }
}
