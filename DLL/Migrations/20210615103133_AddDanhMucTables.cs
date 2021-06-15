using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class AddDanhMucTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    Email = table.Column<string>(nullable: true),
                    SoDienThoai = table.Column<string>(nullable: true),
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
                    DiaChi = table.Column<string>(nullable: true),
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

            migrationBuilder.CreateIndex(
                name: "IX_HangHoaDichVus_DonViTinhId",
                table: "HangHoaDichVus",
                column: "DonViTinhId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoiTuongs");

            migrationBuilder.DropTable(
                name: "HangHoaDichVus");

            migrationBuilder.DropTable(
                name: "LoaiTiens");

            migrationBuilder.DropTable(
                name: "DonViTinhs");
        }
    }
}
