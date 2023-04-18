using DLL.Enums;
using System.ComponentModel.DataAnnotations;

namespace DLL.Entity.DanhMuc
{
    public class HoSoHDDT : ThongTinChung
    {
        public string HoSoHDDTId { get; set; }
        public string MaSoThue { get; set; }
        public string TenDonVi { get; set; }
        public string DiaChi { get; set; }
        public string NganhNghe { get; set; }
        public string NganhNgheKinhDoanhChinh { get; set; }
        public string HoTenNguoiDaiDienPhapLuat { get; set; }
        public string EmailNguoiDaiDienPhapLuat { get; set; }
        public string SoDienThoaiNguoiDaiDienPhapLuat { get; set; }
        public string SoTaiKhoanNganHang { get; set; }
        public string TenNganHang { get; set; }
        public string ChiNhanh { get; set; }
        public string EmailLienHe { get; set; }
        public string SoDienThoaiLienHe { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        public string CoQuanThueCapCuc { get; set; }
        public string TenCoQuanThueCapCuc { get; set; }
        public string CoQuanThueQuanLy { get; set; }
        public string TenCoQuanThueQuanLy { get; set; }
        public string PhuongPhapTinhThueGTGT { get; set; }
        public KyKeKhaiThue KyTinhThue { get; set; }
        /// <summary>
        /// Mã 5 ký tự của cơ quan thuế khi tờ khai lần đầu có tích chọn hình thức hóa đơn có mã từ cơ quan thuế
        /// </summary>
        public string MaCuaCQTToKhaiChapNhan { get; set; }
        /// <summary>
        /// Lưu string chi tiết thông báo trả về của cqt để thể hiện trên tab thông tin hóa đơn
        /// </summary>
        public string ThongBaoChiTietMaCuaCQT { get; set; }
        /// <summary>
        /// ThongDiepId trong bảng thông điệp chung tờ khai 01 có tích hình thức hóa đơn có mã từ máy tính tiền
        /// </summary>
        public string MaThongDiepChuaMCQT { get; set; }
        [MaxLength(36)]
        public string FileId103Import { get; set; }

        public bool IsNhapKhauMaCQT { get; set; }
    }
}
