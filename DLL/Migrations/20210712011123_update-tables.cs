using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class updatetables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailNguoiNhanHD",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileChuaKy",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileDaKy",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoTenNguoiNhanHD",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SoDienThoaiNguoiNhanHD",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThueGTGT",
                table: "HoaDonDienTuChiTiets",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TyLeChietKhau",
                table: "HoaDonDienTuChiTiets",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LuuTruTrangThaiFileHDDTs",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    HoaDonDienTuId = table.Column<string>(nullable: true),
                    PdfChuaKy = table.Column<string>(nullable: true),
                    PdfDaKy = table.Column<string>(nullable: true),
                    XMLChuaKy = table.Column<string>(nullable: true),
                    XMLDaKy = table.Column<string>(nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_LuuTruTrangThaiFileHDDTs_HoaDonDienTuId",
                table: "LuuTruTrangThaiFileHDDTs",
                column: "HoaDonDienTuId");

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
                name: "IX_ThongTinChuyenDois_HoaDonDienTuId",
                table: "ThongTinChuyenDois",
                column: "HoaDonDienTuId");

            migrationBuilder.CreateIndex(
                name: "IX_ThongTinChuyenDois_NguoiChuyenDoiId",
                table: "ThongTinChuyenDois",
                column: "NguoiChuyenDoiId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LuuTruTrangThaiFileHDDTs");

            migrationBuilder.DropTable(
                name: "NhatKyThaoTacHoaDons");

            migrationBuilder.DropTable(
                name: "ThongTinChuyenDois");

            migrationBuilder.DropColumn(
                name: "EmailNguoiNhanHD",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "FileChuaKy",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "FileDaKy",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "HoTenNguoiNhanHD",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "SoDienThoaiNguoiNhanHD",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "ThueGTGT",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropColumn(
                name: "TyLeChietKhau",
                table: "HoaDonDienTuChiTiets");
        }
    }
}
