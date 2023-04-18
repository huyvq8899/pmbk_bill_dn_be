using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class AddForTabThongTinHoaDon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileId103Import",
                table: "HoSoHDDTs",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsNhapKhauMaCQT",
                table: "HoSoHDDTs",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MaThongDiepChuaMCQT",
                table: "HoSoHDDTs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThongBaoChiTietMaCuaCQT",
                table: "HoSoHDDTs",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SinhSoHDMayTinhTiens",
                columns: table => new
                {
                    SinhSoHDMayTinhTienId = table.Column<Guid>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    NamPhatHanh = table.Column<int>(nullable: false),
                    SoBatDau = table.Column<long>(nullable: false),
                    SoKetThuc = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SinhSoHDMayTinhTiens", x => x.SinhSoHDMayTinhTienId);
                });
            migrationBuilder.Sql(@"Update QuanLyThongTinHoaDons set STT = STT + 1
                                   where LoaiThongTin = 1 and STT > 1.9 and STT < 3.0

                                   Insert into QuanLyThongTinHoaDons Values (NEWID(), 2, 1, 9, 1, NULL, NULL, NULL, NULL)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SinhSoHDMayTinhTiens");

            migrationBuilder.DropColumn(
                name: "FileId103Import",
                table: "HoSoHDDTs");

            migrationBuilder.DropColumn(
                name: "IsNhapKhauMaCQT",
                table: "HoSoHDDTs");

            migrationBuilder.DropColumn(
                name: "MaThongDiepChuaMCQT",
                table: "HoSoHDDTs");

            migrationBuilder.DropColumn(
                name: "ThongBaoChiTietMaCuaCQT",
                table: "HoSoHDDTs");
        }
    }
}
