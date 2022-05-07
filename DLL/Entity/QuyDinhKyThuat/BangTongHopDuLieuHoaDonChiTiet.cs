using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DLL.Entity.QuyDinhKyThuat
{
    public class BangTongHopDuLieuHoaDonChiTiet
    {
        public string Id { get; set; }

        public string BangTongHopDuLieuHoaDonId { get; set; }

        public int? TrangThai { get; set; }
        public int? BackupTrangThai { get; set; }

        /// <summary>
        /// Mẫu số hóa đơn
        /// Kiểu dữ liệu: string
        /// </summary>
        [Required]
        [MaxLength(1)]
        public string MauSo { get; set; }

        /// <summary>
        /// Ký hiệu hóa đơn
        /// Kiểu dữ liệu: string
        /// </summary>
        [Required]
        [MaxLength(6)]
        public string KyHieu { get; set; }
        
        /// <summary>
        /// Số hóa đơn
        /// Kiểu dữ liệu: số
        /// </summary>
        public long SoHoaDon { get; set; }

        /// <summary>
        /// Ngày hóa đơn
        /// Kiểu dữ liệu: Ngày tháng
        /// </summary>
        [Required]
        public DateTime? NgayHoaDon { get; set; }

        /// <summary>
        /// Mã số thuế người mua
        /// Kiểu dữ liệu: string
        /// </summary>
        [MaxLength(14)]
        public string MaSoThue { get; set; }

        /// <summary>
        /// Mã khách hàng
        /// Kiểu dữ liệu: string
        /// </summary>
        [MaxLength(50)]
        public string MaKhachHang { get; set; }

        /// <summary>
        /// Tên khách hàng
        /// Kiểu dữ liệu: string
        /// </summary>
        [MaxLength(400)]
        public string TenKhachHang { get; set; }

        /// <summary>
        /// Địa chỉ
        /// Kiểu dữ liệu: string
        /// </summary>
        public string DiaChi { get; set; }

        /// <summary>
        /// Họ tên người mua hàng
        /// Kiểu dữ liệu: string
        /// </summary>
        [MaxLength(400)]
        public string HoTenNguoiMuaHang { get; set; }

        /// <summary>
        /// <para>Mã hàng hóa, dịch vụ</para>
        /// <para>Độ dài tối đa: 50</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Đối với Loại hàng hóa, dịch vụ kinh doanh là 1- Xăng dầu)</para>
        /// </summary>
        [MaxLength(50)]
        public string MaHang { get; set; }

        /// <summary>
        /// <para>Tên hàng hóa, dịch vụ (Mặt hàng)</para>
        /// <para>Độ dài tối đa: 500</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Đối với Loại hàng hóa, dịch vụ kinh doanh là 1- Xăng dầu)</para>
        /// </summary>
        [MaxLength(500)]
        public string TenHang { get; set; }

        /// <summary>
        /// <para>Số lượng hàng hóa</para>
        /// <para>Độ dài tối đa: 19,4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc (Đối với Loại hàng hóa, dịch vụ kinh doanh là 1- Xăng dầu (trừ trường hợp Trạng thái là Điều chỉnh, Giải trình, Sai sót do tổng hợp))</para>
        /// </summary>
        [Column(TypeName = "decimal(19, 4)")]
        public decimal? SoLuong { get; set; }

        /// <summary>
        /// <para>Đơn vị tính</para>
        /// <para>Độ dài tối đa: 50</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Đối với Loại hàng hóa, dịch vụ kinh doanh là 1- Xăng dầu (trừ trường hợp Trạng thái là Điều chỉnh, Giải trình, Sai sót do tổng hợp))</para>
        /// </summary>
        [MaxLength(50)]
        public string DonViTinh { get; set; }

        /// <summary>
        /// <para>Tổng giá trị hàng hóa, dịch vụ bán ra chưa có thuế GTGT</para>
        /// <para>Độ dài tối đa: 19,4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại Điều 10, Điều 22 Nghị định 123/2020/NĐ-CP)</para>
        /// </summary>
        [Column(TypeName = "decimal(19, 4)")]
        public decimal? ThanhTien { get; set; }

        /// <summary>
        /// <para>Thuế suất (Thuế suất thuế GTGT)</para>
        /// <para>Độ dài tối đa: 5</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự (chi tiết tại Phụ lục V kèm theo Quy định này)(Chú thích: TSuat.cs)</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại Điều 10, Điều 22 Nghị định 123/2020/NĐ-CP)</para>
        /// </summary>
        [MaxLength(5)]
        public string ThueGTGT { get; set; }

        /// <summary>
        /// <para>Tổng tiền thuế (Tổng tiền thuế GTGT)</para>
        /// <para>Độ dài tối đa: 19,4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại Điều 10, Điều 22 Nghị định 123/2020/NĐ-CP)</para>
        /// </summary>
        [Column(TypeName = "decimal(19, 4)")]
        public decimal? TienThueGTGT { get; set; }

        /// <summary>
        /// <para>Tổng tiền thanh toán</para>
        /// <para>Độ dài tối đa: 19,4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại Điều 10, Điều 22 Nghị định 123/2020/NĐ-CP)</para>
        /// </summary>
        [Column(TypeName = "decimal(19, 4)")]
        public decimal? TongTienThanhToan { get; set; }

        /// <summary>
        /// <para>Trạng thái</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public int? TrangThaiHoaDon { get; set; }

        /// <summary>
        /// <para>Loại hóa đơn có liên quan (Loại áp dụng hóa đơn của HĐ có liên quan)</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc (Đối với trường hợp điều chỉnh, thay thế cho hóa đơn có Ký hiệu mẫu số hóa đơn, Ký hiệu hóa đơn, Số hóa đơn) </para>
        /// </summary>
        public int? LoaiHoaDonLienQuan { get; set; }


        /// <summary>
        /// <para>Ký hiệu mẫu số hóa đơn có liên quan (Ký hiệu mẫu số hóa đơn bị thay thế/điều chỉnh)</para>
        /// <para>Độ dài tối đa: 11</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự (Chi tiết tại Phụ lục II kèm theo Quy định này)(Chú thích: KHMSHDon.cs)</para>
        /// <para>Bắt buộc (Đối với trường hợp điều chỉnh, thay thế cho hóa đơn có Ký hiệu mẫu số hóa đơn, Ký hiệu hóa đơn, Số hóa đơn)</para>
        /// </summary>
        [MaxLength(11)]
        public string MauSoHoaDonLienQuan { get; set; }

        /// <summary>
        /// <para>Ký hiệu hóa đơn có liên quan (Ký hiệu hóa đơn bị thay thế/điều chỉnh)</para>
        /// <para>Độ dài tối đa: 8</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Đối với trường hợp điều chỉnh, thay thế cho hóa đơn có Ký hiệu mẫu số hóa đơn, Ký hiệu hóa đơn, Số hóa đơn)</para>
        /// </summary>
        [MaxLength(8)]
        public string KyHieuHoaDonLienQuan { get; set; }

        /// <summary>
        /// <para>Số hóa đơn có liên quan (Số hóa đơn bị thay thế/điều chỉnh)</para>
        /// <para>Độ dài tối đa: 8</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Đối với trường hợp điều chỉnh, thay thế cho hóa đơn có Ký hiệu mẫu số hóa đơn, Ký hiệu hóa đơn, Số hóa đơn) </para>
        /// </summary>
        [MaxLength(8)]
        public string SoHoaDonLienQuan { get; set; }

        /// <summary>
        /// <para>Ngày hóa đơn liên quan</para>
        /// <para>Kiểu dữ liệu: Ngày tháng</para>
        /// </summary>
        public DateTime? NgayHoaDonLienQuan { get; set; }

        /// <summary>
        /// <para>Loại kỳ dữ liệu điều chỉnh</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Đối với trường hợp điều chỉnh cho hóa đơn không có Ký hiệu mẫu số hóa đơn, Ký hiệu hóa đơn, Số hóa đơn hoặc Loại hàng hóa, dịch vụ kinh doanh là 1 - Xăng dầu) </para>
        /// </summary>
        [MaxLength(1)]
        public string LKDLDChinh { get; set; }

        /// <summary>
        /// <para>Kỳ dữ liệu điều chỉnh</para>
        /// <para>Độ dài tối đa: 10</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Đối với trường hợp điều chỉnh cho hóa đơn không có Ký hiệu mẫu số hóa đơn, Ký hiệu hóa đơn, Số hóa đơn hoặc Loại hàng hóa, dịch vụ kinh doanh là 1 - Xăng dầu) </para>
        /// </summary>
        [MaxLength(10)]
        public string KDLDChinh { get; set; }

        /// <summary>
        /// <para>Số thông báo (Số thông báo của CQT về hóa đơn điện tử cần rà soát)</para>
        /// <para>Độ dài tối đa: 30</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (đối với trường hợp giải trình theo thông báo của CQT)</para>
        /// </summary>
        public string STBao { get; set; }

        /// <summary>
        /// <para>Ngày thông báo (Ngày thông báo của CQT về hóa đơn điện tử cần rà soát)</para>
        /// <para>Kiểu dữ liệu: Ngày</para>
        /// <para>Bắt buộc (đối với trường hợp giải trình theo thông báo của CQT)</para>
        /// </summary>
        public DateTime? NTBao { get; set; }

        /// <summary>
        /// <para>Ghi chú</para>
        /// <para>Độ dài tối đa: 255</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(255)]
        public string GhiChu { get; set; }

        public string RefHoaDonDienTuId { get; set; }

        public BangTongHopDuLieuHoaDon BangTongHopDuLieuHoaDon;
    }
}
