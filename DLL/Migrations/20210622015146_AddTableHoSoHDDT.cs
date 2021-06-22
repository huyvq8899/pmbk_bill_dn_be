using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class AddTableHoSoHDDT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HoSoHDDTs");
        }
    }
}
