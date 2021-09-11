using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._5
{
    public partial class DSTTUNhiem
    {
        public List<TTUNhiem> TTUNhiem { get; set; }
    }

    public partial class TTUNhiem
    {
        /// <summary>
        /// <para>Mã số thuế (MST tổ chức ủy nhiệm/nhận ủy nhiệm)</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string MST { get; set; }

        /// <summary>
        /// <para>Mã số thuế (MST tổ chức ủy nhiệm/nhận ủy nhiệm)</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string TTChuc { get; set; }

        /// <summary>
        /// <para>Mã số thuế (MST tổ chức ủy nhiệm/nhận ủy nhiệm)</para>
        /// <para>Kiểu dữ liệu: Ngày</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public string NTNhan { get; set; }

        /// <summary>
        /// <para>Kết quả xử lý của CQT</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số (1 Chấp nhận, 2 Không chấp nhận)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public KQua KQua { get; set; }

        public DSLDKCNhan DSLDKCNhan { get; set; }

        public DSHDUNhiem DSHDUNhiem { get; set; }
    }

    public partial class DSLDKCNhan
    {
        public List<LDo> LDo { get; set; }
    }

    public partial class LDo
    {
        /// <summary>
        /// <para>Mã lỗi (Mã tiêu chí)</para>
        /// <para>Độ dài tối đa: 4</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string MLoi { get; set; }

        /// <summary>
        /// <para>Mô tả (Lý do không chấp nhận)</para>
        /// <para>Độ dài tối đa: 100</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string MTa { get; set; }

        /// <summary>
        /// <para>Hướng dẫn xử lý</para>
        /// <para>Độ dài tối đa: 255</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public string HDXLy { get; set; }

        /// <summary>
        /// <para>Ghi chú</para>
        /// <para>Độ dài tối đa: 255</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public string GChu { get; set; }
    }

    public partial class DSHDUNhiem
    {
        public List<HDUNhiem> HDUNhiem { get; set; }
    }

    public partial class HDUNhiem
    {
        /// <summary>
        /// <para>Tên loại hóa đơn</para>
        /// <para>Độ dài tối đa: 100</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string TLHDon { get; set; }

        /// <summary>
        /// <para>Ký hiệu mẫu số hóa đơn</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số (Chi tiết tại Phụ lục II kèm theo Quy định này)</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại khoản 14, Điều 10 Nghị định số 123/2020/NĐ-CP)</para>
        /// </summary>
        public int? KHMSHDon { get; set; }

        /// <summary>
        /// <para>Ký hiệu hóa đơn</para>
        /// <para>Độ dài tối đa: 6</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại khoản 14, Điều 10 Nghị định số 123/2020/NĐ-CP)</para>
        /// </summary>
        public string KHHDon { get; set; }

        /// <summary>
        /// <para>Mục đích (Mục đích ủy nhiệm)</para>
        /// <para>Độ dài tối đa: 255</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string MDich { get; set; }

        /// <summary>
        /// <para>Từ ngày (Thời hạn ủy nhiệm từ ngày)</para>
        /// <para>Kiểu dữ liệu: Ngày</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string TNgay { get; set; }

        /// <summary>
        /// <para>Đến ngày (Thời hạn ủy nhiệm đến ngày)</para>
        /// <para>Kiểu dữ liệu: Ngày</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string DNgay { get; set; }
    }
}
