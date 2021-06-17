using Services.Enums;
using Services.ViewModels.DanhMuc;
using System;
using System.Collections.Generic;

namespace ManagementServices.Helper
{
    public class PagingParams
    {
        public string Date { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; }
        public string Keyword { get; set; }
        public string SortValue { get; set; }
        public string SortKey { get; set; }
        public string KeywordCol { get; set; }
        public string ColName { get; set; }

        public string userId { get; set; }
        public string projectId { get; set; }
        public string roomId { get; set; }
        public string groupId { get; set; }

        public bool? fromMe { get; set; }
        public bool? toMe { get; set; }
        public bool? iSpy { get; set; }

        public string listCreateById { get; set; }
        public string listProcessId { get; set; }

        public string nhomKH_NCC_NV { get; set; }
        public List<string> nhomKH_NCC_NVs { get; set; }
        public string loaiKhachHang { get; set; }
        public string nhomVT_HH { get; set; }
        public bool? IsNhanVien { get; set; }
        public bool? IsKhachHang { get; set; }
        public bool? IsNhaCungCap { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string MaTinhChatVT_HH { get; set; }
        public bool? IsTienMat { get; set; }
        public int? LoaiPhieu { get; set; }
        public int? LoaiDoiTuong { get; set; } // 1 nhà cung cấp, 2 nhân viên
        public string DoiTuongId { get; set; }
        public string donViId { get; set; }
        public string khoId { get; set; }
        public int daGhiDoanhSo { get; set; }
        public string TaiKhoanNganHangId { get; set; }
        public string soTaiKhoan { get; set; }

        public int? LoaiChungTu { get; set; }
        public DateTime? KyKeToanNgayChungTu { get; set; }
        public string NgayBuTru { get; set; }

        public List<string> DoiTuongIds { get; set; }
        public List<string> TaiKhoans { get; set; }
        //public List<TaiKhoanKeToanViewModel> TaiKhoanKeToans { get; set; }
        public bool? IsMuaHangHoa { get; set; }
        public bool? HasLoaiTien { get; set; }
        public string NhaCungCapId { get; set; }
        public string LoaiTienId { get; set; }
        public string MaChucNang { get; set; }
        //public string LoaiTienId { get; set; }
        public string MucThuChiId { get; set; }
        public int? Loai { get; set; }
        public string PhieuId { get; set; }
        public string TypePhieu { get; set; }
        public bool? IsViewDetail { get; set; }

        public int? PTTheoTuoiNo { get; set; }
        public int? TinhTrang { get; set; }

        //public LoaiThamChieu LoaiThamChieu { get; set; }
        //public string GiaTriThamChieu { get; set; }

        public int? TrangThai { get; set; }
        public int? TrangThaiHoaDon { get; set; }
        public int? LapHoaDon { get; set; }
        public int? TrangThaiGhiNhan { get; set; }
        public string chungTuId { get; set; }
        public List<string> SoTaiKhoanTiens { get; set; }
        public List<string> SoTaiKhoanDoiTuongs { get; set; }
        public int? LoaiHoaDon { get; set; }
        public int? TrangThaiHoaDonDienTu { get; set; }
        public int? TrangThaiPhatHanh { get; set; }
        public int? TrangThaiGuiHoaDon { get; set; }
        public bool? TrangThaiChuyenDoi { get; set; }
    }
}
