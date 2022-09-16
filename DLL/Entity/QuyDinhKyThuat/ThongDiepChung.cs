using DLL.Entity.QuanLy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DLL.Entity.QuyDinhKyThuat
{
    public class ThongDiepChung : ThongTinChung
    {
        /// <summary>
        /// Id thông điệp
        /// Kiểu dữ liệu: string
        /// Bắt buộc
        /// </summary>
        public string ThongDiepChungId { get; set; }

        /// <summary>
        /// Phiên bản. Giá trị mặc định là 2.0.0
        /// Độ dài tối đa: 6
        /// Kiểu dữ liệu: chuỗi ký tự
        /// </summary>
        [MaxLength(6)]
        public string PhienBan { get; set; }

        /// <summary>
        /// <para>Mã nơi gửi</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [MaxLength(14)]
        public string MaNoiGui { get; set; }

        /// <summary>
        /// <para>Mã nơi nhận</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [MaxLength(14)]
        public string MaNoiNhan { get; set; }

        /// <summary>
        /// <para>Mã loại thông điệp</para>
        /// <para>Độ dài tối đa: 3</para>
        /// <para>Kiểu dữ liệu: số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public int MaLoaiThongDiep { get; set; }

        /// <summary>
        /// <para>Mã thông điệp</para>
        /// <para>Độ dài tối đa: 46</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [MaxLength(46)]
        public string MaThongDiep { get; set; }

        /// <summary>
        /// <para> Mã thông điệp tham chiếu</para>
        /// <para>Độ dài tối đa: 46</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(46)]
        public string MaThongDiepThamChieu { get; set; }

        /// <summary>
        /// <para>Mã số thuế (MST của NNT)</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [MaxLength(14)]
        public string MaSoThue { get; set; }

        /// <summary>
        /// <para>Số lượng</para>
        /// <para>Độ dài tối đa: 7</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public int SoLuong { get; set; }

        /// <summary>
        /// <para>Thông điệp gửi đi hay gửi về</para>
        /// <para>Kiểu dữ liệu: boolean</para>
        /// </summary>
        public bool ThongDiepGuiDi { get; set; }

        /// <summary>
        /// Hình thức (100, 101: đăng ký/thay đổi thông tin, 300: chính thức)
        /// Kiểu dữ liệu: số (1: đăng ký (với 100, 101), chính thức (với 300); 2: thay đổi thông tin (chỉ áp dụng cho 100, 101)
        /// </summary>
        public int? HinhThuc { get; set; }

        /// <summary>
        /// Trạng thái gửi thông điệp
        /// Kiểu dữ liệu: số
        /// </summary>
        public int? TrangThaiGui { get; set; }

        /// <summary>
        /// Thời gian gửi thông điệp
        /// Kiểu dữ liệu: Ngày tháng
        /// </summary>
        public DateTime? NgayGui { get; set; }

        /// <summary>
        /// File XML lưu trữ thông điệp (chưa ký, đã ký)
        /// Kiểu dữ liệu: string
        /// </summary>
        public string FileXML { get; set; }

        /// <summary>
        /// Ngày thông báo phản hồi
        /// Kiểu dữ liệu: ngày tháng
        /// </summary>
        public DateTime? NgayThongBao { get; set; } // ngày thông báo phản hồi

        /// <summary>
        /// Mã thông điệp phản hồi
        /// Kiểu dữ liệu: string
        /// </summary>
        public string MaThongDiepPhanHoi { get; set; }

        /// <summary>
        /// Id tham chiếu đến thực thể được đóng gói trong thông điệp (thông báo,tờ khai, etc...)
        /// Kiểu dữ liệu: string
        /// </summary>
        public string IdThamChieu { get; set; } // tham chiếu đến thực thể được đóng gói trong thông điệp (thông báo, tờ khai, etc...)

        /// <summary>
        /// Số thông báo phản hồi của CQT 
        /// Kiểu dữ liệu: string
        /// </summary>
        public string MauSoTBaoPhanHoiCuaCQT { get; set; }
        
        /// <summary>
        /// Số thông báo phản hồi của CQT 
        /// Kiểu dữ liệu: string
        /// </summary>
        public string SoTBaoPhanHoiCuaCQT { get; set; }

        /// <summary>
        /// Ngày thông báo phản hồi của CQT 
        /// Kiểu dữ liệu: Ngày tháng
        /// </summary>
        public DateTime? NgayTBaoPhanHoiCuaCQT { get; set; }

        /// <summary>
        /// Id thông điệp phản hồi từ CQT
        /// Kiểu dữ liệu: string
        /// </summary>
        public string IdTDiepTBaoPhanHoiCuaCQT { get; set; }

        public int? ThoiHan { get; set; }
        public List<BoKyHieuHoaDon> BoKyHieuHoaDons { get; set; }
    }
}
