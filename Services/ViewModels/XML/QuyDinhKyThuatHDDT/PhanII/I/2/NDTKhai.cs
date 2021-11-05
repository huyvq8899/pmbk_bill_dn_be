using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._2
{
    public partial class NDTKhai
    {
        public List<CTS> DSCTSSDung { get; set; }
        public List<DKUNhiem> DSDKUNhiem { get; set; }
    }

    public partial class CTS
    {
        /// <summary>
        /// <para>Số thứ tự</para>
        /// <para>Độ dài tối đa: 3</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public int? STT { get; set; }

        /// <summary>
        /// <para>Tên tổ chức (Cơ quan chứng thực/cấp/công nhận chữ ký số, chữ ký điện tử)</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(400)]
        public string TTChuc { get; set; }

        /// <summary>
        /// <para>Seri (Số sê-ri chứng thư số)</para>
        /// <para>Độ dài tối đa: 40</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(40)]
        public string Seri { get; set; }

        /// <summary>
        /// <para>Từ ngày (Thời hạn sử dụng chứng thư số từ ngày)</para>
        /// <para>Kiểu dữ liệu: Ngày giờ</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public string TNgay { get; set; }

        /// <summary>
        /// <para>Đến ngày (Thời hạn sử dụng chứng thư số đến ngày)</para>
        /// <para>Kiểu dữ liệu: Ngày giờ</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public string DNgay { get; set; }

        /// <summary>
        /// <para>Hình thức (Hình thức đăng ký)</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số(1: Thêm mới, 2: Gia hạn, 3: Ngừng sử dụng)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public int HThuc { get; set; }
    }

    public partial class DKUNhiem
    {
        /// <summary>
        /// <para>Số thứ tự</para>
        /// <para>Độ dài tối đa: 3</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public int? STT { get; set; }

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
        [Required]
        [MaxLength(6)]
        public string KHHDon { get; set; }

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
        /// <para>Tên tổ chức (Tên tổ chức ủy nhiệm/nhận ủy nhiệm)</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(400)]
        public string TTChuc { get; set; }

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
        /// <para>Kiểu dữ liệu: Ngày giờ</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public string TNgay { get; set; }

        /// <summary>
        /// <para>Đến ngày (Thời hạn ủy nhiệm đến ngày)</para>
        /// <para>Kiểu dữ liệu: Ngày giờ</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public string DNgay { get; set; }

        /// <summary>
        /// <para>Phương thức (Phương thức thanh toán hóa đơn ủy nhiệm)</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số (Chi tiết tại Phụ lục XI kèm theo Quy định này)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public int PThuc { get; set; }

        /// <summary>
        /// <para>Tên hình thức thanh toán khác</para>
        /// <para>Độ dài tối đa: 50</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc (Bắt buộc trong trường hợp hình thức thanh toán là khác)</para>
        /// </summary>
        [MaxLength(50)]
        public string THTTTKhac { get; set; }
    }
}
