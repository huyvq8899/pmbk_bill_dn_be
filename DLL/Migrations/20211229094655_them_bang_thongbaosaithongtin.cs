using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class them_bang_thongbaosaithongtin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ThongBaoSaiThongTins",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    HoaDonDienTuId = table.Column<string>(maxLength: 36, nullable: true),
                    DoiTuongId = table.Column<string>(maxLength: 36, nullable: true),
                    HoTenNguoiMuaHang_Sai = table.Column<string>(maxLength: 200, nullable: true),
                    HoTenNguoiMuaHang_Dung = table.Column<string>(maxLength: 200, nullable: true),
                    TenDonVi_Sai = table.Column<string>(maxLength: 1000, nullable: true),
                    TenDonVi_Dung = table.Column<string>(maxLength: 1000, nullable: true),
                    DiaChi_Sai = table.Column<string>(maxLength: 1000, nullable: true),
                    DiaChi_Dung = table.Column<string>(maxLength: 1000, nullable: true),
                    TenNguoiNhan = table.Column<string>(maxLength: 200, nullable: true),
                    EmailCuaNguoiNhan = table.Column<string>(maxLength: 300, nullable: true),
                    EmailCCCuaNguoiNhan = table.Column<string>(maxLength: 300, nullable: true),
                    EmailBCCCuaNguoiNhan = table.Column<string>(maxLength: 300, nullable: true),
                    SDTCuaNguoiNhan = table.Column<string>(maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 36, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongBaoSaiThongTins", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThongBaoSaiThongTins");
        }
    }
}
