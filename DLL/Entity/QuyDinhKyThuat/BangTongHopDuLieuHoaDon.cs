using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DLL.Entity.QuyDinhKyThuat
{
    public class BangTongHopDuLieuHoaDon
    {
        public string Id { get; set; }
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
        /// <para>Lần đầu</para>
        /// <para>Độ dài tối đa: 10</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự(Định dạng trường kỳ theo tháng, quý: N1N2/Y1Y2Y3Y4, Định dạng trường kỳ theo ngày: N1N2/N3N4/Y1Y2Y3Y4)(Chú thích: KDLieu.cs)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string KyDuLieu { get; set; }

        public int? NamDuLieu { get; set; }
        
        public int? ThangDuLieu { get; set; }

        public int? QuyDuLieu { get; set; }

        public DateTime? NgayDuLieu { get; set; }

        /// <summary>
        /// <para>Lần đầu</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public bool LanDau { get; set; }
        /// <summary>
        /// <para>Bổ sung lần thứ)</para>
        /// <para>Độ dài tối đa: 3</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc nếu LDau = 0</para>
        /// </summary>
        public int? BoSungLanThu { get; set; }

        public int? SuaDoiLanThu { get; set; }

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

        public DateTime ThoiGianGui { get; set; }

        public string NNT { get; set; }
    }
}
