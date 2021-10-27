using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._3;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._5
{
    public partial class TTUNhiem
    {
        /// <summary>
        /// <para>Mã số thuế (MST tổ chức ủy nhiệm/nhận ủy nhiệm)</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(14)]
        public string MST { get; set; }

        /// <summary>
        /// <para>Mã số thuế (MST tổ chức ủy nhiệm/nhận ủy nhiệm)</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(400)]
        public string TTChuc { get; set; }

        /// <summary>
        /// <para>Mã số thuế (MST tổ chức ủy nhiệm/nhận ủy nhiệm)</para>
        /// <para>Kiểu dữ liệu: Ngày</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public string NTNhan { get; set; }

        public List<LDo> DSLDKCNhan { get; set; }

        public List<HDUNhiem> DSHDUNhiem { get; set; }
    }

    public partial class HDUNhiem
    {
        /// <summary>
        /// <para>Tên loại hóa đơn</para>
        /// <para>Độ dài tối đa: 100</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string TLHDon { get; set; }

        /// <summary>
        /// <para>Ký hiệu mẫu số hóa đơn</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số (Chi tiết tại Phụ lục II kèm theo Quy định này)</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại khoản 14, Điều 10 Nghị định số 123/2020/NĐ-CP)</para>
        /// </summary>
        [MaxLength(1)]
        public int? KHMSHDon { get; set; }

        /// <summary>
        /// <para>Ký hiệu hóa đơn</para>
        /// <para>Độ dài tối đa: 6</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại khoản 14, Điều 10 Nghị định số 123/2020/NĐ-CP)</para>
        /// </summary>
        [MaxLength(6)]
        public string KHHDon { get; set; }

        /// <summary>
        /// <para>Mục đích (Mục đích ủy nhiệm)</para>
        /// <para>Độ dài tối đa: 255</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string MDich { get; set; }

        /// <summary>
        /// <para>Từ ngày (Thời hạn ủy nhiệm từ ngày)</para>
        /// <para>Kiểu dữ liệu: Ngày</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public string TNgay { get; set; }

        /// <summary>
        /// <para>Đến ngày (Thời hạn ủy nhiệm đến ngày)</para>
        /// <para>Kiểu dữ liệu: Ngày</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public string DNgay { get; set; }
    }
}
