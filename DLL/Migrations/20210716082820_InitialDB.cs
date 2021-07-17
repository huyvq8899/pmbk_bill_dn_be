using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class InitialDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfigNoiDungEmails",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    LoaiEmail = table.Column<int>(nullable: false),
                    TieuDeEmail = table.Column<string>(nullable: true),
                    NoiDungEmail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigNoiDungEmails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DoiTuongs",
                columns: table => new
                {
                    DoiTuongId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    LoaiKhachHang = table.Column<int>(nullable: true),
                    MaSoThue = table.Column<string>(nullable: true),
                    Ma = table.Column<string>(nullable: true),
                    Ten = table.Column<string>(nullable: true),
                    DiaChi = table.Column<string>(nullable: true),
                    SoTaiKhoanNganHang = table.Column<string>(nullable: true),
                    TenNganHang = table.Column<string>(nullable: true),
                    ChiNhanh = table.Column<string>(nullable: true),
                    HoTenNguoiMuaHang = table.Column<string>(nullable: true),
                    EmailNguoiMuaHang = table.Column<string>(nullable: true),
                    SoDienThoaiNguoiMuaHang = table.Column<string>(nullable: true),
                    HoTenNguoiNhanHD = table.Column<string>(nullable: true),
                    EmailNguoiNhanHD = table.Column<string>(nullable: true),
                    SoDienThoaiNguoiNhanHD = table.Column<string>(nullable: true),
                    ChucDanh = table.Column<string>(nullable: true),
                    TenDonVi = table.Column<string>(nullable: true),
                    IsKhachHang = table.Column<bool>(nullable: true),
                    IsNhanVien = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoiTuongs", x => x.DoiTuongId);
                });

            migrationBuilder.CreateTable(
                name: "DonViTinhs",
                columns: table => new
                {
                    DonViTinhId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    Ten = table.Column<string>(nullable: true),
                    MoTa = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonViTinhs", x => x.DonViTinhId);
                });

            migrationBuilder.CreateTable(
                name: "Functions",
                columns: table => new
                {
                    FunctionId = table.Column<string>(maxLength: 36, nullable: false),
                    FunctionName = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    SubTitle = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Functions", x => x.FunctionId);
                });

            migrationBuilder.CreateTable(
                name: "HinhThucThanhToans",
                columns: table => new
                {
                    HinhThucThanhToanId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    Ten = table.Column<string>(nullable: true),
                    MoTa = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HinhThucThanhToans", x => x.HinhThucThanhToanId);
                });

            migrationBuilder.CreateTable(
                name: "HoSoHDDTs",
                columns: table => new
                {
                    HoSoHDDTId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    MaSoThue = table.Column<string>(nullable: true),
                    TenDonVi = table.Column<string>(nullable: true),
                    DiaChi = table.Column<string>(nullable: true),
                    NganhNgheKinhDoanhChinh = table.Column<string>(nullable: true),
                    HoTenNguoiDaiDienPhapLuat = table.Column<string>(nullable: true),
                    EmailNguoiDaiDienPhapLuat = table.Column<string>(nullable: true),
                    SoDienThoaiNguoiDaiDienPhapLuat = table.Column<string>(nullable: true),
                    SoTaiKhoanNganHang = table.Column<string>(nullable: true),
                    TenNganHang = table.Column<string>(nullable: true),
                    ChiNhanh = table.Column<string>(nullable: true),
                    EmailLienHe = table.Column<string>(nullable: true),
                    SoDienThoaiLienHe = table.Column<string>(nullable: true),
                    Fax = table.Column<string>(nullable: true),
                    Website = table.Column<string>(nullable: true),
                    CoQuanThueCapCuc = table.Column<string>(nullable: true),
                    CoQuanThueQuanLy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoSoHDDTs", x => x.HoSoHDDTId);
                });

            migrationBuilder.CreateTable(
                name: "KyKeToans",
                columns: table => new
                {
                    KyKeToanId = table.Column<string>(maxLength: 36, nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    NgayChungTu = table.Column<DateTime>(nullable: true),
                    DenNgay = table.Column<DateTime>(nullable: true),
                    GhiChu = table.Column<string>(nullable: true),
                    DaKhoaSo = table.Column<bool>(nullable: true),
                    DaKhoaNhapSoDuBanDau = table.Column<bool>(nullable: true),
                    LoaiThongTu = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KyKeToans", x => x.KyKeToanId);
                });

            migrationBuilder.CreateTable(
                name: "LoaiTiens",
                columns: table => new
                {
                    LoaiTienId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    Ma = table.Column<string>(nullable: true),
                    Ten = table.Column<string>(nullable: true),
                    TyGiaQuyDoi = table.Column<decimal>(nullable: true),
                    SapXep = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiTiens", x => x.LoaiTienId);
                });

            migrationBuilder.CreateTable(
                name: "MauHoaDons",
                columns: table => new
                {
                    MauHoaDonId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    Ten = table.Column<string>(nullable: true),
                    SoThuTu = table.Column<int>(nullable: true),
                    MauSo = table.Column<string>(nullable: true),
                    KyHieu = table.Column<string>(nullable: true),
                    TenBoMau = table.Column<string>(nullable: true),
                    DocHoaDonMauCoBan = table.Column<string>(nullable: true),
                    DocHoaDonMauDangChuyenDoi = table.Column<string>(nullable: true),
                    DocHoaDonMauCoChietKhau = table.Column<string>(nullable: true),
                    DocHoaDonMauNgoaiTe = table.Column<string>(nullable: true),
                    IsDaKy = table.Column<bool>(nullable: true),
                    QuyDinhApDung = table.Column<int>(nullable: false),
                    LoaiHoaDon = table.Column<int>(nullable: false),
                    LoaiMauHoaDon = table.Column<int>(nullable: false),
                    LoaiThueGTGT = table.Column<int>(nullable: false),
                    LoaiNgonNgu = table.Column<int>(nullable: false),
                    LoaiKhoGiay = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MauHoaDons", x => x.MauHoaDonId);
                });

            migrationBuilder.CreateTable(
                name: "NhatKyGuiEmails",
                columns: table => new
                {
                    NhatKyGuiEmailId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    MauSo = table.Column<string>(nullable: true),
                    KyHieu = table.Column<string>(nullable: true),
                    So = table.Column<string>(nullable: true),
                    Ngay = table.Column<string>(nullable: true),
                    TrangThaiGuiEmail = table.Column<int>(nullable: false),
                    TenNguoiNhan = table.Column<string>(nullable: true),
                    EmailNguoiNhan = table.Column<string>(nullable: true),
                    RefId = table.Column<string>(nullable: true),
                    RefType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhatKyGuiEmails", x => x.NhatKyGuiEmailId);
                });

            migrationBuilder.CreateTable(
                name: "NhatKyTruyCaps",
                columns: table => new
                {
                    NhatKyTruyCapId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    DoiTuongThaoTac = table.Column<string>(nullable: true),
                    HanhDong = table.Column<string>(nullable: true),
                    ThamChieu = table.Column<string>(nullable: true),
                    MoTaChiTiet = table.Column<string>(nullable: true),
                    DiaChiIP = table.Column<string>(nullable: true),
                    TenMayTinh = table.Column<string>(nullable: true),
                    RefFile = table.Column<string>(nullable: true),
                    RefId = table.Column<string>(nullable: true),
                    RefType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhatKyTruyCaps", x => x.NhatKyTruyCapId);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    PermissionId = table.Column<string>(maxLength: 36, nullable: false),
                    PermissionName = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.PermissionId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<string>(maxLength: 36, nullable: false),
                    RoleName = table.Column<string>(maxLength: 200, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    Status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "TaiLieuDinhKems",
                columns: table => new
                {
                    TaiLieuDinhKemId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    NghiepVuId = table.Column<string>(nullable: true),
                    LoaiNghiepVu = table.Column<int>(nullable: false),
                    TenGoc = table.Column<string>(nullable: true),
                    TenGuid = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiLieuDinhKems", x => x.TaiLieuDinhKemId);
                });

            migrationBuilder.CreateTable(
                name: "ThaoTacs",
                columns: table => new
                {
                    ThaoTacId = table.Column<string>(nullable: false),
                    Ma = table.Column<string>(nullable: true),
                    Ten = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThaoTacs", x => x.ThaoTacId);
                });

            migrationBuilder.CreateTable(
                name: "ThongBaoDieuChinhThongTinHoaDons",
                columns: table => new
                {
                    ThongBaoDieuChinhThongTinHoaDonId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    NgayThongBaoDieuChinh = table.Column<DateTime>(nullable: true),
                    NgayThongBaoPhatHanh = table.Column<DateTime>(nullable: true),
                    CoQuanThue = table.Column<string>(nullable: true),
                    So = table.Column<string>(nullable: true),
                    TrangThaiHieuLuc = table.Column<int>(nullable: false),
                    TenDonViCu = table.Column<string>(nullable: true),
                    TenDonViMoi = table.Column<string>(nullable: true),
                    DiaChiCu = table.Column<string>(nullable: true),
                    DiaChiMoi = table.Column<string>(nullable: true),
                    DienThoaiCu = table.Column<string>(nullable: true),
                    DienThoaiMoi = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongBaoDieuChinhThongTinHoaDons", x => x.ThongBaoDieuChinhThongTinHoaDonId);
                });

            migrationBuilder.CreateTable(
                name: "ThongBaoKetQuaHuyHoaDons",
                columns: table => new
                {
                    ThongBaoKetQuaHuyHoaDonId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    CoQuanThue = table.Column<string>(nullable: true),
                    NgayGioHuy = table.Column<DateTime>(nullable: true),
                    PhuongPhapHuy = table.Column<string>(nullable: true),
                    So = table.Column<string>(nullable: true),
                    NgayThongBao = table.Column<DateTime>(nullable: true),
                    TrangThaiNop = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongBaoKetQuaHuyHoaDons", x => x.ThongBaoKetQuaHuyHoaDonId);
                });

            migrationBuilder.CreateTable(
                name: "ThongBaoPhatHanhs",
                columns: table => new
                {
                    ThongBaoPhatHanhId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    DienThoai = table.Column<string>(nullable: true),
                    CoQuanThue = table.Column<string>(nullable: true),
                    NguoiDaiDienPhapLuat = table.Column<string>(nullable: true),
                    Ngay = table.Column<DateTime>(nullable: false),
                    So = table.Column<string>(nullable: true),
                    TrangThaiNop = table.Column<int>(nullable: false),
                    IsXacNhan = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongBaoPhatHanhs", x => x.ThongBaoPhatHanhId);
                });

            migrationBuilder.CreateTable(
                name: "TuyChons",
                columns: table => new
                {
                    Ma = table.Column<string>(nullable: false),
                    Ten = table.Column<string>(nullable: true),
                    GiaTri = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TuyChons", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "ViewThaoTacs",
                columns: table => new
                {
                    ThaoTacId = table.Column<string>(nullable: false),
                    PemissionId = table.Column<string>(nullable: true),
                    FunctionId = table.Column<string>(nullable: true),
                    RoleId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    FTID = table.Column<string>(nullable: true),
                    UTID = table.Column<string>(nullable: true),
                    Ma = table.Column<string>(nullable: true),
                    Ten = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViewThaoTacs", x => x.ThaoTacId);
                });

            migrationBuilder.CreateTable(
                name: "HangHoaDichVus",
                columns: table => new
                {
                    HangHoaDichVuId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    Ma = table.Column<string>(nullable: true),
                    Ten = table.Column<string>(nullable: true),
                    DonGiaBan = table.Column<decimal>(nullable: true),
                    IsGiaBanLaDonGiaSauThue = table.Column<bool>(nullable: true),
                    ThueGTGT = table.Column<int>(nullable: false),
                    TyLeChietKhau = table.Column<decimal>(nullable: true),
                    MoTa = table.Column<string>(nullable: true),
                    DonViTinhId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangHoaDichVus", x => x.HangHoaDichVuId);
                    table.ForeignKey(
                        name: "FK_HangHoaDichVus_DonViTinhs_DonViTinhId",
                        column: x => x.DonViTinhId,
                        principalTable: "DonViTinhs",
                        principalColumn: "DonViTinhId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HoaDonDienTus",
                columns: table => new
                {
                    HoaDonDienTuId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    NgayHoaDon = table.Column<DateTime>(nullable: true),
                    SoHoaDon = table.Column<string>(nullable: true),
                    MauHoaDonId = table.Column<string>(nullable: true),
                    MauSo = table.Column<string>(nullable: true),
                    KyHieu = table.Column<string>(nullable: true),
                    KhachHangId = table.Column<string>(nullable: true),
                    MaKhachHang = table.Column<string>(nullable: true),
                    TenKhachHang = table.Column<string>(nullable: true),
                    DiaChi = table.Column<string>(nullable: true),
                    MaSoThue = table.Column<string>(nullable: true),
                    HoTenNguoiMuaHang = table.Column<string>(nullable: true),
                    SoDienThoaiNguoiMuaHang = table.Column<string>(nullable: true),
                    EmailNguoiMuaHang = table.Column<string>(nullable: true),
                    TenNganHang = table.Column<string>(nullable: true),
                    SoTaiKhoanNganHang = table.Column<string>(nullable: true),
                    HoTenNguoiNhanHD = table.Column<string>(nullable: true),
                    EmailNguoiNhanHD = table.Column<string>(nullable: true),
                    SoDienThoaiNguoiNhanHD = table.Column<string>(nullable: true),
                    HinhThucThanhToanId = table.Column<string>(nullable: true),
                    NhanVienBanHangId = table.Column<string>(nullable: true),
                    MaNhanVienBanHang = table.Column<string>(nullable: true),
                    TenNhanVienBanHang = table.Column<string>(nullable: true),
                    LoaiTienId = table.Column<string>(nullable: true),
                    TyGia = table.Column<decimal>(nullable: true),
                    TrangThai = table.Column<int>(nullable: true),
                    TrangThaiPhatHanh = table.Column<int>(nullable: true),
                    MaTraCuu = table.Column<string>(nullable: true),
                    TrangThaiGuiHoaDon = table.Column<int>(nullable: true),
                    KhachHangDaNhan = table.Column<bool>(nullable: true),
                    SoLanChuyenDoi = table.Column<int>(nullable: true),
                    NgayXoaBo = table.Column<DateTime>(nullable: true),
                    SoCTXoaBo = table.Column<string>(nullable: true),
                    TrangThaiBienBanXoaBo = table.Column<int>(nullable: false),
                    LyDoXoaBo = table.Column<string>(nullable: true),
                    LoaiHoaDon = table.Column<int>(nullable: false),
                    NgayLap = table.Column<DateTime>(nullable: true),
                    NguoiLapId = table.Column<string>(nullable: true),
                    LoaiChungTu = table.Column<int>(nullable: false),
                    ThamChieu = table.Column<string>(nullable: true),
                    TaiLieuDinhKem = table.Column<string>(nullable: true),
                    FileChuaKy = table.Column<string>(nullable: true),
                    FileDaKy = table.Column<string>(nullable: true),
                    TongTienHang = table.Column<decimal>(nullable: true),
                    TongTienChietKhau = table.Column<decimal>(nullable: true),
                    TongTienThueGTGT = table.Column<decimal>(nullable: true),
                    TongTienThanhToan = table.Column<decimal>(nullable: true),
                    TongTienHangQuyDoi = table.Column<decimal>(nullable: true),
                    TongTienChietKhauQuyDoi = table.Column<decimal>(nullable: true),
                    TongTienThueGTGTQuyDoi = table.Column<decimal>(nullable: true),
                    TongTienThanhToanQuyDoi = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDonDienTus", x => x.HoaDonDienTuId);
                    table.ForeignKey(
                        name: "FK_HoaDonDienTus_HinhThucThanhToans_HinhThucThanhToanId",
                        column: x => x.HinhThucThanhToanId,
                        principalTable: "HinhThucThanhToans",
                        principalColumn: "HinhThucThanhToanId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HoaDonDienTus_DoiTuongs_KhachHangId",
                        column: x => x.KhachHangId,
                        principalTable: "DoiTuongs",
                        principalColumn: "DoiTuongId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HoaDonDienTus_LoaiTiens_LoaiTienId",
                        column: x => x.LoaiTienId,
                        principalTable: "LoaiTiens",
                        principalColumn: "LoaiTienId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HoaDonDienTus_MauHoaDons_MauHoaDonId",
                        column: x => x.MauHoaDonId,
                        principalTable: "MauHoaDons",
                        principalColumn: "MauHoaDonId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HoaDonDienTus_DoiTuongs_NguoiLapId",
                        column: x => x.NguoiLapId,
                        principalTable: "DoiTuongs",
                        principalColumn: "DoiTuongId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HoaDonDienTus_DoiTuongs_NhanVienBanHangId",
                        column: x => x.NhanVienBanHangId,
                        principalTable: "DoiTuongs",
                        principalColumn: "DoiTuongId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MauHoaDonThietLapMacDinhs",
                columns: table => new
                {
                    MauHoaDonThietLapMacDinhId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    MauHoaDonId = table.Column<string>(nullable: true),
                    Loai = table.Column<int>(nullable: false),
                    GiaTri = table.Column<string>(nullable: true),
                    GiaTriBoSung = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MauHoaDonThietLapMacDinhs", x => x.MauHoaDonThietLapMacDinhId);
                    table.ForeignKey(
                        name: "FK_MauHoaDonThietLapMacDinhs_MauHoaDons_MauHoaDonId",
                        column: x => x.MauHoaDonId,
                        principalTable: "MauHoaDons",
                        principalColumn: "MauHoaDonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Function_Roles",
                columns: table => new
                {
                    FRID = table.Column<string>(maxLength: 36, nullable: false),
                    FunctionId = table.Column<string>(nullable: true),
                    RoleId = table.Column<string>(nullable: true),
                    PermissionId = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Function_Roles", x => x.FRID);
                    table.ForeignKey(
                        name: "FK_Function_Roles_Functions_FunctionId",
                        column: x => x.FunctionId,
                        principalTable: "Functions",
                        principalColumn: "FunctionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Function_Roles_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Function_Roles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(maxLength: 36, nullable: false),
                    Password = table.Column<string>(maxLength: 200, nullable: true),
                    ConfirmPassword = table.Column<string>(maxLength: 200, nullable: true),
                    UserName = table.Column<string>(maxLength: 200, nullable: true),
                    Email = table.Column<string>(maxLength: 200, nullable: true),
                    FullName = table.Column<string>(maxLength: 200, nullable: true),
                    Gender = table.Column<int>(nullable: true),
                    Avatar = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(maxLength: 200, nullable: true),
                    Title = table.Column<string>(maxLength: 200, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    RoleId = table.Column<string>(nullable: true),
                    IsAdmin = table.Column<bool>(nullable: true),
                    IsNodeAdmin = table.Column<bool>(nullable: true),
                    IsOnline = table.Column<bool>(nullable: true),
                    LoginCount = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Function_ThaoTacs",
                columns: table => new
                {
                    FTID = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: true),
                    FunctionId = table.Column<string>(nullable: true),
                    ThaoTacId = table.Column<string>(nullable: true),
                    PermissionId = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Function_ThaoTacs", x => x.FTID);
                    table.ForeignKey(
                        name: "FK_Function_ThaoTacs_Functions_FunctionId",
                        column: x => x.FunctionId,
                        principalTable: "Functions",
                        principalColumn: "FunctionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Function_ThaoTacs_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Function_ThaoTacs_ThaoTacs_ThaoTacId",
                        column: x => x.ThaoTacId,
                        principalTable: "ThaoTacs",
                        principalColumn: "ThaoTacId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ThongBaoDieuChinhThongTinHoaDonChiTiets",
                columns: table => new
                {
                    ThongBaoDieuChinhThongTinHoaDonChiTietId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    ThongBaoDieuChinhThongTinHoaDonId = table.Column<string>(nullable: true),
                    MauHoaDonId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongBaoDieuChinhThongTinHoaDonChiTiets", x => x.ThongBaoDieuChinhThongTinHoaDonChiTietId);
                    table.ForeignKey(
                        name: "FK_ThongBaoDieuChinhThongTinHoaDonChiTiets_MauHoaDons_MauHoaDonId",
                        column: x => x.MauHoaDonId,
                        principalTable: "MauHoaDons",
                        principalColumn: "MauHoaDonId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ThongBaoDieuChinhThongTinHoaDonChiTiets_ThongBaoDieuChinhThongTinHoaDons_ThongBaoDieuChinhThongTinHoaDonId",
                        column: x => x.ThongBaoDieuChinhThongTinHoaDonId,
                        principalTable: "ThongBaoDieuChinhThongTinHoaDons",
                        principalColumn: "ThongBaoDieuChinhThongTinHoaDonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThongBaoKetQuaHuyHoaDonChiTiets",
                columns: table => new
                {
                    ThongBaoKetQuaHuyHoaDonChiTietId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    ThongBaoKetQuaHuyHoaDonId = table.Column<string>(nullable: true),
                    LoaiHoaDon = table.Column<int>(nullable: false),
                    MauHoaDonId = table.Column<string>(nullable: true),
                    TuSo = table.Column<int>(nullable: true),
                    DenSo = table.Column<int>(nullable: true),
                    SoLuong = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongBaoKetQuaHuyHoaDonChiTiets", x => x.ThongBaoKetQuaHuyHoaDonChiTietId);
                    table.ForeignKey(
                        name: "FK_ThongBaoKetQuaHuyHoaDonChiTiets_MauHoaDons_MauHoaDonId",
                        column: x => x.MauHoaDonId,
                        principalTable: "MauHoaDons",
                        principalColumn: "MauHoaDonId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ThongBaoKetQuaHuyHoaDonChiTiets_ThongBaoKetQuaHuyHoaDons_ThongBaoKetQuaHuyHoaDonId",
                        column: x => x.ThongBaoKetQuaHuyHoaDonId,
                        principalTable: "ThongBaoKetQuaHuyHoaDons",
                        principalColumn: "ThongBaoKetQuaHuyHoaDonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThongBaoPhatHanhChiTiets",
                columns: table => new
                {
                    ThongBaoPhatHanhChiTietId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    ThongBaoPhatHanhId = table.Column<string>(nullable: true),
                    MauHoaDonId = table.Column<string>(nullable: true),
                    KyHieu = table.Column<string>(nullable: true),
                    SoLuong = table.Column<int>(nullable: true),
                    TuSo = table.Column<int>(nullable: true),
                    DenSo = table.Column<int>(nullable: true),
                    NgayBatDauSuDung = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongBaoPhatHanhChiTiets", x => x.ThongBaoPhatHanhChiTietId);
                    table.ForeignKey(
                        name: "FK_ThongBaoPhatHanhChiTiets_MauHoaDons_MauHoaDonId",
                        column: x => x.MauHoaDonId,
                        principalTable: "MauHoaDons",
                        principalColumn: "MauHoaDonId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ThongBaoPhatHanhChiTiets_ThongBaoPhatHanhs_ThongBaoPhatHanhId",
                        column: x => x.ThongBaoPhatHanhId,
                        principalTable: "ThongBaoPhatHanhs",
                        principalColumn: "ThongBaoPhatHanhId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BienBanXoaBos",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    NgayBienBan = table.Column<DateTime>(nullable: true),
                    SoBienBan = table.Column<string>(nullable: true),
                    KhachHangId = table.Column<string>(nullable: true),
                    TenKhachHang = table.Column<string>(nullable: true),
                    MaSoThue = table.Column<string>(nullable: true),
                    SoDienThoai = table.Column<string>(nullable: true),
                    DaiDien = table.Column<string>(nullable: true),
                    ChucVu = table.Column<string>(nullable: true),
                    NgayKyBenB = table.Column<DateTime>(nullable: true),
                    TenCongTyBenA = table.Column<string>(nullable: true),
                    DiaChiBenA = table.Column<string>(nullable: true),
                    MaSoThueBenA = table.Column<string>(nullable: true),
                    SoDienThoaiBenA = table.Column<string>(nullable: true),
                    DaiDienBenA = table.Column<string>(nullable: true),
                    ChucVuBenA = table.Column<string>(nullable: true),
                    NgayKyBenA = table.Column<DateTime>(nullable: true),
                    HoaDonDienTuId = table.Column<string>(nullable: true),
                    LyDoXoaBo = table.Column<string>(nullable: true),
                    FileDaKy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BienBanXoaBos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BienBanXoaBos_HoaDonDienTus_HoaDonDienTuId",
                        column: x => x.HoaDonDienTuId,
                        principalTable: "HoaDonDienTus",
                        principalColumn: "HoaDonDienTuId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BienBanXoaBos_DoiTuongs_KhachHangId",
                        column: x => x.KhachHangId,
                        principalTable: "DoiTuongs",
                        principalColumn: "DoiTuongId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HoaDonDienTuChiTiets",
                columns: table => new
                {
                    HoaDonDienTuChiTietId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    HoaDonDienTuId = table.Column<string>(nullable: true),
                    HangHoaDichVuId = table.Column<string>(nullable: true),
                    MaHang = table.Column<string>(nullable: true),
                    TenHang = table.Column<string>(nullable: true),
                    DongChietKhau = table.Column<bool>(nullable: true),
                    DongMoTa = table.Column<bool>(nullable: true),
                    HangKhuyenMai = table.Column<bool>(nullable: true),
                    DonViTinhId = table.Column<string>(nullable: true),
                    SoLuong = table.Column<decimal>(nullable: true),
                    DonGia = table.Column<decimal>(nullable: true),
                    DonGiaSauThue = table.Column<decimal>(nullable: true),
                    DonGiaQuyDoi = table.Column<decimal>(nullable: true),
                    ThanhTien = table.Column<decimal>(nullable: true),
                    ThanhTienSauThue = table.Column<decimal>(nullable: true),
                    ThanhTienQuyDoi = table.Column<decimal>(nullable: true),
                    TyLeChietKhau = table.Column<decimal>(nullable: true),
                    TienChietKhau = table.Column<decimal>(nullable: true),
                    TienChietKhauQuyDoi = table.Column<decimal>(nullable: true),
                    ThueGTGT = table.Column<string>(nullable: true),
                    TienThueGTGT = table.Column<decimal>(nullable: true),
                    TienThueGTGTQuyDoi = table.Column<decimal>(nullable: true),
                    SoLo = table.Column<string>(nullable: true),
                    HanSuDung = table.Column<DateTime>(nullable: true),
                    SoKhung = table.Column<string>(nullable: true),
                    SoMay = table.Column<string>(nullable: true),
                    GhiChu = table.Column<string>(nullable: true),
                    TongTienThanhToan = table.Column<decimal>(nullable: true),
                    TongTienThanhToanQuyDoi = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDonDienTuChiTiets", x => x.HoaDonDienTuChiTietId);
                    table.ForeignKey(
                        name: "FK_HoaDonDienTuChiTiets_DonViTinhs_DonViTinhId",
                        column: x => x.DonViTinhId,
                        principalTable: "DonViTinhs",
                        principalColumn: "DonViTinhId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HoaDonDienTuChiTiets_HangHoaDichVus_HangHoaDichVuId",
                        column: x => x.HangHoaDichVuId,
                        principalTable: "HangHoaDichVus",
                        principalColumn: "HangHoaDichVuId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HoaDonDienTuChiTiets_HoaDonDienTus_HoaDonDienTuId",
                        column: x => x.HoaDonDienTuId,
                        principalTable: "HoaDonDienTus",
                        principalColumn: "HoaDonDienTuId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LuuTruTrangThaiFileHDDTs",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    HoaDonDienTuId = table.Column<string>(nullable: true),
                    PdfChuaKy = table.Column<byte[]>(nullable: true),
                    PdfDaKy = table.Column<byte[]>(nullable: true),
                    XMLChuaKy = table.Column<byte[]>(nullable: true),
                    XMLDaKy = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LuuTruTrangThaiFileHDDTs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LuuTruTrangThaiFileHDDTs_HoaDonDienTus_HoaDonDienTuId",
                        column: x => x.HoaDonDienTuId,
                        principalTable: "HoaDonDienTus",
                        principalColumn: "HoaDonDienTuId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ThongTinChuyenDois",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    HoaDonDienTuId = table.Column<string>(nullable: true),
                    NgayChuyenDoi = table.Column<DateTime>(nullable: false),
                    NguoiChuyenDoiId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongTinChuyenDois", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThongTinChuyenDois_HoaDonDienTus_HoaDonDienTuId",
                        column: x => x.HoaDonDienTuId,
                        principalTable: "HoaDonDienTus",
                        principalColumn: "HoaDonDienTuId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ThongTinChuyenDois_DoiTuongs_NguoiChuyenDoiId",
                        column: x => x.NguoiChuyenDoiId,
                        principalTable: "DoiTuongs",
                        principalColumn: "DoiTuongId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Function_Users",
                columns: table => new
                {
                    FUID = table.Column<string>(maxLength: 36, nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    FunctionId = table.Column<string>(nullable: true),
                    PermissionId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Function_Users", x => x.FUID);
                    table.ForeignKey(
                        name: "FK_Function_Users_Functions_FunctionId",
                        column: x => x.FunctionId,
                        principalTable: "Functions",
                        principalColumn: "FunctionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Function_Users_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Function_Users_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NhatKyThaoTacHoaDons",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    HoaDonDienTuId = table.Column<string>(nullable: true),
                    NgayGio = table.Column<DateTime>(nullable: false),
                    KhachHangId = table.Column<string>(nullable: true),
                    MoTa = table.Column<string>(nullable: true),
                    ErrorMessage = table.Column<string>(nullable: true),
                    HasError = table.Column<bool>(nullable: false),
                    LoaiThaoTac = table.Column<int>(nullable: false),
                    NguoiThucHienId = table.Column<string>(nullable: true),
                    DiaChiIp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhatKyThaoTacHoaDons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NhatKyThaoTacHoaDons_HoaDonDienTus_HoaDonDienTuId",
                        column: x => x.HoaDonDienTuId,
                        principalTable: "HoaDonDienTus",
                        principalColumn: "HoaDonDienTuId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NhatKyThaoTacHoaDons_DoiTuongs_KhachHangId",
                        column: x => x.KhachHangId,
                        principalTable: "DoiTuongs",
                        principalColumn: "DoiTuongId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NhatKyThaoTacHoaDons_Users_NguoiThucHienId",
                        column: x => x.NguoiThucHienId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User_Roles",
                columns: table => new
                {
                    URID = table.Column<string>(maxLength: 36, nullable: false),
                    RoleId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Roles", x => x.URID);
                    table.ForeignKey(
                        name: "FK_User_Roles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_Roles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LuuTruTrangThaiBBXBs",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    BienBanXoaBoId = table.Column<string>(nullable: true),
                    PdfChuaKy = table.Column<byte[]>(nullable: true),
                    PdfDaKy = table.Column<byte[]>(nullable: true),
                    XMLChuaKy = table.Column<byte[]>(nullable: true),
                    XMLDaKy = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LuuTruTrangThaiBBXBs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LuuTruTrangThaiBBXBs_BienBanXoaBos_BienBanXoaBoId",
                        column: x => x.BienBanXoaBoId,
                        principalTable: "BienBanXoaBos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BienBanXoaBos_HoaDonDienTuId",
                table: "BienBanXoaBos",
                column: "HoaDonDienTuId");

            migrationBuilder.CreateIndex(
                name: "IX_BienBanXoaBos_KhachHangId",
                table: "BienBanXoaBos",
                column: "KhachHangId");

            migrationBuilder.CreateIndex(
                name: "IX_Function_Roles_FunctionId",
                table: "Function_Roles",
                column: "FunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_Function_Roles_PermissionId",
                table: "Function_Roles",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Function_Roles_RoleId",
                table: "Function_Roles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Function_ThaoTacs_FunctionId",
                table: "Function_ThaoTacs",
                column: "FunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_Function_ThaoTacs_PermissionId",
                table: "Function_ThaoTacs",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Function_ThaoTacs_ThaoTacId",
                table: "Function_ThaoTacs",
                column: "ThaoTacId");

            migrationBuilder.CreateIndex(
                name: "IX_Function_Users_FunctionId",
                table: "Function_Users",
                column: "FunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_Function_Users_PermissionId",
                table: "Function_Users",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Function_Users_UserId",
                table: "Function_Users",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HangHoaDichVus_DonViTinhId",
                table: "HangHoaDichVus",
                column: "DonViTinhId");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTuChiTiets_DonViTinhId",
                table: "HoaDonDienTuChiTiets",
                column: "DonViTinhId");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTuChiTiets_HangHoaDichVuId",
                table: "HoaDonDienTuChiTiets",
                column: "HangHoaDichVuId");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTuChiTiets_HoaDonDienTuId",
                table: "HoaDonDienTuChiTiets",
                column: "HoaDonDienTuId");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTus_HinhThucThanhToanId",
                table: "HoaDonDienTus",
                column: "HinhThucThanhToanId");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTus_KhachHangId",
                table: "HoaDonDienTus",
                column: "KhachHangId");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTus_LoaiTienId",
                table: "HoaDonDienTus",
                column: "LoaiTienId");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTus_MauHoaDonId",
                table: "HoaDonDienTus",
                column: "MauHoaDonId");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTus_NguoiLapId",
                table: "HoaDonDienTus",
                column: "NguoiLapId");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTus_NhanVienBanHangId",
                table: "HoaDonDienTus",
                column: "NhanVienBanHangId");

            migrationBuilder.CreateIndex(
                name: "IX_LuuTruTrangThaiBBXBs_BienBanXoaBoId",
                table: "LuuTruTrangThaiBBXBs",
                column: "BienBanXoaBoId");

            migrationBuilder.CreateIndex(
                name: "IX_LuuTruTrangThaiFileHDDTs_HoaDonDienTuId",
                table: "LuuTruTrangThaiFileHDDTs",
                column: "HoaDonDienTuId");

            migrationBuilder.CreateIndex(
                name: "IX_MauHoaDonThietLapMacDinhs_MauHoaDonId",
                table: "MauHoaDonThietLapMacDinhs",
                column: "MauHoaDonId",
                unique: true,
                filter: "[MauHoaDonId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_NhatKyThaoTacHoaDons_HoaDonDienTuId",
                table: "NhatKyThaoTacHoaDons",
                column: "HoaDonDienTuId");

            migrationBuilder.CreateIndex(
                name: "IX_NhatKyThaoTacHoaDons_KhachHangId",
                table: "NhatKyThaoTacHoaDons",
                column: "KhachHangId");

            migrationBuilder.CreateIndex(
                name: "IX_NhatKyThaoTacHoaDons_NguoiThucHienId",
                table: "NhatKyThaoTacHoaDons",
                column: "NguoiThucHienId");

            migrationBuilder.CreateIndex(
                name: "IX_ThongBaoDieuChinhThongTinHoaDonChiTiets_MauHoaDonId",
                table: "ThongBaoDieuChinhThongTinHoaDonChiTiets",
                column: "MauHoaDonId");

            migrationBuilder.CreateIndex(
                name: "IX_ThongBaoDieuChinhThongTinHoaDonChiTiets_ThongBaoDieuChinhThongTinHoaDonId",
                table: "ThongBaoDieuChinhThongTinHoaDonChiTiets",
                column: "ThongBaoDieuChinhThongTinHoaDonId");

            migrationBuilder.CreateIndex(
                name: "IX_ThongBaoKetQuaHuyHoaDonChiTiets_MauHoaDonId",
                table: "ThongBaoKetQuaHuyHoaDonChiTiets",
                column: "MauHoaDonId");

            migrationBuilder.CreateIndex(
                name: "IX_ThongBaoKetQuaHuyHoaDonChiTiets_ThongBaoKetQuaHuyHoaDonId",
                table: "ThongBaoKetQuaHuyHoaDonChiTiets",
                column: "ThongBaoKetQuaHuyHoaDonId");

            migrationBuilder.CreateIndex(
                name: "IX_ThongBaoPhatHanhChiTiets_MauHoaDonId",
                table: "ThongBaoPhatHanhChiTiets",
                column: "MauHoaDonId");

            migrationBuilder.CreateIndex(
                name: "IX_ThongBaoPhatHanhChiTiets_ThongBaoPhatHanhId",
                table: "ThongBaoPhatHanhChiTiets",
                column: "ThongBaoPhatHanhId");

            migrationBuilder.CreateIndex(
                name: "IX_ThongTinChuyenDois_HoaDonDienTuId",
                table: "ThongTinChuyenDois",
                column: "HoaDonDienTuId");

            migrationBuilder.CreateIndex(
                name: "IX_ThongTinChuyenDois_NguoiChuyenDoiId",
                table: "ThongTinChuyenDois",
                column: "NguoiChuyenDoiId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Roles_RoleId",
                table: "User_Roles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Roles_UserId",
                table: "User_Roles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfigNoiDungEmails");

            migrationBuilder.DropTable(
                name: "Function_Roles");

            migrationBuilder.DropTable(
                name: "Function_ThaoTacs");

            migrationBuilder.DropTable(
                name: "Function_Users");

            migrationBuilder.DropTable(
                name: "HoaDonDienTuChiTiets");

            migrationBuilder.DropTable(
                name: "HoSoHDDTs");

            migrationBuilder.DropTable(
                name: "KyKeToans");

            migrationBuilder.DropTable(
                name: "LuuTruTrangThaiBBXBs");

            migrationBuilder.DropTable(
                name: "LuuTruTrangThaiFileHDDTs");

            migrationBuilder.DropTable(
                name: "MauHoaDonThietLapMacDinhs");

            migrationBuilder.DropTable(
                name: "NhatKyGuiEmails");

            migrationBuilder.DropTable(
                name: "NhatKyThaoTacHoaDons");

            migrationBuilder.DropTable(
                name: "NhatKyTruyCaps");

            migrationBuilder.DropTable(
                name: "TaiLieuDinhKems");

            migrationBuilder.DropTable(
                name: "ThongBaoDieuChinhThongTinHoaDonChiTiets");

            migrationBuilder.DropTable(
                name: "ThongBaoKetQuaHuyHoaDonChiTiets");

            migrationBuilder.DropTable(
                name: "ThongBaoPhatHanhChiTiets");

            migrationBuilder.DropTable(
                name: "ThongTinChuyenDois");

            migrationBuilder.DropTable(
                name: "TuyChons");

            migrationBuilder.DropTable(
                name: "User_Roles");

            migrationBuilder.DropTable(
                name: "ViewThaoTacs");

            migrationBuilder.DropTable(
                name: "ThaoTacs");

            migrationBuilder.DropTable(
                name: "Functions");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "HangHoaDichVus");

            migrationBuilder.DropTable(
                name: "BienBanXoaBos");

            migrationBuilder.DropTable(
                name: "ThongBaoDieuChinhThongTinHoaDons");

            migrationBuilder.DropTable(
                name: "ThongBaoKetQuaHuyHoaDons");

            migrationBuilder.DropTable(
                name: "ThongBaoPhatHanhs");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "DonViTinhs");

            migrationBuilder.DropTable(
                name: "HoaDonDienTus");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "HinhThucThanhToans");

            migrationBuilder.DropTable(
                name: "DoiTuongs");

            migrationBuilder.DropTable(
                name: "LoaiTiens");

            migrationBuilder.DropTable(
                name: "MauHoaDons");
        }
    }
}
