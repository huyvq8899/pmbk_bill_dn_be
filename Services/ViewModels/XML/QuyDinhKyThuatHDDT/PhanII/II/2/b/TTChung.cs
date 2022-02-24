using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.b
{
    public partial class TTChung
    {
        /// <summary>
        /// <para>Phiên bản XML (Trong Quy định này có giá trị là 2.0.0)</para>
        /// <para>Độ dài tối đa: 6</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(6)]
        public string PBan { get; set; }

        /// <summary>
        /// <para>Tên hóa đơn</para>
        /// <para>Độ dài tối đa: 100</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại khoản 14, Điều 10 Nghị định số 123/2020/NĐ-CP)</para>
        /// </summary>
        [MaxLength(100)]
        public string THDon { get; set; }

        /// <summary>
        /// <para>Ký hiệu mẫu số hóa đơn</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự (Chi tiết tại Phụ lục II kèm theo Quy định này)</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại khoản 14, Điều 10 Nghị định số 123/2020/NĐ-CP)</para>
        /// </summary>
        [MaxLength(1)]
        public string KHMSHDon { get; set; }

        /// <summary>
        /// <para>Ký hiệu hóa đơn</para>
        /// <para>Độ dài tối đa: 6</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại khoản 14, Điều 10 Nghị định số 123/2020/NĐ-CP)</para>
        /// </summary>
        [MaxLength(6)]
        public string KHHDon { get; set; }

        /// <summary>
        /// <para>Số hóa đơn</para>
        /// <para>Độ dài tối đa: 8</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại khoản 14, Điều 10 Nghị định số 123/2020/NĐ-CP)</para>
        /// </summary>
        [MaxLength(8)]
        public long? SHDon { get; set; }

        /// <summary>
        /// <para>Mã hồ sơ</para>
        /// <para>Độ dài tối đa: 20</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Đối với trường hợp là hóa đơn đề nghị cấp mã của cơ quan thuế theo từng lần phát sinh)</para>
        /// </summary>
        [MaxLength(20)]
        public string MHSo { get; set; }

        /// <summary>
        /// <para>Ngày lập</para>
        /// <para>Kiểu dữ liệu: Ngày</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public string NLap { get; set; }

        /// <summary>
        /// <para>Hóa đơn dành cho khu phi thuế quan (Hóa đơn dành cho tổ chức, cá nhân trong khu phi thuế quan)</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số (1-Hóa đơn GTGT kiêm tờ khai hoàn thuế)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(1)]
        public HDDCKPTQuan HDDCKPTQuan { get; set; }

        /// <summary>
        /// <para>Số bảng kê (Bảng kê các loại hàng hóa, dịch vụ đã bán kèm theo hóa đơn)</para>
        /// <para>Độ dài tối đa: 50</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Nếu có)</para>
        /// <para>Thao khảo: điểm a khoản 6 điều 10 ND 123</para>
        [MaxLength(50)]
        public string SBKe { get; set; }

        /// <summary>
        /// <para>Ngày bảng kê</para>
        /// <para>Kiểu dữ liệu: Ngày</para>
        /// <para>Bắt buộc (Nếu có)</para>
        /// <para>Thao khảo: điểm a khoản 6 điều 10 ND 123</para>
        /// </summary>
        public string NBKe { get; set; }

        /// <summary>
        /// <para>Đơn vị tiền tệ</para>
        /// <para>Độ dài tối đa: 3</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự (Chi tiết tại Khoản 2, Mục IV, Phần I)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(3)]
        public string DVTTe { get; set; }

        /// <summary>
        /// <para>Tỷ giá</para>
        /// <para>Độ dài tối đa: 7,2</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc (Trừ trường hợp Đơn vị tiền tệ là VNĐ)</para>
        /// </summary>
        [Column(TypeName = "decimal(7, 2)")]
        public decimal? TGia { get; set; }

        /// <summary>
        /// <para>Hình thức thanh toán</para>
        /// <para>Độ dài tối đa: 50</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(50)]
        public string HTTToan { get; set; }

        /// <summary>
        /// <para>Mã số thuế tổ chức cung cấp giải pháp hóa đơn điện tử</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(14)]
        public string MSTTCGP { get; set; }

        /// <summary>
        /// <para>Mã số thuế đơn vị nhận ủy nhiệm lập hóa đơn</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Đối với trường hợp ủy nhiệm lập hóa đơn)</para>
        /// </summary>
        [MaxLength(14)]
        public string MSTDVNUNLHDon { get; set; }

        /// <summary>
        /// <para>Tên đơn vị nhận ủy nhiệm lập hóa đơn</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Đối với trường hợp ủy nhiệm lập hóa đơn)</para>
        /// </summary>
        [MaxLength(400)]
        public string TDVNUNLHDon { get; set; }

        /// <summary>
        /// <para>Địa chỉ đơn vị nhận ủy nhiệm lập hóa đơn</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Đối với trường hợp ủy nhiệm lập hóa đơn)</para>
        /// </summary>
        public string DCDVNUNLHDon { get; set; }

        public TTHDLQuan TTHDLQuan { get; set; }

        public List<TTin> TTKhac { get; set; }
    }
}
