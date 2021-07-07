using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class AddThongBaoPhatHanh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoaiHoaDon",
                table: "MauHoaDons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LoaiKhoGiay",
                table: "MauHoaDons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LoaiMauHoaDon",
                table: "MauHoaDons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LoaiNgonNgu",
                table: "MauHoaDons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LoaiThueGTGT",
                table: "MauHoaDons",
                nullable: false,
                defaultValue: 0);

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
                    TenDonViPhatHanh = table.Column<string>(nullable: true),
                    MaSoThue = table.Column<string>(nullable: true),
                    DiaChiTruSo = table.Column<string>(nullable: true),
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

            migrationBuilder.CreateIndex(
                name: "IX_ThongBaoPhatHanhChiTiets_MauHoaDonId",
                table: "ThongBaoPhatHanhChiTiets",
                column: "MauHoaDonId");

            migrationBuilder.CreateIndex(
                name: "IX_ThongBaoPhatHanhChiTiets_ThongBaoPhatHanhId",
                table: "ThongBaoPhatHanhChiTiets",
                column: "ThongBaoPhatHanhId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThongBaoPhatHanhChiTiets");

            migrationBuilder.DropTable(
                name: "ThongBaoPhatHanhs");

            migrationBuilder.DropColumn(
                name: "LoaiHoaDon",
                table: "MauHoaDons");

            migrationBuilder.DropColumn(
                name: "LoaiKhoGiay",
                table: "MauHoaDons");

            migrationBuilder.DropColumn(
                name: "LoaiMauHoaDon",
                table: "MauHoaDons");

            migrationBuilder.DropColumn(
                name: "LoaiNgonNgu",
                table: "MauHoaDons");

            migrationBuilder.DropColumn(
                name: "LoaiThueGTGT",
                table: "MauHoaDons");
        }
    }
}
