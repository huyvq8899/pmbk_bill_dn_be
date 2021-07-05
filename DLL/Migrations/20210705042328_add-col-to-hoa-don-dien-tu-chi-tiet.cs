using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addcoltohoadondientuchitiet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TenNhanVienBanHang",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DongChietKhau",
                table: "HoaDonDienTuChiTiets",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DongMoTa",
                table: "HoaDonDienTuChiTiets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenNhanVienBanHang",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "DongChietKhau",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropColumn(
                name: "DongMoTa",
                table: "HoaDonDienTuChiTiets");
        }
    }
}
