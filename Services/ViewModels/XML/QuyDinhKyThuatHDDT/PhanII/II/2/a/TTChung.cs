﻿using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a
{
    public partial class TTChung
    {
        /// <summary>
        /// <para>Phiên bản XML (Trong Quy định này có giá trị là 2.0.0)</para>
        /// <para>Độ dài tối đa: 6</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string PBan { get; set; }

        /// <summary>
        /// <para>Tên hóa đơn</para>
        /// <para>Độ dài tối đa: 100</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại khoản 14, Điều 10 Nghị định số 123/2020/NĐ-CP)</para>
        /// </summary>
        public string THDon { get; set; }

        /// <summary>
        /// <para>Ký hiệu mẫu số hóa đơn</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự (Chi tiết tại Phụ lục II kèm theo Quy định này)</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại khoản 14, Điều 10 Nghị định số 123/2020/NĐ-CP)</para>
        /// </summary>
        public string KHMSHDon { get; set; }

        /// <summary>
        /// <para>Ký hiệu hóa đơn</para>
        /// <para>Độ dài tối đa: 6</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại khoản 14, Điều 10 Nghị định số 123/2020/NĐ-CP)</para>
        /// </summary>
        public string KHHDon { get; set; }

        /// <summary>
        /// <para>Số hóa đơn</para>
        /// <para>Độ dài tối đa: 8</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại khoản 14, Điều 10 Nghị định số 123/2020/NĐ-CP)</para>
        /// </summary>
        public int? SHDon { get; set; }

        /// <summary>
        /// <para>Mã hồ sơ</para>
        /// <para>Độ dài tối đa: 20</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Đối với trường hợp là hoá đơn đề nghị cấp mã của cơ quan thuế theo từng lần phát sinh)</para>
        /// </summary>
        public string MHSo { get; set; }

        /// <summary>
        /// <para>Ngày lập</para>
        /// <para>Kiểu dữ liệu: Ngày</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string NLap { get; set; }

        /// <summary>
        /// <para>Hóa đơn xuất khẩu</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số (1-Hóa đơn xuất khẩu, 0- Không phải Hóa đơn xuất khẩu)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public HDXKhau HDXKhau { get; set; }

        /// <summary>
        /// <para>Hóa đơn xuất vào khu phi thuế quan</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số (1-Hóa đơn xuất vào khu phi thuế quan, 0- Không phải Hóa đơn xuất vào khu phi thuế quan)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public HDXKPTQuan HDXKPTQuan { get; set; }

        /// <summary>
        /// <para>Số bảng kê (Bảng kê các loại hàng hóa, dịch vụ đã bán kèm theo hóa đơn)</para>
        /// <para>Độ dài tối đa: 50</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Đối với dịch vụ xuất theo kỳ phát sinh)</para>
        /// </summary>
        public string SBKe { get; set; }

        /// <summary>
        /// <para>Ngày bảng kê</para>
        /// <para>Kiểu dữ liệu: Ngày</para>
        /// <para>Bắt buộc (Đối với dịch vụ xuất theo kỳ phát sinh)</para>
        /// </summary>
        public string NBKe { get; set; }

        /// <summary>
        /// <para>Đơn vị tiền tệ</para>
        /// <para>Độ dài tối đa: 3</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự (Chi tiết tại Khoản 2, Mục IV, Phần I)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string DVTTe { get; set; }

        /// <summary>
        /// <para>Tỷ giá</para>
        /// <para>Độ dài tối đa: 7,2</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc (Trừ trường hợp Đơn vị tiền tệ là VNĐ)</para>
        /// </summary>
        public decimal? TGia { get; set; }

        /// <summary>
        /// <para>Hình thức thanh toán</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số (Chi tiết tại Phụ lục XI kèm theo Quy định này)</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public HTTToan HTTToan { get; set; }

        /// <summary>
        /// <para>Tên hình thức thanh toán khác</para>
        /// <para>Độ dài tối đa: 50</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc (Bắt buộc trong trường hợp hình thức thanh toán là khác)</para>
        /// </summary>
        public string THTTTKhac { get; set; }

        /// <summary>
        /// <para>Mã số thuế đơn vị cung cấp hóa đơn điện tử</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public string MSTDVCCHDDTu { get; set; }

        /// <summary>
        /// <para>Mã số thuế đơn vị nhận ủy nhiệm lập hóa đơn</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public string MSTDVNUNLHDon { get; set; }

        /// <summary>
        /// <para>Tên đơn vị nhận ủy nhiệm lập hóa đơn</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public string TDVNUNLHDon { get; set; }

        public TTHDLQuan TTHDLQuan { get; set; }

        public TTKhac TTKhac { get; set; }
    }
}