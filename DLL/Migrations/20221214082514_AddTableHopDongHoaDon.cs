using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DLL.Migrations
{
    public partial class AddTableHopDongHoaDon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HopDongHoaDons",
                columns: table => new
                {
                    HopDongHoaDonId = table.Column<string>(nullable: false,maxLength:255),
                    Status = table.Column<bool>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true, maxLength: 255),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true, maxLength: 255),
                    STT = table.Column<int>(nullable: true),
                    NgayLap = table.Column<DateTime>(nullable: false),
                    NgayDuyet = table.Column<DateTime>(nullable: false),
                    MauHopDongId = table.Column<string>(nullable: true, maxLength: 255),
                    KhachHangId = table.Column<string>(nullable: true, maxLength: 255),
                    SoHopDong = table.Column<string>(nullable: true, maxLength: 255),
                    TenKhachHang = table.Column<string>(nullable: true, maxLength: 255),
                    NguoiDaiDien = table.Column<string>(nullable: true, maxLength: 255),
                    ChucVu = table.Column<string>(nullable: true, maxLength: 255),
                    DiaChi = table.Column<string>(nullable: true, maxLength: 500),
                    MaSoThue = table.Column<string>(nullable: true, maxLength: 255),
                    SoDienThoai = table.Column<string>(nullable: true, maxLength: 255),
                    Fax = table.Column<string>(nullable: true, maxLength: 255),
                    Email = table.Column<string>(nullable: true, maxLength: 255),
                    SoTaiKhoan = table.Column<string>(nullable: true, maxLength: 255),
                    NganHangMo = table.Column<string>(nullable: true, maxLength: 255),
                    GoiBan = table.Column<string>(nullable: true, maxLength: 255),
                    SoLuongBan = table.Column<int>(nullable: false, defaultValue: 0),
                    DonGiaBan = table.Column<decimal>(nullable: false, defaultValue: 0m),
                    ThanhTienBan = table.Column<decimal>(nullable: false, defaultValue: 0m),
                    TongThanhTien = table.Column<decimal>(nullable: false, defaultValue: 0m),
                    TienThueGTGT = table.Column<decimal>(nullable: false, defaultValue: 0m),
                    TongThanhToan = table.Column<decimal>(nullable: false, defaultValue: 0m),
                    TongPhiBanQuyen = table.Column<decimal>(nullable: false, defaultValue: 0m),
                    TongPhiKeToan = table.Column<decimal>(nullable: false,defaultValue: 0m),
                    TienPhiKhoiTao = table.Column<decimal>(nullable: false,defaultValue: 0m),
                    TongPhiKhoiTao = table.Column<decimal>(nullable: false,defaultValue: 0m),
                    PhiBanQuyen = table.Column<decimal>(nullable: false,defaultValue: 0m),
                    PhanMemKeToan = table.Column<decimal>(nullable: false,defaultValue: 0m),
                    TongTien = table.Column<decimal>(nullable: false,defaultValue: 0m),
                    SoLuongTang = table.Column<int>(nullable: false, defaultValue: 0),
                    TongThanhToanBangChu = table.Column<string>(nullable: true, maxLength: 255),
                    SoftWareRef = table.Column<string>(nullable: true, maxLength: 255),
                    website = table.Column<string>(nullable: true, maxLength: 500),
                    KyHieu = table.Column<string>(nullable: true, maxLength: 255),
                    TuSo = table.Column<string>(nullable: true, maxLength: 255),
                    DenSo = table.Column<string>(nullable: true, maxLength: 255),
                    TongSoLuong = table.Column<int>(nullable: false),
                    GhiChuPhiKhoiTao = table.Column<string>(nullable: true, maxLength: 4000),
                    GhiChuSoLuongTang = table.Column<string>(nullable: true, maxLength: 4000),
                    pathFilePDF = table.Column<string>(nullable: true, maxLength: 4000),
                    NgayThiHanh = table.Column<DateTime>(nullable: false),
                    TenCucThue = table.Column<string>(nullable: true, maxLength: 255),
                    CongTyPhatTrienKeToan = table.Column<string>(nullable: true, maxLength: 255),
                    LoaiHoaDon = table.Column<string>(nullable: true, maxLength: 255),
                    NoiLap = table.Column<string>(nullable: true, maxLength: 255),
                    LoaiUSB = table.Column<string>(nullable: true, maxLength: 255),
                    SoSeriUSB = table.Column<string>(nullable: true, maxLength: 255),
                    TuNgayUSB = table.Column<DateTime>(nullable: false),
                    DenNgayUSB = table.Column<DateTime>(nullable: false),
                    ChungThuSo = table.Column<string>(nullable: true, maxLength: 4000),
                    GhiChuPhanMemKeToan = table.Column<string>(nullable: true, maxLength: 4000),
                    MauSoHoaDon = table.Column<string>(nullable: true, maxLength: 255),
                    NganhKinhDoanh = table.Column<string>(nullable: true, maxLength: 255),
                    Thoigiansudung = table.Column<int>(nullable: true),
                    GiaiDoan = table.Column<int>(nullable: true),
                    TrangThaiHopDong = table.Column<int>(nullable: true),
                    TinhTrangHopDong = table.Column<int>(nullable: true),
                    MaxSoHoaDon = table.Column<int>(nullable: true),
                    LoaiHopDong = table.Column<int>(nullable: true),//=0 Hợp đồng dùng thử ,=1 Hợp đồng hóa đơn bình thường, =2 hợp đồng hóa đơn VIP
                    LoaiCongTy = table.Column<int>(nullable: true),//LoaiCongTy= 0 : "Công ty Điện-Điện tử Bách Khoa" LoaiCongTy=  1, loai: "Công ty Phần mềm Bách Khoa"
                    MstGp = table.Column<string>(nullable: true, maxLength: 255),//Là mst của cty cung cấp GP dựa vào LoaiCongTy
                    IsGiaHan = table.Column<bool>(nullable: true),//=True Là HĐ gia hạn
                    TenNguoiDuyet = table.Column<string>(nullable: true, maxLength: 255),
                    TenNguoiTao = table.Column<string>(nullable: true, maxLength: 255),
                    KyHieuFromDatabase = table.Column<string>(nullable: true, maxLength: 255),
                    MauSo = table.Column<string>(nullable: true, maxLength: 255),
                    NguoiLienHe = table.Column<string>(nullable: true, maxLength: 255),
                    SoDienThoaiNguoiLienHe = table.Column<string>(nullable: true, maxLength: 255),
                    NgayHoaDonCuoiCung = table.Column<DateTime>(nullable: true),
                    GhiChu = table.Column<string>(nullable: true, maxLength: 4000),
                    ThanhTienTang = table.Column<decimal>(nullable: true),
                    TienChietKhau = table.Column<decimal>(nullable: true),
                    DonGiaTang = table.Column<decimal>(nullable: true),

                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HopDongHoaDons");
        }
    }
}
