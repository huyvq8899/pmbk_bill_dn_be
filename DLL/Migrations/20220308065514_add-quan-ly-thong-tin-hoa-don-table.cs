using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addquanlythongtinhoadontable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuanLyThongTinHoaDons",
                columns: table => new
                {
                    QuanLyThongTinHoaDonId = table.Column<string>(maxLength: 36, nullable: false),
                    STT = table.Column<float>(nullable: false),
                    LoaiThongTin = table.Column<int>(nullable: false),
                    LoaiThongTinChiTiet = table.Column<int>(nullable: false),
                    TrangThaiSuDung = table.Column<int>(nullable: false),
                    NgayBatDauSuDung = table.Column<DateTime>(nullable: true),
                    TuNgayTamNgungSuDung = table.Column<DateTime>(nullable: true),
                    DenNgayTamNgungSuDung = table.Column<DateTime>(nullable: true),
                    NgayNgungSuDung = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuanLyThongTinHoaDons", x => x.QuanLyThongTinHoaDonId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuanLyThongTinHoaDons");
        }
    }
}
