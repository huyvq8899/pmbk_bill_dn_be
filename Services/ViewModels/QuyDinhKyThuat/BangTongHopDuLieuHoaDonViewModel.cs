using DLL.Enums;
using Services.Helper.LogHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.QuyDinhKyThuat
{
    public class BangTongHopDuLieuHoaDonViewModel : ThongTinChungViewModel
    {
        public string Id { get; set; }

        /// <summary>
        /// <para>Phiên bản XML (Trong Quy định này có giá trị là 2.0.1)</para>
        /// <para>Độ dài tối đa: 6</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(6)]
        public string PhienBan { get; set; }

        /// <summary>
        /// <para>Mẫu số (mẫu số bảng tổng hợp)</para>
        /// <para>Độ dài tối đa: 15</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(15)]
        public string MauSo { get; set; }

        /// <summary>
        /// <para>Tên (tên bảng tổng hợp)</para>
        /// <para>Độ dài tối đa: 100</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Ten { get; set; }

        /// <summary>
        /// <para>Số bảng tổng hợp dữ liệu (Số thứ tự bảng tổng hợp dữ liệu)</para>
        /// <para>Độ dài tối đa: 5</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public int SoBTHDLieu { get; set; }

        /// <summary>
        /// <para>Loại kỳ dữ liệu</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự (T: tháng, Q: quý, N: năm)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(1)]
        public string LoaiKyDuLieu { get; set; }

        /// <summary>
        /// <para>Kỳ dữ liệu</para>
        /// <para>Độ dài tối đa: 10</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự(Định dạng trường kỳ theo tháng, quý: N1N2/Y1Y2Y3Y4, Định dạng trường kỳ theo ngày: N1N2/N3N4/Y1Y2Y3Y4)(Chú thích: KDLieu.cs)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string KyDuLieu { get; set; }

        /// <summary>
        /// Năm dữ liệu
        /// Kiểu dữ liệu: Số
        /// </summary>
        public int? NamDuLieu { get; set; }

        /// <summary>
        /// Tháng dữ liệu (đối với trường hợp kỳ kê khai là tháng)
        /// Kiểu dữ liệu: Số
        /// </summary>
        public int? ThangDuLieu { get; set; }

        /// <summary>
        /// Quý dữ liệu (đối với trường hợp kỳ kê khai là quý)
        /// Kiểu dữ liệu: Số
        /// </summary>
        public int? QuyDuLieu { get; set; }

        /// <summary>
        /// Ngày dữ liệu (đối với trường hợp gửi dữ liệu trong ngày)
        /// </summary>
        public DateTime? NgayDuLieu { get; set; }

        /// <summary>
        /// <para>Lần đầu</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: boolean</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public bool LanDau { get; set; }

        /// <summary>
        /// <para>Bổ sung lần thứ)</para>
        /// <para>Độ dài tối đa: 3</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc nếu LanDau = false</para>
        /// </summary>
        public int? BoSungLanThu { get; set; }

        /// <summary>
        /// <para>Ngày lập</para>
        /// <para>Kiểu dữ liệu: Ngày</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public DateTime NgayLap { get; set; }

        /// <summary>
        /// <para>Tên NNT</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(400)]
        public string TenNNT { get; set; }

        /// <summary>
        /// <para>Mã số thuế NNT</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(14)]
        public string MaSoThue { get; set; }

        /// <summary>
        /// <para>Hóa đơn đặt in</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public bool HDDIn { get; set; }

        /// <summary>
        /// <para>Loại hàng hóa (Loại hàng hóa, dịch vụ kinh doanh)</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public int LHHoa { get; set; }

        [IgnoreLogging]
        public string TenLoaiHH { get; set; }

        /// <summary>
        /// Thời hạn gửi bảng tổng hợp lên CQT
        /// Kiểu dữ liệu: ngày tháng
        /// </summary>
        public DateTime ThoiHanGui { get; set; }

        [IgnoreLogging]
        public bool IsQuaHan { get; set; }

        /// <summary>
        /// Tên người nộp thuế
        /// Kiểu dữ liệu: string
        /// </summary>
        public string NNT { get; set; }

        public List<BangTongHopDuLieuHoaDonChiTietViewModel> ChiTiets { get; set; }

        public UserViewModel ActionUser { get; set; }
        public UserViewModel NguoiTao { get; set; }
        public UserViewModel NguoiCapNhat { get; set; }

        //Thông điệp tương ứng
        public string ThongDiepChungId { get; set; }

        public DateTime? ThoiGianGui { get; set; }

        public int? MaLoaiThongDiep { get; set; }
        public string MaThongDiep { get; set; }

        public TrangThaiGuiThongDiep TrangThaiGui { get; set; }

        public TrangThaiQuyTrinh_BangTongHop TrangThaiQuyTrinh { get; set; }

        public string TenTrangThaiQuyTrinh { get; set; }
        public string TenTrangThaiGui { get; set; }
        public string DVTTe { get; set; }
    }
}
